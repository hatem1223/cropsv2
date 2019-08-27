import { AppComponentBase } from 'shared/app-component-base';
import { finalize } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ProjectDetailsServiceProxy, ProjectDetailDto } from '@shared/service-proxies/service-proxies';
import { Component, OnInit, Injector } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';

export abstract class FormComponentBase<TEntityDto> extends AppComponentBase implements OnInit {
    public formGroup: FormGroup;
    public saving: boolean;
    public model: TEntityDto | any;
    public id: any;
    public isEditing = false;
    constructor(
        protected route: ActivatedRoute,
        protected fb: FormBuilder,
        protected service: any,
        protected injector: Injector,
        protected navigationRoute: Router) {
        super(injector);
    }

    ngOnInit() {
        this.route.params.subscribe((params: any) => {
            if (params.id) {
                this.id = params.id;
                this.service.get(this.id).subscribe(res => {
                    this.model = res;
                    this.isEditing = true;
                    this.load(this.model);
                });
            }
        });
    }

    isValid() { return true }
    onSaving() { }
    onSaved() { }
    onCreating() { }
    onCreated() {
        this.notify.info(this.l('Created Successfully'))
    }
    onUpdating() { }
    onUpdated() {
        this.notify.info(this.l('Saved Successfully'))
    }

    hasError = (controlName: string, errorName: string) => {
        return this.formGroup.controls[controlName].hasError(errorName);
    }

    load(dataModel: TEntityDto) { }

    save() {
        this.model = this.formGroup.value;
        if (this.formGroup.valid && this.isValid()) {
            this.saving = true;
            this.onSaving();
            this.route.params.subscribe((params: any) => {
                if (params.id) {
                    this.id = params.id;
                    this.onUpdating();
                    this.service.update(this.model)
                        .pipe(
                            finalize(() => {
                                this.saving = false;
                            })
                        )
                        .subscribe(
                            result => { this.onUpdated(); this.onSaved() },
                            error => { this.notify.error(this.l(`Saving Failed [${error}]`)) }
                        );
                } else {
                    delete this.model.id;
                    this.onCreating();
                    this.service
                        .create(this.model)
                        .pipe(
                            finalize(() => {
                                this.saving = false;
                            })
                        )
                        .subscribe(
                            result => { this.onCreated(); this.onSaved() },
                            error => { this.notify.error(this.l(`Saving Failed [${error}]`)) }
                        );
                }
            });
        }
    }

    cancel(navigationUrl: string) {
        this.navigationRoute.navigate([navigationUrl]);
    }
}
