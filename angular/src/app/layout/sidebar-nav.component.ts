import { Component, Injector, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { MenuItem } from '@shared/layout/menu-item';
import { WorkspaceServiceProxy, WorkspaceDto } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase } from '@shared/paged-listing-component-base';
import { Router } from '@angular/router';
import * as _ from 'lodash';
//import { setTimeout } from 'timers';

@Component({
    selector: 'sidebar-nav',
    templateUrl: './sidebar-nav.component.html',
    styleUrls: ['./sidebar-nav.component.scss'],
})
export class SideBarNavComponent extends PagedListingComponentBase<WorkspaceDto> {
    /********************************************************************************************** Fields */
    menuItems: MenuItem[] = [];
    configuration = {
        highlightOnSelect: true,
        interfaceWithRoute: true
    };

    /********************************************************************************************** Life-Cycle-Hooks */
    constructor(
        injector: Injector,
        private workSpaceService: WorkspaceServiceProxy,
        private _router: Router
    ) {
        super(injector, workSpaceService, _router);
        this.bindMenuItems();
    }

    /********************************************************************************************** Helpers */
    bindMenuItems() {
        // bind dynamic menu items.
        this.workSpaceService.getAll()
            .subscribe((items) => {
                var workspaceItems = this.adjustWorkspaceMenuItems(items);
                staticMenuItems[1].items = workspaceItems;
                this.menuItems = staticMenuItems;
            });

        // static menu items list.
        var staticMenuItems = [
            // Home: (Roles: User)
            new MenuItem('HomePage', '', 'home', '/app/home'),

            //Workspace: (Roles: User, Admin)
            new MenuItem('Workspace', '', 'menu', '', []),

            // Admin Panel: (Roles: Admin)
            new MenuItem('Admin Panel', '', 'menu', '', [
                new MenuItem('Create Account', '', '', '/app/account/create'),
                new MenuItem('Register Project', '', '', '/app/project/create'),
                new MenuItem('Managed Locked Users', '', '', '/app/home'),
            ]),

            // Project Configuration: (Roles: ProjectManager, Admin)
            new MenuItem('Project Configuration', '', 'menu', '', [
                new MenuItem('Project Details', '', '', '/app/configure-project'),
                new MenuItem('Iteration Data', '', '', '/app/home'),
                new MenuItem('User Stories Data', '', '', '/app/home'),
                new MenuItem('Role Activity', '', '', '/app/home'),
                new MenuItem('Sample CRUD', '', '', '/app/project-details'),
            ]),

            // User Management: (Roles: Admin)
            new MenuItem('User Management', '', 'chevron_right', '/app/users'),
        ];
    }

    adjustWorkspaceMenuItems(items: any): MenuItem[] {
        let childItems: MenuItem[] = [];
        _.forEach(items, (item) => {
            let reportItem = new MenuItem('Reports', '', 'menu', '');
            let dashboardItem = new MenuItem('Dashboards', '', 'menu', '');
            let workspaceItem = new MenuItem(item.name, '', '', '');

            console.log(item.reports);
            // reports
            if (item.reports) {
                for (let report of item.reports) {
                    if (report.isActive) {
                        reportItem.items.push(new MenuItem(report.name, '', '', '/app/Report/' + report.id));
                    }
                }
            }
            // dashboards
            if (item.dashboards) {
                for (let dashboard of item.dashboards) {
                    if (dashboard.isActive) {
                        dashboardItem.items.push(new MenuItem(dashboard.name, '', '', '/app/Dashboard/' + dashboard.id));
                    }
                }
            }

            // fill child-items list
            workspaceItem.items.push(reportItem);
            workspaceItem.items.push(dashboardItem);
            childItems.push(workspaceItem);
        });

        return childItems;
    }

    showMenuItem(menuItem): boolean {
        if (menuItem.permissionName) {
            return this.permission.isGranted(menuItem.permissionName);
        }
        return true;
    }
}
