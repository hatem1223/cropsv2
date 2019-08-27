import { Component, OnInit, Injector } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProjectDetailsServiceProxy, ProjectDetailDto, AccountCreateDto, AccountServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponent } from '@app/app.component';
import { finalize } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { load } from '@angular/core/src/render3';
import { FormComponentBase } from '@shared/form-component-base';
import * as moment from 'moment';

@Component({
    selector: 'account-create-form',
    templateUrl: './account-create-form.component.html',
    styleUrls: ['./account-create-form.component.scss']
})


export class AccountFormComponent extends FormComponentBase<AccountCreateDto> implements OnInit {

    constructor(
        route: ActivatedRoute,
        fb: FormBuilder,
        service: AccountServiceProxy,
        injector: Injector,
        router:Router) {

        super(route, fb, service, injector,router);
        this.model = new AccountCreateDto();
    }

    ngOnInit() {

        super.ngOnInit();

        this.formGroup = this.fb.group({
            accountName: ['', [Validators.required, Validators.maxLength(50)]],
            descriptions: ['', [Validators.required, Validators.maxLength(50)]]
        });
    }

    load(dataModel) {
        this.formGroup.setValue({
            accountName: dataModel.accountName,
            descriptions: dataModel.descriptions
        });
    }
}
