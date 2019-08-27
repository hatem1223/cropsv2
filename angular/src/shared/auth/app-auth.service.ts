import { Injectable, EventEmitter } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { LoginService } from 'account/login/login.service';

@Injectable()
export class AppAuthService {

    constructor(private loginService: LoginService)
    {
    }

    logout(reload?: boolean): void {
        abp.auth.clearToken();
        abp.utils.setCookieValue(AppConsts.authorization.encrptedAuthTokenName, undefined, undefined, abp.appPath);
        this.loginService.startSignoutMainWindow();
        //if (reload !== false) {
        //    location.href = AppConsts.appBaseUrl;
        //}
    }
}
