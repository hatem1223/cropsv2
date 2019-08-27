import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import * as _ from 'lodash';

@Component({
    selector: 'app-lock-cell-renderer',
    template: `<a (click)="onClick($event)" class="btn btn-link" [attr.title]="params.data.isLocked?'Unlock':'Lock'"><i class="fa" [ngClass]="{'fa-lock':params.data.isLocked,'fa-unlock':!params.data.isLocked}" aria-hidden="true"></i></a>`
})
export class LockCellRendererComponent implements ICellRendererAngularComp {
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
