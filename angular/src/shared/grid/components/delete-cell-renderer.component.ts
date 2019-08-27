import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import * as _ from 'lodash';

@Component({
    selector: 'app-delete-cell-renderer',
    template: `<a (click)="onClick($event)" class="btn btn-link" title="Delete"><i class="fa fa-trash" aria-hidden="true"></i></a>`
})
export class DeleteCellRendererComponent implements ICellRendererAngularComp {

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
