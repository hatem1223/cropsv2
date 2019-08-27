import { UserManagementComponent } from './user-management/user-management.component';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { LoginCallbackComponent } from './login-callback/login-callback.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { ProjectDetailsComponent } from './project-details/project-details.component';
import { ProjectDetailsFormComponent } from './project-details/project-details-form/project-details-form.component';
import {ReportComponent} from './report/report.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ConfigProjectDataComponent } from './project-details/config-project-data/config-project-data.component';
import { AccountFormComponent } from './account/account-create-form/account-create-form.component';
import { ProjectFormComponent } from './project/project-form/project-form.component';
import { UserManagementFormComponent } from './user-management/user-management-form/user-management-form.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    { path: 'home', component: HomeComponent, canActivate: [AppRouteGuard] },
                    { path: 'loginCallback', component: LoginCallbackComponent },
                    { path: 'unauthorized', component: UnauthorizedComponent },
                    { path: 'about', component: AboutComponent },
                    { path: 'project-details', component: ProjectDetailsComponent, canActivate: [AppRouteGuard] },
                    { path: 'project-details/create', component: ProjectDetailsFormComponent, canActivate: [AppRouteGuard] },
                    { path: 'project-details/edit/:id', component: ProjectDetailsFormComponent, canActivate: [AppRouteGuard] },
                    { path: 'Dashboard/:id', component: DashboardComponent, canActivate: [AppRouteGuard]},
                    { path: 'Report/:id', component: ReportComponent, canActivate: [AppRouteGuard] },
                    { path: 'configure-project', component: ConfigProjectDataComponent, canActivate: [AppRouteGuard] },
                    { path: 'account/create', component: AccountFormComponent, canActivate: [AppRouteGuard] },
                    { path: 'project/create', component: ProjectFormComponent, canActivate: [AppRouteGuard] },
                    { path: 'configure-project', component: ConfigProjectDataComponent, canActivate: [AppRouteGuard] },
                    { path: 'users/create', component: UserManagementFormComponent, canActivate: [AppRouteGuard] },
                    { path: 'users/edit/:id', component: UserManagementFormComponent, canActivate: [AppRouteGuard] },
                    { path: 'users', component: UserManagementComponent, canActivate: [AppRouteGuard] }
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
