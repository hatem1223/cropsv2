import { Component, OnInit } from '@angular/core';
import { LoginService } from 'account/login/login.service';

@Component({
  templateUrl: './unauthorized.component.html',
})
export class UnauthorizedComponent implements OnInit {

  constructor(private _loginService: LoginService) { }

  ngOnInit() {
  }

  login(){
    this._loginService.authenticate();
  }

}
