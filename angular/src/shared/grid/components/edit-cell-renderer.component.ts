import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import * as _ from 'lodash';

@Component({
    selector: 'app-edit-cell-renderer',
    template: `<a (click)="onClick($event)" class="btn btn-link" title="Edit"><i class="fa fa-pencil" aria-hidden="true"></i></a>`
})
export class EditCellRendererComponent implements ICellRendererAngularComp {
    public params: any;

    agInit(params: any): void {
        this.params = params;
    }

    refresh(): boolean {
        return true;
    }

    onClick($event) {
        if (_.isFunction(this.params.onClick)) {
            this.params.onClick(this.params.node.data);
        }
    }
}
