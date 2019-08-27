import { Component, Injector } from '@angular/core';
import { PagedListingComponentBase } from 'shared/paged-listing-component-base';
import { ProjectDetailsServiceProxy, ProjectDetailDto } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Router } from '@angular/router';
import * as _ from 'lodash';
import { NumberOrString } from '../../shared/AppTypes';


@Component({
    selector: 'project-details',
    templateUrl: './project-details.component.html',
    styleUrls: ['./project-details.component.scss'],
    animations: [appModuleAnimation()]
})

export class ProjectDetailsComponent extends PagedListingComponentBase<ProjectDetailDto> {

    constructor(injector: Injector, service: ProjectDetailsServiceProxy, router: Router) {
        super(injector, service, router);
        this.initializeGrid();
    }

    onEditRow(row: any): void {
        if (row && row.id) {
            this.router.navigate(['/app/project-details/edit', row.id]);
        }
    }

    initializeGrid() {
        this.columnDefs = [
            { headerName: 'Description', field: 'descriptions' },
            { field: 'creationDate', type: '' },
            { field: 'optimisticIteration' },
            { field: 'pessimisticIteration' },
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
            },
        ];
    }
}
