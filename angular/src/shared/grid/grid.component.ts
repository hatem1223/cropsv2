import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { ColDef, ColGroupDef, IServerSideDatasource } from 'ag-grid-community';
import { IGridGetRowsParams, IGridGetRowsRequest } from './models';
import * as _ from 'lodash';
import { CurrencyCellRendererComponent, DeleteCellRendererComponent, EditCellRendererComponent, EditDeleteCellRendererComponent } from './components';
import { NumberOrString } from '../AppTypes';
import { LockCellRendererComponent } from './components/lock-cell-renderer.component';

@Component({
    selector: 'app-grid',
    templateUrl: './grid.component.html',
    styleUrls: ['./grid.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class GridComponent {

    /********************************************************************************************** Configuration */
    context;
    defaultColDef = { sortable: false, filter: false };
    frameworkComponents;
    gridApi: any;
    columnApi: any;
    height = 500;
    paginationPageSize = 10;
    rowModelType = "serverSide";
    width = 100;

    /********************************************************************************************** Inputs-Params */
    @Input() columnDefs: ColDef[] | ColGroupDef[];
    @Input() isItemDeleted: boolean;

    /********************************************************************************************** Output-Params */
    @Output() deleteRow = new EventEmitter<NumberOrString>();
    @Output() editRow = new EventEmitter<NumberOrString>();
    @Output() getRows = new EventEmitter<IGridGetRowsParams>();
    @Output() lockRow = new EventEmitter<any>();

    /********************************************************************************************** Life-Cycle-Hooks */
    constructor() {
        this.context = { componentParent: this };
        this.frameworkComponents = {
            currencyRenderer: CurrencyCellRendererComponent,
            deleteRenderer: DeleteCellRendererComponent,
            editDeleteRenderer: EditDeleteCellRendererComponent,
            editRenderer: EditCellRendererComponent,
            lockRenderer: LockCellRendererComponent,
        };
    }

    ngOnChanges() {
        if (this.isItemDeleted) {
            this.gridApi.purgeServerSideCache();
        }
    }

    /********************************************************************************************** Handlers */
    onGridReady(params) {
        this.gridApi = params.api;
        this.columnApi = params.columnApi;
        this.gridApi.sizeColumnsToFit();
        this.gridApi.setServerSideDatasource(this.getDatasource());
    }

    onResetFilters() {
        this.gridApi.setFilterModel(null);
        this.gridApi.onFilterChanged();
    }

    onDeleteRow(id: NumberOrString) {
        if (_.isEmpty(_.toString(id)) || _.isNull(id))
            return;
        this.deleteRow.emit(id);
    }

    onEditRow(id: NumberOrString) {
        if (_.isEmpty(_.toString(id)) || _.isNull(id))
            return;
        this.editRow.emit(id);
    }

    /********************************************************************************************** Helpers */
    getDatasource(): IServerSideDatasource {
        return {
            getRows: (params) => {
                this.getRows.emit({
                    filter: this.adjustFilterModel(params.request.filterModel),
                    sorting: this.adjustSortModel(params.request.sortModel),
                    skipCount: params.request.startRow,
                    maxResultCount: this.paginationPageSize,
                    onSuccess: params.successCallback,
                    onError: params.failCallback,
                });
            }
        };
    }

    // adjust sort model to a string before submitting datasource.
    adjustSortModel(sortModel: any[]): string {
        var sort = "";
        if (sortModel && sortModel.length) {
            sort = _.map(sortModel, (item) => item.colId + " " + item.sort).join(",");
        }
        return sort;
    }

    // adjust filter model to a string before submitting datasource.
    adjustFilterModel(filterModel: any[]): string {
        //TODO: implement the OData filter template.

        var filter = "";
        return filter;
    }

    getRowNodeId(data) {
        return data.id;
    }
}
