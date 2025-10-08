import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgChartsModule } from 'ng2-charts';

@Component({
  selector: 'app-teacher-grades',
  standalone: true,
  imports: [CommonModule, FormsModule, NgChartsModule],
  templateUrl: './teacher-grades.html',
  styleUrls: ['./teacher-grades.css']
})
export class TeacherGrades implements OnInit {
  classes: string[] = ['1st', '2nd', '3rd', '4th', '5th'];
  sections: string[] = ['A', 'B', 'C'];
  subjects: string[] = ['Math', 'Science', 'English', 'Social'];

  selectedClass = '';
  selectedSection = '';
  selectedDate = '';

  students: any[] = [];

  pieChartOptions = {
    responsive: true,
    plugins: {
      legend: { position: 'bottom' }
    }
  };

  constructor(private http: HttpClient) {}

  ngOnInit(): void {}

  // When class is changed, reset section
  onClassChange() {
    this.selectedSection = '';
  }

  onSectionChange() {}

  fetchStudents() {
    if (!this.selectedClass || !this.selectedSection) {
      alert('Please select both class and section.');
      return;
    }

    const apiUrl = `https://localhost:7128/api/Students?class=${this.selectedClass}&section=${this.selectedSection}`;
    this.http.get<any[]>(apiUrl).subscribe({
      next: (data) => {
        this.students = data.map((s) => ({
          ...s,
          subject: '',
          marks: null,
          grade: '',
          marksData: {},
          chartData: { labels: this.subjects, datasets: [{ data: [0, 0, 0, 0] }] }
        }));
      },
      error: (err) => {
        console.error('❌ Error fetching students:', err);
        alert('Failed to load students. Please check backend connection.');
      }
    });
  }

  assignGrade(student: any) {
    const marks = student.marks || 0;

    if (marks >= 90) student.grade = 'A+';
    else if (marks >= 80) student.grade = 'A';
    else if (marks >= 70) student.grade = 'B+';
    else if (marks >= 60) student.grade = 'B';
    else student.grade = 'C';

    if (student.subject) {
      student.marksData[student.subject] = marks;
      this.updatePieChart(student);
    }
  }

  updatePieChart(student: any) {
    const dataPoints = this.subjects.map((subject) => student.marksData[subject] || 0);
    student.chartData = {
      labels: this.subjects,
      datasets: [{ data: dataPoints }]
    };
  }

  saveStudentMark(student: any) {
    if (!student.subject || student.marks == null) {
      alert('Please select a subject and enter marks before saving.');
      return;
    }

    const gradeData = {
      studentId: student.id,
      subject: student.subject,
      marks: student.marks,
      grade: student.grade,
      date: this.selectedDate || new Date().toISOString()
    };

    this.http.post('https://localhost:7128/api/Grades', gradeData).subscribe({
      next: (res) => {
        console.log('✅ Marks saved:', res);
        alert(`✅ Marks for ${student.name} (${student.subject}) saved successfully!`);
      },
      error: (err) => {
        console.error('❌ Error saving marks:', err);
        if (err.status === 400) {
          alert('⚠️ Invalid data sent. Please check subject, marks, and student details.');
        } else {
          alert('❌ Failed to save marks. Check backend server.');
        }
      }
    });
  }
}
