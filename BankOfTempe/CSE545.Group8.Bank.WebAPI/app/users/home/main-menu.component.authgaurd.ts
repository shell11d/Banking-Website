import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CanActivate } from '@angular/router';
// Import our authentication service
import { LoginService } from '../login/login.service';

@Injectable()
export class MainMenuAuthgaurd implements CanActivate {

  constructor(private loginService: LoginService, private router: Router) {}

  canActivate() {
      if (!this.loginService.isLoggedInAny()) {
      this.router.navigate(['/login']);
      return false;
    }
    return true;
  }

}