import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JsonpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { ModalModule } from 'ngx-bootstrap';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgMaterialMultilevelMenuModule } from 'ng-material-multilevel-menu';
import { NgScrollbarModule } from 'ngx-scrollbar';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { AbpModule } from '@abp/abp.module';

import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';

import { HomeComponent } from '@app/home/home.component';
import { AboutComponent } from '@app/about/about.component';
import { TopBarComponent } from '@app/layout/topbar.component';
import { TopBarLanguageSwitchComponent } from '@app/layout/topbar-languageswitch.component';
import { SideBarUserAreaComponent } from '@app/layout/sidebar-user-area.component';
import { SideBarNavComponent } from '@app/layout/sidebar-nav.component';
import { SideBarFooterComponent } from '@app/layout/sidebar-footer.component';
import { RightSideBarComponent } from '@app/layout/right-sidebar.component';
import { WidgetComponent } from './widget/widget.component';
import { ProjectDetailsComponent } from './project-details/project-details.component';
import { ProjectDetailsServiceProxy, UsersServiceProxy } from '@shared/service-proxies/service-proxies';
import { AccountServiceProxy } from '@shared/service-proxies/service-proxies';

import { LoginCallbackComponent } from './login-callback/login-callback.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { AgGridModule } from 'ag-grid-angular';
import 'ag-grid-enterprise';
import { GridComponent } from '../shared/grid/grid.component';
import { ConfigProjectDataComponent } from './project-details/config-project-data/config-project-data.component';
import { ProjectDetailsFormComponent } from './project-details/project-details-form/project-details-form.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ReportComponent } from './report/report.component';
import { AccountFormComponent } from './account/account-create-form/account-create-form.component';
import { ProjectFormComponent } from './project/project-form/project-form.component';
import { UserManagementFormComponent } from './user-management/user-management-form/user-management-form.component';
import { UserManagementComponent } from './user-management/user-management.component';

@NgModule({
    declarations: [
        AboutComponent,
        AppComponent,
        HomeComponent,
        LoginCallbackComponent,
        ProjectDetailsComponent,
        RightSideBarComponent,
        SideBarFooterComponent,
        SideBarNavComponent,
        SideBarUserAreaComponent,
        TopBarComponent,
        TopBarLanguageSwitchComponent,
        UnauthorizedComponent,
        WidgetComponent,
        ConfigProjectDataComponent,
        ProjectDetailsFormComponent,
        DashboardComponent,
        ReportComponent,
        AccountFormComponent,
        ConfigProjectDataComponent,
        ProjectDetailsFormComponent,
        ProjectFormComponent ,
        ProjectFormComponent,
        
        UserManagementFormComponent,
        UserManagementComponent
    ],
    imports: [
        NgMaterialMultilevelMenuModule,
        NgScrollbarModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        JsonpModule,
        ModalModule.forRoot(),
        AbpModule,
        AppRoutingModule,
        ServiceProxyModule,
        SharedModule,
        NgxPaginationModule,
    ],
    providers: [AccountServiceProxy, UsersServiceProxy],
    entryComponents: [
    ]
})
export class AppModule { }
