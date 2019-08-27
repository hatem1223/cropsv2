import { UsersServiceProxy } from './../../shared/service-proxies/service-proxies';
import { UserRoleDTO } from '@shared/service-proxies/service-proxies';
import { Component, OnInit, Injector } from '@angular/core';
import { PagedListingComponentBase } from '@shared/paged-listing-component-base';
import { Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import * as _ from 'lodash';
import { NumberOrString } from '@shared/AppTypes';

@Component({
    selector: 'user-management',
    templateUrl: './user-management.component.html',
    styleUrls: ['./user-management.component.scss'],
    animations: [appModuleAnimation()]
})
export class UserManagementComponent extends PagedListingComponentBase<UserRoleDTO>  {

    constructor(injector: Injector, service: UsersServiceProxy, router: Router) {
        super(injector, service, router);
        this.initializeGrid();
    }

    ngOnInit() {
    }

    onEditRow(row: any): void {
        if (row && row.id) {
            this.router.navigate(['/app/users/edit', row.id]);
        }
    }
    onLockRow(row: any): void {
        //TODO: set the current state of the row as row.isLocked.

        let msg = `Do you want to ${row.isLocked ? "unlock" : "lock"} this user?`;
        abp.message.confirm(msg, (res) => { if (res) row.isLocked = !row.isLocked; });
    }

    initializeGrid() {
        this.columnDefs = [
            { headerName: 'Name', field: 'userName' },
            { headerName: 'Email', field: 'email' },
            { headerName: 'Role', field: 'roleName' },
            {
                // delete button
                cellClass: ['action-cell'],
                cellRenderer: 'deleteRenderer',
                cellRendererParams: { onClick: this.onDeleteRow.bind(this), },
                minWidth: 50,
                maxWidth: 50,
            },
            {
                // edit button
                cellClass: ['action-cell'],
                cellRenderer: 'editRenderer',
                cellRendererParams: { onClick: this.onEditRow.bind(this), },
                minWidth: 50,
                maxWidth: 50,
            }
            // {
            //     // lock button
            //     cellClass: ['action-cell'],
            //     cellRenderer: 'lockRenderer',
            //     cellRendererParams: { onClick: this.onLockRow.bind(this), },
            //     minWidth: 50,
            //     maxWidth: 50,
            // }
        ];
    }
}
