import { Component, OnInit, Injector } from '@angular/core';
import { FormComponentBase } from '@shared/form-component-base';
import { ProjectDetailDto, ProjectDetailsServiceProxy, AccountServiceProxy, ProjectServiceProxy, LookupDtoOfInt32 } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import * as moment from 'moment';
import { validateVerticalPosition } from '@angular/cdk/overlay';
import { AppConsts } from '@shared/AppConsts';

export interface SelectOption {
    id: string;
    name: string;
}


@Component({
    selector: 'app-config-project-data',
    templateUrl: './config-project-data.component.html',
    styleUrls: ['./config-project-data.component.scss']
})
export class ConfigProjectDataComponent extends FormComponentBase<ProjectDetailDto> implements OnInit {


    Projects: LookupDtoOfInt32[];// = [{ id: '1', name: "1111" }, { id: '2', name: "2222" }, { id: '3', name: "3333" },];
    selectedProject: string;
    ProjectDetailModel: ProjectDetailDto;
    static readonly MAXINT = 36725413;
    baseServerUrl: string;
    originalFileUrl: string;

    constructor(
        route: ActivatedRoute,
        fb: FormBuilder,
        service: ProjectDetailsServiceProxy,
        projectService: ProjectServiceProxy,
        injector: Injector,
        router:Router) {
        super(route, fb, service, injector,router);
        this.model = new ProjectDetailDto();
        this.baseServerUrl = AppConsts.remoteServiceBaseUrl;
        projectService.getLookup().subscribe(res=> {
            this.Projects=res;
        })
    }

    ngOnInit() {

        super.ngOnInit();

        this.formGroup = this.fb.group({
            projectId: ['', [Validators.required]],
            optimisticIteration: ['', [Validators.required, Validators.min(1), Validators.max(ConfigProjectDataComponent.MAXINT)]],
            pessimisticIteration: ['', [Validators.required, Validators.min(1), Validators.max(ConfigProjectDataComponent.MAXINT)]],
            descriptions: ['', [Validators.maxLength(500)]]
        });
    }

    //ngAfterViewInit() {
    //this.select.optionSelectionChanges.subscribe(res => {console.log(res)});
    //}

    // load(dataModel) {
    //   this.formGroup.setValue({
    //       projectId: dataModel.projectId,
    //       optimisticIteration: dataModel.optimisticIteration,
    //       pessimisticIteration: dataModel.pessimisticIteration,
    //       creationDate: dataModel.creationDate.toISOString(),
    //       scope: dataModel.scope,
    //       logo: dataModel.logo,
    //       descriptions: dataModel.descriptions,
    //       pullDataFromDate: dataModel.pullDataFromDate.toISOString(),
    //       pullDataToDate: dataModel.pullDataToDate.toISOString(),
    //   });
    // }

    // gettingTheModelReady() {
    //   return this.model = new ProjectDetailDto({
    //     'projectId': this.isEditing ? this.model.projectId : this.formGroup.get('projectId').value,
    //     'optimisticIteration': this.formGroup.get('optimisticIteration').value,
    //     'pessimisticIteration': this.formGroup.get('pessimisticIteration').value,
    //     'creationDate': moment(this.formGroup.get('creationDate').value.toString()),
    //     'scope': this.formGroup.get('scope').value,
    //     'logo': this.formGroup.get('logo').value,
    //     'descriptions': this.formGroup.get('descriptions').value,
    //     'pullDataFromDate': moment(this.formGroup.get('pullDataFromDate').value.toString()),
    //     'pullDataToDate': moment(this.formGroup.get('pullDataToDate').value.toString()),
    //     'id': this.isEditing ? this.model.id : undefined
    //   });
    // }

    projectddlChange($event) {
        let selectedProject = this.formGroup.controls['projectId'].value;
        console.log($event, selectedProject);
        if (selectedProject) {
            this.service.getAll(`projectId eq ${selectedProject}`).subscribe(res => {
                if (res.totalCount > 0) {
                    console.log("res", res.items[0]);
                    this.fillForm(res.items[0])
                }
            });
        }
        else {
            this.fillForm(null);
        }
    }

    fillForm(dto: ProjectDetailDto) {
        //this.formGroup.controls['projectId'].setValue(this.selectedProject);
        this.ProjectDetailModel = dto;
        this.formGroup.patchValue({
            optimisticIteration: dto ? dto.optimisticIteration : null,
            pessimisticIteration: dto ? dto.pessimisticIteration : null,
            descriptions: dto ? dto.descriptions : null
        });
        this.originalFileUrl = dto.logo;
    }


    save() {

        if (this.formGroup.valid && this.ProjectDetailModel) {
            this.ProjectDetailModel.optimisticIteration = +this.formGroup.get('optimisticIteration').value,
                this.ProjectDetailModel.pessimisticIteration = +this.formGroup.get('pessimisticIteration').value,
                this.ProjectDetailModel.descriptions = this.formGroup.get('descriptions').value
            this.service.update(this.ProjectDetailModel).subscribe(res => {
                console.log("res", res);
                abp.message.success("Updated Successfully");
            });
        }
    }

    get staticMaxInt() {
        return ConfigProjectDataComponent.MAXINT;
    }

    uploadFinished(fileUrl) {
        this.ProjectDetailModel.logo = fileUrl;
    }
}
