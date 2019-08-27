import { Component, OnInit, Injector } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProjectDetailsServiceProxy, ProjectDetailDto } from '@shared/service-proxies/service-proxies';
import { AppComponent } from '@app/app.component';
import { finalize } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { load } from '@angular/core/src/render3';
import { FormComponentBase } from '@shared/form-component-base';
import * as moment from 'moment';

@Component({
  selector: 'app-project-details-form',
  templateUrl: './project-details-form.component.html',
  styleUrls: ['./project-details-form.component.scss']
})


export class ProjectDetailsFormComponent extends FormComponentBase<ProjectDetailDto> implements OnInit {

  constructor(
      route: ActivatedRoute,
      fb: FormBuilder,
      service: ProjectDetailsServiceProxy,
      injector: Injector,
      router:Router) {

    super(route, fb, service, injector,router);
    this.model = new ProjectDetailDto();
  }

  ngOnInit() {

    super.ngOnInit();

    this.formGroup = this.fb.group({
      projectId: ['', [Validators.required]],
      optimisticIteration: ['', [Validators.required, Validators.maxLength(50)]],
      pessimisticIteration: ['', [Validators.required, Validators.maxLength(50)]],
      creationDate: ['', [Validators.required]],
      scope: ['', [Validators.required, Validators.maxLength(50)]],
      logo: ['', [Validators.required, Validators.maxLength(50)]],
      descriptions: ['', [Validators.required, Validators.maxLength(50)]],
      pullDataFromDate: ['', [Validators.required]],
      pullDataToDate: ['', [Validators.required]],
      id: ['']
    });
  }

  load(dataModel) {
    this.formGroup.setValue({
      projectId: dataModel.projectId,
      optimisticIteration: dataModel.optimisticIteration,
      pessimisticIteration: dataModel.pessimisticIteration,
      creationDate: dataModel.creationDate.toISOString(),
      scope: dataModel.scope,
      logo: dataModel.logo,
      descriptions: dataModel.descriptions,
      pullDataFromDate: dataModel.pullDataFromDate.toISOString(),
      pullDataToDate: dataModel.pullDataToDate.toISOString(),
      id: this.id
    });
  }
}
