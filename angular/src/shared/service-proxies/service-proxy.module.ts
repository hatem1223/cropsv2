import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from '@abp/abpHttpInterceptor';

import * as ApiServiceProxies from './service-proxies';

@NgModule({
    providers: [
        ApiServiceProxies.ProjectDetailsServiceProxy,
        ApiServiceProxies.ProjectServiceProxy,
        ApiServiceProxies.WorkspaceServiceProxy,
        ApiServiceProxies.DashboardServiceProxy,
        ApiServiceProxies.ReportServiceProxy,
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }
    ]
})
export class ServiceProxyModule { }
