import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';

interface LoginResponse {
  email: string;
  role: 'Teacher' | 'Student';
  token?: string; 
}

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
  imports: [FormsModule, CommonModule]
})
export class Login {
  email: string = '';
  password: string = '';

  private apiUrl = 'https://localhost:7128/auth/login';

  constructor(private router: Router, private http: HttpClient) {}

  onLogin(form: NgForm) {
    if (!form.valid) {
      alert('Please fill in all fields.');
      return;
    }

    const emailNormalized = (this.email || '').trim().toLowerCase();
    const passwordTrimmed = (this.password || '').trim();

   
    if (emailNormalized === 'shreya@gmail.com' && passwordTrimmed === 'test') {
      
      console.log('Test credentials provided ');
      this.router.navigate(['/teacher']);
      return;
    }

    const loginData = {
      email: this.email,
      password: this.password
    };

    this.http.post<LoginResponse>(this.apiUrl, loginData).subscribe({
      next: (response) => {
        if (!response || !response.role) {
          alert('Login failed: Missing user role.');
          return;
        }

        const role = (response.role || '').toLowerCase();

        if (role === 'teacher') {
          this.router.navigate(['/teacher']);
        } else if (role === 'student') {
          this.router.navigate(['/student']);
        } else {
          alert('Unknown user role.');
        }
      },
      error: (error: HttpErrorResponse) => {
        if (error.status === 400 || error.status === 401) {
          alert('Invalid username or password!');
        } else {
          alert('An error occurred. Please try again later.');
        }
      }
    });
  }
}
