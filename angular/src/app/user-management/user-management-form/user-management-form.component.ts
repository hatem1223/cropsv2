import { Component, OnInit, Injector } from '@angular/core';
import { UserRoleDTO, UsersServiceProxy } from '@shared/service-proxies/service-proxies';
import { FormComponentBase } from '@shared/form-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators, FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-user-management-form',
  templateUrl: './user-management-form.component.html',
  styleUrls: ['./user-management-form.component.scss']
})
export class UserManagementFormComponent extends FormComponentBase<UserRoleDTO> implements OnInit {

  userId: string;
  roles = [
    { id: -1, name: "Select" },
    { id: 0, name: "PM" },
    { id: 1, name: "Manager" },
    { id: 2, name: "Admin" },
    { id: 3, name: "Member" }
  ];
  selectedRole = null;

  constructor(
    route: ActivatedRoute,
    fb: FormBuilder,
    service: UsersServiceProxy,
    injector: Injector,
    router:Router) {

    super(route, fb, service, injector,router);
    this.model = new UserRoleDTO();
  }

  ngOnInit() {

    super.ngOnInit();

    this.formGroup = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(128)]],
      lastName: ['', [Validators.required, Validators.maxLength(128)]],
      roleName: [this.roles[0].name, [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.maxLength(50), Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required, Validators.maxLength(50), Validators.minLength(8)]],
    }, { validator: [this.passwordMatchValidator, this.roleValidator] });

    this.updateUIControlsVisibility()

  }

  passwordMatchValidator(group: FormGroup) {
    let pass = group.controls.password.value;
    let confirmPass = group.controls.confirmPassword.value;
    if (pass !== confirmPass) {
      return group.get('confirmPassword').setErrors({ ConfirmPassword: true });
    }
    return null
  }

  roleValidator(group: FormGroup) {

    if (!group.controls.roleName.value || group.controls.roleName.value == "Select") {
      return group.controls.roleName.setErrors({ EmptyRole: true })
    }
    return null;
  }

  load(dataModel) {
    this.formGroup.setValue({
      firstName: dataModel.firstName,
      lastName: dataModel.lastName,
      roleName: dataModel.roleName,
      email: dataModel.email,
      password: dataModel.userName,
      confirmPassword: dataModel.userName
    });
    this.userId = dataModel.id;
  }

  onSaving() {
    this.model.userName = this.formGroup.get('email').value;
  }

  onUpdating() {
    this.model.id = this.userId;
  }

  updateUIControlsVisibility() {
    if (this.isEditing) {
      // this.formGroup.get('password').setValidators(null);
      // this.formGroup.get('confirmPassword').setValidators(null);
      //this.formGroup.setValidators(null);

    }
    else {
      // this.formGroup.addControl('password', new FormControl('', [Validators.required, Validators.maxLength(50), Validators.minLength(9)]));
      // this.formGroup.addControl('confirmPassword', new FormControl('', [Validators.required, Validators.maxLength(50), Validators.minLength(9)]));
    }
  }

}
