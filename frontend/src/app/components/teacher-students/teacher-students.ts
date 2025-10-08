import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import * as XLSX from 'xlsx';
import FileSaver from 'file-saver';

interface Student {
  id: number;
  name: string;
  class: string;
  roll: number;
  section: string;
}

declare var bootstrap: any;

@Component({
  selector: 'app-teacher-students',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FormsModule],
  templateUrl: './teacher-students.html',
  styleUrls: ['./teacher-students.css'],
})
export class TeacherStudents implements OnInit {
  students: Student[] = [];
  isLoading = false;
  error: string = '';

  editingStudent: Student | null = null;
  newStudent: Partial<Student> = {
    name: '',
    class: '',
    roll: 0,
    section: '',
  };

  // Filters
  selectedClass: string = '';
  selectedStream: string = '';
  selectedSection: string = '';
  
 classes: string[] = [
  '1st',
  '2nd',
  '3rd',
  '4th',
  '5th',

];

  sections: string[] = ['A', 'B', 'C']; 

  searchText: string = '';

  private apiUrl = 'https://localhost:7128/api/Students';
  private modal: any;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.fetchStudents();
    const modalEl = document.getElementById('studentModal');
    if (modalEl) this.modal = new bootstrap.Modal(modalEl);
  }

  fetchStudents(): void {
    this.isLoading = true;

    const params: any = {};
    if (this.selectedClass) params.class = this.selectedClass;
    if (this.selectedStream) params.stream = this.selectedStream;
    if (this.selectedSection) params.section = this.selectedSection;

    this.http.get<Student[]>(this.apiUrl, { params }).subscribe({
      next: (data) => {
        // ✅ Sort by class and section
        this.students = data.sort((a, b) => {
          if (a.class !== b.class) return a.class.localeCompare(b.class);
          return a.section.localeCompare(b.section);
        });
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching students', err);
        this.error = 'Could not load students.';
        this.isLoading = false;
      },
    });
  }

  get filteredStudents(): Student[] {
    return this.students.filter((s) =>
      s.name.toLowerCase().includes(this.searchText.toLowerCase())
    );
  }

  openModal(): void {
    this.editingStudent = null;
    this.newStudent = { name: '', class: '', roll: 0, section: '' };
    this.modal.show();
  }

  startEdit(student: Student): void {
    this.editingStudent = { ...student };
    this.newStudent = {
      name: student.name,
      class: student.class,
      roll: student.roll,
      section: student.section,
    };
    this.modal.show();
  }

  addStudent(): void {
    if (!this.newStudent.name?.trim() || !this.newStudent.section) {
      alert('Name and section are required');
      return;
    }

    if (this.editingStudent) {
      const updatedStudent: Student = {
        id: this.editingStudent.id,
        name: this.newStudent.name.trim(),
        class: this.newStudent.class?.trim() || '',
        roll: this.newStudent.roll || 0,
        section: this.newStudent.section || '',
      };

      this.http.put(`${this.apiUrl}/${this.editingStudent.id}`, updatedStudent).subscribe({
        next: () => {
          this.fetchStudents();
          this.modal.hide();
          this.resetForm();
        },
        error: (err) => console.error('Error updating student', err),
      });
    } else {
      const newStudent: Omit<Student, 'id'> = {
        name: this.newStudent.name.trim(),
        class: this.newStudent.class?.trim() || '',
        roll: this.newStudent.roll || 0,
        section: this.newStudent.section || '',
      };

      this.http.post<Student>(this.apiUrl, newStudent).subscribe({
        next: () => {
          this.fetchStudents();
          this.modal.hide();
          this.resetForm();
        },
        error: (err) => console.error('Error adding student', err),
      });
    }
  }

  deleteStudent(id: number): void {
    if (confirm('Are you sure you want to delete this student?')) {
      this.http.delete(`${this.apiUrl}/${id}`).subscribe({
        next: () => this.fetchStudents(),
        error: (err) => console.error('Error deleting student', err),
      });
    }
  }

  cancel(): void {
    this.resetForm();
    this.modal.hide();
  }

  resetForm(): void {
    this.newStudent = { name: '', class: '', roll: 0, section: '' };
    this.editingStudent = null;
  }

  exportToExcel(): void {
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.filteredStudents);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Students');
    const excelBuffer: any = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
    const data: Blob = new Blob([excelBuffer], { type: 'application/octet-stream' });
    FileSaver.saveAs(data, 'Student_List.xlsx');
  }
}
