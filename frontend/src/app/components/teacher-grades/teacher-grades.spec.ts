import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeacherGrades } from './teacher-grades';

describe('TeacherGrades', () => {
  let component: TeacherGrades;
  let fixture: ComponentFixture<TeacherGrades>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeacherGrades]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TeacherGrades);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
