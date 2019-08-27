import { Component, OnInit, Injector } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProjectServiceProxy, ProjectDto, AccountServiceProxy, AccountDto } from '@shared/service-proxies/service-proxies';
import { AppComponent } from '@app/app.component';
import { finalize } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { load } from '@angular/core/src/render3';
import { FormComponentBase } from '@shared/form-component-base';
import * as moment from 'moment';


export interface SelectOption {
  id: string;
  name: string;
}



@Component({
  selector: 'app-project-form',
  templateUrl: './project-form.component.html',
  styleUrls: ['./project-form.component.scss']
})

export class ProjectFormComponent extends FormComponentBase<ProjectDto> implements OnInit {
  isOnPremProjectFlag:boolean=true;
  selectedAccount: string;
  ProjectModel: ProjectDto;  
  projectType :string=  "On-Premises";
  //accounts: SelectOption[] = [{id : '1', name: "1111"},{id : '2', name: "2222"},{id : '3', name: "3333"},];
  accounts: AccountDto[];
  authTypies: SelectOption[] = [{id : '1', name: "Alternative Password"},{id : '2', name: "Personal Access Token"}];
  regForURL :string = '(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?';
  //regForURL :string ="https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";
  constructor(
    route: ActivatedRoute,
    fb: FormBuilder,
    service: ProjectServiceProxy,
   private accountServiceProxy : AccountServiceProxy,
    injector: Injector,
    router:Router) {

      super(route, fb, service, injector,router);
      this.model = new ProjectDto();
  }

  ngOnInit() {
    this.formGroup = this.fb.group({
      projectName: ['', [Validators.required, Validators.maxLength(50)]],
      collectionName: ['', [Validators.required, Validators.maxLength(50)]],
      releaseName: ['', [Validators.required, Validators.maxLength(50)]],
      accountId: ['', [Validators.required]],
      workspaceName: ['', [Validators.required, Validators.maxLength(50)]],
    });

    this.accountServiceProxy.getAll().subscribe(res => {
      if (res.totalCount > 0) {
          this.accounts = res.items;
      }
  });
  }

  
  load(dataModel) {
    this.formGroup.setValue({
      projectName: dataModel.projectName,
      userName: dataModel.userName,
      password: dataModel.password,
      projectAreaPath: dataModel.projectAreaPath,
      onlineTfsCollectionUri: dataModel.onlineTfsCollectionUri,
      collectionName: dataModel.collectionName,
      releaseName: dataModel.releaseName,
      isOnPremProject: dataModel.isOnPremProject,
      authType: dataModel.authType,
      workspaceName: dataModel.workspaceName,
      fromDate: dataModel.fromDate.toISOString(),
      toDate: dataModel.toDate.toISOString(),
    });
  }

  toggle(isOnPrem){
    this.removeValidators(this.formGroup);
    //this.isOnPremProjectFlag= this.formGroup.get('isOnPremProject').value ;
    this.isOnPremProjectFlag= isOnPrem.checked ;
    
    this.formGroup.reset();

    this.formGroup.patchValue({
      isOnPremProject: this.isOnPremProjectFlag,
    });

    if(this.isOnPremProjectFlag ==true)
    {
      this.projectType =  "On-Premises";
      this.formGroup = this.fb.group({
        projectName: ['', [Validators.required, Validators.maxLength(50)]],
        collectionName: ['', [Validators.required, Validators.maxLength(50)]],
        releaseName: ['', [Validators.required, Validators.maxLength(50)]],
        accountId: ['', [Validators.required]],
        workspaceName: ['', [Validators.required, Validators.maxLength(50)]],
      });
    }
    else
    {
      this.projectType =  "Online";
      this.formGroup = this.fb.group({
        projectName: ['', [Validators.required, Validators.maxLength(50)]],
        accountId: ['', [Validators.required]],
        workspaceName: ['', [Validators.required, Validators.maxLength(50)]],
        userName: ['', [Validators.required, Validators.maxLength(50)]],
        password: ['', [Validators.required, Validators.maxLength(50)]],
        onlineTfsCollectionUri: ['', [Validators.required,Validators.pattern(this.regForURL), Validators.maxLength(100)]],
        isOnPremProject: [true, [Validators.required]],
        authType: [1, [Validators.required]],
        fromDate:['', [Validators.required]],
        toDate: ['', [Validators.required]],
      });

    }
    this.formGroup.reset();
    this.formGroup.patchValue({
      isOnPremProject: this.isOnPremProjectFlag,
    });
  }
  
  gettingTheModelReady() {
    if(this.isOnPremProjectFlag == true)
    {
      return this.model = new ProjectDto({
        "id": this.isEditing ? this.model.id : undefined,
        "projectId": this.isEditing ? this.model.projectId : undefined,
        "accountId": this.formGroup.get('accountId').value,
        "isOnPremProject": true,
        "projectName": this.formGroup.get('projectName').value,
        "collectionName": this.formGroup.get('collectionName').value,
        "releaseName": this.formGroup.get('releaseName').value,
        "workspaceName": this.formGroup.get('workspaceName').value,

        "userName": '',
        "password": '',
        "onlineTfsCollectionUri": '',
        "authType": 0,
        "fromDate": moment(),
        "toDate": moment(),
      });
    }
    else
    {
      return this.model = new ProjectDto({
        "id": this.isEditing ? this.model.id : undefined,
        "isOnPremProject": false,
        "projectId": this.isEditing ? this.model.projectId : undefined,
        "accountId": this.formGroup.get('accountId').value,
        "projectName": this.formGroup.get('projectName').value,
        "userName": this.formGroup.get('userName').value,
        "password": this.formGroup.get('password').value,
        "onlineTfsCollectionUri": this.formGroup.get('onlineTfsCollectionUri').value,
        "authType": this.formGroup.get('authType').value,
        "fromDate": moment(this.formGroup.get('fromDate').value.toString()),
        "toDate": moment(this.formGroup.get('toDate').value.toString()),
        "workspaceName": this.formGroup.get('workspaceName').value,

        "collectionName": '',
        "releaseName": '',
      });
    }

  }


  save(){
    this.ProjectModel = this.gettingTheModelReady();
    console.log(this.formGroup);
    if(this.formGroup.valid && this.ProjectModel)
    {
       this.service.create(this.ProjectModel).subscribe(res => {
          console.log("res",res);
          abp.message.success("create Successfully");
       });
    }
    else{
      abp.message.error("Data not valid");
    }
  }

  public removeValidators(form: FormGroup) {
    for (const key in form.controls) {
         form.get(key).clearValidators();
         form.get(key).updateValueAndValidity();
    }
}


// public addValidators(form: FormGroup) {
//         for (const key in form.controls) {
//              form.get(key).setValidators(this.validationType[key]);
//              form.get(key).updateValueAndValidity();
//         }
// }


}
