import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { NumberOrString } from '../../AppTypes';

@Component({
    selector: 'app-edit-delete-cell-renderer',
    template: `
                <a (click)="editRow(params.value)" class="btn btn-link" title="Edit"><i class="fa fa-pencil" aria-hidden="true"></i></a>
                <a (click)="deleteRow(params.value)" class="btn btn-link" title="Delete"><i class="fa fa-trash" aria-hidden="true"></i></a>
              `
})
export class EditDeleteCellRendererComponent implements ICellRendererAngularComp {
    public params: any;

    agInit(params: any): void {
        this.params = params;
    }

    refresh(): boolean {
        return false;
    }

    editRow(id: NumberOrString) {
        this.params.context.componentParent.onEditRow(id);
    }

    deleteRow(id: NumberOrString) {
        this.params.context.componentParent.onDeleteRow(id);
    }
}
