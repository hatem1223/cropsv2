import { AppComponentBase } from 'shared/app-component-base';
import { Injector, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { Observable } from 'rxjs';
import * as _ from 'lodash';
import { Router } from '@angular/router';
import { setTime } from 'ngx-bootstrap/chronos/utils/date-setters';
import { IGridGetRowsParams } from './grid';
import { NumberOrString } from './AppTypes';

export class PagedResultDto {
    items: any[];
    totalCount: number;
}

export class EntityDto {
    id: number;
}

export class PagedRequestDto {
    skipCount: number;
    maxResultCount: number;
}

export abstract class PagedListingComponentBase<TEntityDto> extends AppComponentBase implements OnInit {

    public pageSize = 10;
    public pageNumber = 1;
    public totalPages = 1;
    public totalItems: number;
    public isTableLoading = false;
    public service: any;
    public router: Router;
    public items: TEntityDto[] = [];
    public isItemDeleted: boolean;
    public columnDefs: any[];

    constructor(injector: Injector, service: any, router: Router) {
        super(injector);
        this.service = service;
        this.router = router;
    }

    ngOnInit(): void {
        this.refresh();
    }

    refresh(): void {
        this.getDataPage(this.pageNumber);
    }

    public showPaging(result: PagedResultDto, pageNumber: number): void {
        this.totalPages = ((result.totalCount - (result.totalCount % this.pageSize)) / this.pageSize) + 1;

        this.totalItems = result.totalCount;
        this.pageNumber = pageNumber;
    }

    public getDataPage(page: number): void {
        const req = new PagedRequestDto();
        req.maxResultCount = this.pageSize;
        req.skipCount = (page - 1) * this.pageSize;

        this.isTableLoading = true;
        this.list(req, page, () => {
            this.isTableLoading = false;
        });
    }

    /*protected abstract list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void;
    protected abstract delete(entity: TEntityDto): void;*/
    public list(pagedRequest: any, pageNumber: number, finishedCallback: Function): Observable<any> {

        return this.service
            .getAll(pagedRequest.keyword, pagedRequest.isActive, pagedRequest.skipCount, pagedRequest.maxResultCount)
            .subscribe((result: any) => {
                this.items = result.items;
                this.showPaging(result, pageNumber);
            });
    }

    // Gets all data based on filter/sort/pagination parameters, then calls a success/fail callbacks.
    public getAll(params: IGridGetRowsParams): void {
        const { filter, sorting, skipCount, maxResultCount, onSuccess, onError } = params;
        this.service.getAll(filter, sorting, skipCount, maxResultCount)
            .subscribe(
                (result) => {
                    if (_.isFunction(onSuccess)) {
                        onSuccess(result.items, result.totalCount);
                    }
                },
                (error) => {
                    if (_.isFunction(onError)) {
                        onError();
                    }
                });
    }

    // Edit row handler.
    public onEditRow(row: any): void {
        //TODO: implement common edit action across all views.
    }

    // Delete row handler.
    public onDeleteRow(row: any): void {
        if (!row || !row.id) return;
        let id = row.id;
        abp.message.confirm(
            this.l('UserDeleteWarningMessage'),
            (result: boolean) => {
                if (result) {
                    this.service.delete(id)
                        .subscribe(() => {
                            abp.notify.success(this.l('SuccessfullyDeleted'));
                            this.isItemDeleted = true;
                        });
                }
            }
        );
    }
}
