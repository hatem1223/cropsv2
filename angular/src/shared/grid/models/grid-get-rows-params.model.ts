import { IGridGetRowsRequest } from "./";

export interface IGridGetRowsParams extends IGridGetRowsRequest {
    onSuccess(items: any[], totalCount: number): void;
    onError(): void;
}
