import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AgGridModule } from 'ag-grid-angular';
import 'ag-grid-enterprise';
import {
    CurrencyCellRendererComponent,
    DeleteCellRendererComponent,
    EditCellRendererComponent,
    EditDeleteCellRendererComponent,
    GridComponent
} from './';
import { LockCellRendererComponent } from './components/lock-cell-renderer.component';
@NgModule({
    imports: [
        CommonModule,
        AgGridModule.withComponents([
            CurrencyCellRendererComponent,
            DeleteCellRendererComponent,
            EditCellRendererComponent,
            EditDeleteCellRendererComponent,
            LockCellRendererComponent,
        ])
    ],
    declarations: [
        CurrencyCellRendererComponent,
        DeleteCellRendererComponent,
        EditCellRendererComponent,
        EditDeleteCellRendererComponent,
        GridComponent,
        LockCellRendererComponent,
    ],
    exports: [
        CurrencyCellRendererComponent,
        DeleteCellRendererComponent,
        EditCellRendererComponent,
        EditDeleteCellRendererComponent,
        LockCellRendererComponent,
        GridComponent,
    ]
})
export class GridModule { }
