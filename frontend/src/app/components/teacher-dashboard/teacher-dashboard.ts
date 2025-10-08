import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TeacherReports } from '../teacher-reports/teacher-reports';
import { TeacherFeedback } from '../teacher-feedback/teacher-feedback';
import { TeacherAttendance } from '../teacher-attendance/teacher-attendance';
import { TeacherGrades } from '../teacher-grades/teacher-grades';
import { TeacherStudents } from '../teacher-students/teacher-students';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-teacher-dashboard',
  standalone: true,
  imports: [ 
    CommonModule, 
    TeacherStudents,
    TeacherGrades,
    TeacherAttendance,
    TeacherFeedback,
    TeacherReports,
  ],
  templateUrl: './teacher-dashboard.html',
  styleUrls: ['./teacher-dashboard.css']
})
export class TeacherDashboard {
  isCollapsed = false;
  activeMenu: string = 'dashboard';

  constructor(private router: Router) {}

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
  }

  selectMenu(menu: string) {
    if (menu === 'logout') {
      // Clear stored data
      localStorage.clear();
      sessionStorage.clear();

      // Redirect to login page
      this.router.navigate(['/login']);
    } else {
      this.activeMenu = menu;
    }
  }
}
