import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { finalize } from 'rxjs/operators';

import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import * as XLSX from 'xlsx';

interface Student {
  id: number;
  name: string;
  class: string;
  roll: number;
  section: string;
}

interface ReportRow {
  subject: string;
  grade: string;
  attendance: string;
  feedback: string;
}

@Component({
  selector: 'app-teacher-reports',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './teacher-reports.html',
  styleUrls: ['./teacher-reports.css'],
})
export class TeacherReports implements OnInit, AfterViewInit {

  classes = ['1st', '2nd', '3rd', '4th', '5th'];
  sections = ['A', 'B', 'C'];

  selectedClass = '';
  selectedSection = '';
  selectedStudent = '';
  studentSearchTerm = '';

  students: Student[] = [];
  filteredStudents: Student[] = [];
  reportData: ReportRow[] = [];

  startDate = '';
  endDate = '';

  loadingStudents = false;
  loadingReport = false;
  errorLoadingStudents = '';
  errorLoadingReport = '';
  reportGenerated = false;

  private apiBase = 'https://localhost:7128/api';

  constructor(private http: HttpClient) {}

  ngOnInit() {}

  ngAfterViewInit() {}

  fetchStudents() {
    if (!this.selectedClass || !this.selectedSection) {
      alert('⚠️ Please select both Class and Section.');
      return;
    }

    this.loadingStudents = true;
    this.errorLoadingStudents = '';

    const apiUrl = `${this.apiBase}/Students?class=${this.selectedClass}&section=${this.selectedSection}`;
    this.http
      .get<Student[]>(apiUrl)
      .pipe(finalize(() => (this.loadingStudents = false)))
      .subscribe({
        next: (data) => {
          this.students = data;
          this.filteredStudents = [...data];
          if (!data.length)
            alert('⚠️ No students found for the selected class and section.');
        },
        error: (err) => {
          console.error('❌ Error fetching students:', err);
          this.errorLoadingStudents = 'Failed to load students.';
          alert('❌ Failed to load students. Please check backend.');
        },
      });
  }


  filterStudents() {
    const term = this.studentSearchTerm.toLowerCase();
    this.filteredStudents = this.students.filter((s) =>
      s.name.toLowerCase().includes(term)
    );
  }

  
  generateReport() {
    if (!this.selectedStudent) {
      alert('⚠️ Please select a student.');
      return;
    }

    this.loadingReport = true;
    this.errorLoadingReport = '';
    this.reportGenerated = false;

    let params = new HttpParams()
      .set('student', this.selectedStudent)
      .set('class', this.selectedClass)
      .set('section', this.selectedSection);

    if (this.startDate) params = params.set('startDate', this.startDate);
    if (this.endDate) params = params.set('endDate', this.endDate);

    this.http
      .get<ReportRow[]>(`${this.apiBase}/Reports`, { params })
      .pipe(finalize(() => (this.loadingReport = false)))
      .subscribe({
        next: (data) => {
          this.reportData = data;
          this.reportGenerated = true;
        },
        error: (err) => {
          console.error('❌ Generate Report Error:', err);
          this.errorLoadingReport = 'Failed to load report.';
          alert('❌ Failed to load report. Please check backend.');
        },
      });
  }

  downloadPDF() {
    if (!this.reportGenerated) return alert('Generate report first!');
    const doc = new jsPDF();
    doc.setFontSize(18);
    doc.text('Student Report', 14, 20);
    doc.setFontSize(12);
    doc.text(`Student: ${this.selectedStudent}`, 14, 30);

    autoTable(doc, {
      startY: 40,
      head: [['Subject', 'Grade', 'Attendance', 'Feedback']],
      body: this.reportData.map((r) => [
        r.subject,
        r.grade,
        r.attendance,
        r.feedback,
      ]),
      theme: 'grid',
      headStyles: { fillColor: [25, 135, 84] },
    });

    doc.save(`${this.selectedStudent}_report.pdf`);
  }

  
  downloadExcel() {
    if (!this.reportGenerated) return alert('Generate report first!');
    const ws = XLSX.utils.json_to_sheet(this.reportData);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Report');
    XLSX.writeFile(wb, `${this.selectedStudent}_report.xlsx`);
  }

  printReport() {
    if (!this.reportGenerated) return alert('Generate report first!');
    const printContents = document.getElementById('reportContent')?.innerHTML;
    if (!printContents) return;
    const win = window.open('', '', 'width=900,height=650');
    win?.document.write(
      `<html><head><title>Student Report</title></head><body>${printContents}</body></html>`
    );
    win?.document.close();
    win?.print();
  }

  gradeColorClass(grade: string): string {
    switch (grade) {
      case 'A+': return 'badge bg-success';
      case 'A': return 'badge bg-primary';
      case 'B': return 'badge bg-info text-dark';
      case 'C': return 'badge bg-warning text-dark';
      case 'D': return 'badge bg-secondary';
      case 'F': return 'badge bg-danger';
      default: return 'badge bg-light text-dark';
    }
  }
}
