import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HttpParams } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';

interface Student {
  id: number;
  name: string;
  class: string;
  roll: number;
  section: string;
}

interface StudentFeedback {
  feedbacks: string;
  subject: string;
  feedbackText: string;
}

@Component({
  selector: 'app-teacher-feedback',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './teacher-feedback.html',
  styleUrls: ['./teacher-feedback.css']
})
export class TeacherFeedback implements OnInit {
  classes: string[] = ['1st', '2nd', '3rd', '4th', '5th'];
  sections: string[] = ['A', 'B', 'C'];

  selectedClass = '';
  selectedSection = '';
  selectedDate = '';

  students: Student[] = [];
  feedbacks: { [studentId: number]: StudentFeedback } = {};

  isLoadingStudents = false;
  isSubmitting = false;

  private readonly studentApiUrl = 'https://localhost:7128/api/Students';
  private readonly feedbackApiUrl = 'https://localhost:7128/api/Feedback';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {}

  fetchStudents(): void {
    if (!this.selectedClass || !this.selectedSection) {
      alert('⚠️ Please select both class and section.');
      return;
    }

    this.isLoadingStudents = true;

    const params = new HttpParams()
      .set('class', this.selectedClass)
      .set('section', this.selectedSection);

    this.http.get<Student[]>(this.studentApiUrl, { params }).subscribe({
      next: (students) => {
        this.students = students;
        this.feedbacks = {};

        students.forEach(student => {
          this.feedbacks[student.id] = {
            feedbacks: '',
            subject: '',
            feedbackText: ''
          };
        });

        if (students.length === 0) {
          alert('⚠️ No students found for the selected class and section.');
        }

        console.log('✅ Students fetched:', students);
        this.isLoadingStudents = false;
      },
      error: (err) => {
        console.error('❌ Error fetching students:', err);
        alert('❌ Failed to fetch students. Please try again later.');
        this.isLoadingStudents = false;
      }
    });
  }

  async submitFeedback(): Promise<void> {
    if (!this.selectedDate) {
      alert('⚠️ Please select a date.');
      return;
    }

    if (this.students.length === 0) {
      alert('⚠️ No students to submit feedback for. Please fetch students first.');
      return;
    }

    const formattedDate = new Date(this.selectedDate).toISOString();

    const feedbackList = this.students.map(student => {
  const feedback = this.feedbacks[student.id];
  return {
    StudentId: student.id,
    Feedbacks: feedback.feedbacks.trim(),
    Date: formattedDate,
    ...(feedback.subject.trim() && { Subject: feedback.subject.trim() }),
    ...(feedback.feedbackText.trim() && { FeedbackText: feedback.feedbackText.trim() })
  };
});


    const incompleteFeedbacks = feedbackList.filter(f => !f.Feedbacks);
    if (incompleteFeedbacks.length > 0) {
      alert('⚠️ Please enter feedback for all students before submitting.');
      return;
    }

    this.isSubmitting = true;

    try {
      console.log('📤 Submitting feedback:', feedbackList);
      await lastValueFrom(this.http.post(this.feedbackApiUrl, feedbackList));
      alert('✅ Feedback submitted successfully!');
      this.feedbacks = {};
      this.selectedDate = '';
    } catch (error) {
      console.error('❌ Error submitting feedback:', error);
      alert('❌ Failed to submit feedback. Please try again later.');
    } finally {
      this.isSubmitting = false;
    }
  }

  canSubmit(): boolean {
    return (
      !!this.selectedDate &&
      this.students.length > 0 &&
      this.students.every(student => this.feedbacks[student.id]?.feedbacks.trim())
    );
  }
}
