export class MenuItem {
    label?= '';
    permissionName = '';
    icon = '';
    link = '';
    items: MenuItem[];

    constructor(name: string, permissionName: string, icon: string, link: string, items: MenuItem[] = null) {
        this.label = name;
        this.permissionName = permissionName;
        this.icon = icon;
        this.link = link;

        if (items) {
            this.items = items;
        } else {
            this.items = [];
        }
    }
}
