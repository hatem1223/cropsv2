import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigProjectDataComponent } from './config-project-data.component';

describe('ConfigProjectDataComponent', () => {
  let component: ConfigProjectDataComponent;
  let fixture: ComponentFixture<ConfigProjectDataComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfigProjectDataComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfigProjectDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
