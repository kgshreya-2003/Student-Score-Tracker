import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeacherFeedback } from './teacher-feedback';

describe('TeacherFeedback', () => {
  let component: TeacherFeedback;
  let fixture: ComponentFixture<TeacherFeedback>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeacherFeedback]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TeacherFeedback);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
