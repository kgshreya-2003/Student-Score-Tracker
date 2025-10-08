import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HttpParams } from '@angular/common/http';

interface Student {
  id: number;
  name: string;
  class: string;
  roll: number;
  section: string;
}

interface AttendanceRecord {
  id?: number;
  studentId: number;
  date: string;
  status: string;
}

@Component({
  selector: 'app-teacher-attendance',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './teacher-attendance.html',
  styleUrls: ['./teacher-attendance.css'],
})
export class TeacherAttendance implements OnInit {
  classes = ['1st', '2nd', '3rd', '4th', '5th'];
  sections = ['A', 'B', 'C'];
  subjects = ['English', 'Math', 'Science'];

  selectedClass = '';
  selectedSection = '';
  selectedSubject = '';
  selectedDate: string = '';

  students: Student[] = [];
  attendanceRecord: { [key: number]: string } = {};

  private studentApiUrl = 'https://localhost:7128/api/Students';
  private attendanceApiUrl = 'https://localhost:7128/api/Attendance';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {}

 
  fetchStudents(): void {
    if (!this.selectedClass || !this.selectedSection) {
      alert('⚠️ Please select both class and section.');
      return;
    }

    const params = new HttpParams()
      .set('class', this.selectedClass)
      .set('section', this.selectedSection);

    this.http.get<Student[]>(this.studentApiUrl, { params }).subscribe({
      next: (data) => {
        this.students = data;
        this.attendanceRecord = {};
        console.log('✅ Students fetched:', data);
      },
      error: (err) => {
        console.error('❌ Error fetching students:', err);
        alert('Failed to load students.');
      },
    });
  }

  
  markAllPresent(): void {
    this.students.forEach((s) => (this.attendanceRecord[s.id] = 'Present'));
  }

submitAttendance(): void {
  if (!this.selectedDate) {
    alert('⚠️ Please select a date.');
    return;
  }

  const attendanceList: AttendanceRecord[] = this.students.map((student) => ({
    studentId: student.id,
    date: this.selectedDate,
    status: this.attendanceRecord[student.id] || 'Absent',
  }));

  attendanceList.forEach((record) => {
    this.http.post(this.attendanceApiUrl, record).subscribe({
      next: () => console.log(`✅ Saved for Student ID: ${record.studentId}`),
      error: (err) => {
        console.error(`❌ Error for ${record.studentId}:`, err);
        alert(`Failed for Student ID ${record.studentId}. Check console.`);
      },
    });
  });
}


  updateAttendance(studentId: number): void {
    if (!this.selectedDate) {
      alert('⚠️ Please select a date before updating.');
      return;
    }

    const newStatus = this.attendanceRecord[studentId];
    if (!newStatus) {
      alert('⚠️ Please mark attendance before updating.');
      return;
    }

    this.http.get<AttendanceRecord[]>(this.attendanceApiUrl).subscribe({
      next: (records) => {
        const existing = records.find(
          (r) =>
            r.studentId === studentId &&
            r.date.split('T')[0] === this.selectedDate
        );

        if (!existing) {
          alert('⚠️ No attendance record found. Please submit first.');
          return;
        }

        const updatedRecord: AttendanceRecord = {
          id: existing.id,
          studentId: studentId,
          date: this.selectedDate,
          status: newStatus,
        };

        this.http.put(`${this.attendanceApiUrl}/${existing.id}`, updatedRecord)
          .subscribe({
            next: () => {
              console.log(`✅ Updated for Student ID: ${studentId}`);
              alert('Attendance updated successfully!');
            },
            error: (err) => {
              console.error('❌ Error updating attendance:', err);
              alert('Failed to update attendance.');
            },
          });
      },
      error: (err) => {
        console.error('❌ Error fetching attendance:', err);
        alert('Failed to fetch existing records.');
      },
    });
  }


  markAsLate(studentId: number): void {
    this.attendanceRecord[studentId] = 'Late';
    this.updateAttendance(studentId);
  }
}
