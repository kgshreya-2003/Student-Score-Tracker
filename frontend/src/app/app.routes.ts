import { RouterModule, Routes } from '@angular/router';
import { Login} from './components/login/login';
import { TeacherDashboard } from './components/teacher-dashboard/teacher-dashboard';
import { NgModule } from '@angular/core';


export const routes: Routes = [
    { path: 'login', component: Login },
    { path: 'teacher', component: TeacherDashboard},
  { path: '**', redirectTo: '' }
];
@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
  export class AppRoutingModule { }