import { TokenService } from '@abp/auth/token.service';
import { LogService } from '@abp/log/log.service';
import { MessageService } from '@abp/message/message.service';
import { UtilsService } from '@abp/utils/utils.service';
import { Injectable, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { UserManager, User } from 'oidc-client';
import { environment } from 'environments/environment';
import { AppSessionService } from '@shared/session/app-session.service';


@Injectable()
export class LoginService {

    static readonly twoFactorRememberClientTokenName = 'TwoFactorRememberClientToken';

    //authenticateModel: AuthenticateModel;
    //authenticateResult: AuthenticateResultModel;

    settings: any = {
        authority: AppConsts.identityUrl,
        client_id: AppConsts.client_id,
        redirect_uri: AppConsts.appBaseUrl+ '/app/loginCallback',
        post_logout_redirect_uri: AppConsts.appBaseUrl + '/app/home',
        response_type: "code",
        scope: "openid profile api1",
        loadUserInfo: true
    };


    rememberMe: boolean;
    private mgr: UserManager = new UserManager(this.settings);
    currentUser: User;
    loggedIn = false;
    private userLoadededEvent: EventEmitter<User> = new EventEmitter<User>();

    constructor(
        private _router: Router,
        private _utilsService: UtilsService,
        private _tokenService: TokenService,
        private _sessionService: AppSessionService
    ) {
        this.clear();
        this.mgr.events.addUserSignedOut(() => {
            window.alert("signed out!");
        });

        this.mgr.events.addUserLoaded((user) => {
            this.currentUser = user;
            this.loggedIn = (user !== undefined);
            if (!environment.production) {
                console.log('authService addUserLoaded', user);
            }
        });

        this.mgr.events.addUserUnloaded((e) => {
            if (!environment.production) {
                console.log('user unloaded');
            }
            this.loggedIn = false;
        });

        
    }

    authenticate(): Promise<boolean> {
        return this.getUser().then((user) => {
            if (!user) {
                this.mgr.signinRedirect({ data: 'some data' }).then(function () {
                    console.log('signinRedirect done');

                }).catch(function (err) {
                    console.log(err);
                });
                return false;
            }
            this._sessionService.setUser(user);
            return true;
        })
    }

    processAuthenticateResult() {
        this.mgr.signinRedirectCallback().then((user) => {
            console.log('signed in', user);
            if (user) {
                this.login(user.access_token, user.access_token, user.expires_in, false);
                this._sessionService.setUser(user);
            }
            this._router.navigate(["/app/home"], { replaceUrl: true });
        }).catch((err) => {
            console.log(err);
            this._router.navigate(["/app/unauthorized"], { replaceUrl: true });
        });
    }

    getUser(): Promise<User> {
        return this.mgr.getUser();
    }

    removeUser() {
        this.mgr.removeUser().then(() => {
            this.userLoadededEvent.emit(null);
            console.log('user removed');
        }).catch(function (err) {
            console.log(err);
        });
    }

    startSignoutMainWindow() {
        this.mgr.getUser().then(user => {
            return this.mgr.signoutRedirect({ id_token_hint: user.id_token }).then(resp => {
                console.log('signed out', resp);
            }).catch(function (err) {
                console.log(err);
            });
        });
    };

    endSignoutMainWindow() {
        this.mgr.signoutRedirectCallback().then(function (resp) {
            console.log('signed out', resp);
        }).catch(function (err) {
            console.log(err);
        });
    };

    private login(accessToken: string, encryptedAccessToken: string, expireInSeconds: number, rememberMe?: boolean): void {

        const tokenExpireDate = rememberMe ? (new Date(new Date().getTime() + 1000 * expireInSeconds)) : undefined;

        this._tokenService.setToken(
            accessToken,
            tokenExpireDate
        );

        this._utilsService.setCookieValue(
            AppConsts.authorization.encrptedAuthTokenName,
            encryptedAccessToken,
            tokenExpireDate,
            abp.appPath
        );

        // let initialUrl = UrlHelper.initialUrl;
        // if (initialUrl.indexOf('/login') > 0) {
        //     initialUrl = AppConsts.appBaseUrl;
        // }

        // location.href = initialUrl;
    }

    private clear(): void {
        /*this.authenticateModel = new AuthenticateModel();
        this.authenticateModel.rememberClient = false;
        this.authenticateResult = null;*/
        this.rememberMe = false;
    }
}
