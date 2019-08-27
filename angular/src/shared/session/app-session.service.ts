import { AbpMultiTenancyService } from '@abp/multi-tenancy/abp-multi-tenancy.service';
import { Injectable } from '@angular/core';
import { TokenService } from '@abp/auth/token.service';
import {
    ApplicationInfoDto,
    TenantLoginInfoDto,
    UserLoginInfoDto
} from '@shared/service-proxies/old-classes';
import * as moment from 'moment';

@Injectable()
export class AppSessionService {

    private _user: UserLoginInfoDto;
    private _tenant: TenantLoginInfoDto;
    private _application: ApplicationInfoDto;

    constructor(
        private _tokenService: TokenService,
        private _abpMultiTenancyService: AbpMultiTenancyService
    ) {
    }

    get application(): ApplicationInfoDto {
        return this._application;
    }

    get user(): UserLoginInfoDto {
        return this._user;
    }

    get userId(): number {
        return this.user ? this.user.id : null;
    }

    get tenant(): TenantLoginInfoDto {
        return this._tenant;
    }

    get tenantId(): number {
        return this.tenant ? this.tenant.id : null;
    }

    getShownLoginName(): string {
        const userName = this._user.userName;
        if (!this._abpMultiTenancyService.isEnabled) {
            return userName;
        }

        return (this._tenant ? this._tenant.tenancyName : '.') + '\\' + userName;
    }

    init(data?: any): Promise<boolean> {
        this._application = new ApplicationInfoDto();
        this._tenant = new TenantLoginInfoDto();

        this._application.releaseDate = moment();
        this._application.version = '1.0';

        if(this._user == undefined){this._user = new UserLoginInfoDto();}
        return new Promise<boolean>((resolve, reject) => { resolve(true) });
    }

    public setUser(user: any) {
        if (user) {
            this._user.id = user.profile.sub;
            this._user.name = user.profile.name;
            this._user.emailAddress = user.profile.name;
            this._user.userName = user.profile.name;
        }
    }

    changeTenantIfNeeded(tenantId?: number): boolean {
        if (this.isCurrentTenant(tenantId)) {
            return false;
        }

        abp.multiTenancy.setTenantIdCookie(tenantId);
        location.reload();
        return true;
    }

    private isCurrentTenant(tenantId?: number) {
        if (!tenantId && this.tenant) {
            return false;
        } else if (tenantId && (!this.tenant || this.tenant.id !== tenantId)) {
            return false;
        }

        return true;
    }
}
