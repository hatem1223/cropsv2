import { Component, Injector, ViewEncapsulation, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

class UiThemeInfo {
    constructor(
        public name: string,
        public cssClass: string
    ) { }
}

@Component({
    templateUrl: './right-sidebar.component.html',
    selector: 'right-sidebar',
    encapsulation: ViewEncapsulation.None
})
export class RightSideBarComponent extends AppComponentBase implements OnInit {

    themes: UiThemeInfo[] = [
        new UiThemeInfo('Red', 'red'),
        new UiThemeInfo('Pink', 'pink'),
        new UiThemeInfo('Purple', 'purple'),
        new UiThemeInfo('Deep Purple', 'deep-purple'),
        new UiThemeInfo('Indigo', 'indigo'),
        new UiThemeInfo('Blue', 'blue'),
        new UiThemeInfo('Light Blue', 'light-blue'),
        new UiThemeInfo('Cyan', 'cyan'),
        new UiThemeInfo('Teal', 'teal'),
        new UiThemeInfo('Green', 'green'),
        new UiThemeInfo('Light Green', 'light-green'),
        new UiThemeInfo('Lime', 'lime'),
        new UiThemeInfo('Yellow', 'yellow'),
        new UiThemeInfo('Amber', 'amber'),
        new UiThemeInfo('Orange', 'orange'),
        new UiThemeInfo('Deep Orange', 'deep-orange'),
        new UiThemeInfo('Brown', 'brown'),
        new UiThemeInfo('Grey', 'grey'),
        new UiThemeInfo('Blue Grey', 'blue-grey'),
        new UiThemeInfo('Black', 'black')
    ];

    selectedThemeCssClass = 'red';

    constructor(
        injector: Injector
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.selectedThemeCssClass = this.setting.get('App.UiTheme');
        $('body').addClass('theme-' + this.selectedThemeCssClass);
    }

}

