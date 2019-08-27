import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectDetailsFormComponent } from './project-details-form.component';

describe('ProjectDetailsComponent', () => {
  let component: ProjectDetailsFormComponent;
  let fixture: ComponentFixture<ProjectDetailsFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ProjectDetailsFormComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectDetailsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
