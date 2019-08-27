import { Component, OnInit } from '@angular/core';
import { LoginService } from 'account/login/login.service';
import { AppSessionService } from '@shared/session/app-session.service';

@Component({
  templateUrl: './login-callback.component.html'
})
export class LoginCallbackComponent implements OnInit {

  constructor(private _loginService: LoginService,
    private _sessionService: AppSessionService) {
    this._loginService.processAuthenticateResult();
  }

  ngOnInit() {
  }

}
