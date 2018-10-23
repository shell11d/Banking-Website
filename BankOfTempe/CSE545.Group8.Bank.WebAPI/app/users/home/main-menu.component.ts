import { Component, ViewChild, ViewEncapsulation, OnInit} from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { UserService } from './shared/user.service';
import { AccountService } from './shared/account.service'
import { User } from './model/user';
import { LoginService } from '../login/login.service';
import { Account } from './model/account';
import { UserType } from './model/user';
import { ModalComponent } from 'ng2-bs3-modal/ng2-bs3-modal';
import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Router } from '@angular/router';

@Component({
    selector: 'app-main-menu',
    templateUrl: 'app/users/home/main-menu.component.html',
    styleUrls: ['app/users/home/main-menu.component.css'],
    providers: [LoginService],
    encapsulation: ViewEncapsulation.None
})
    

export class MainMenuComponent {
    @ViewChild('otpModal')
    otpModal: ModalComponent;

  user : User;
  userName:string;
  userType1: string;
  openModal: boolean;

  oldPassword: string;
  newPassword: string;
  confirmPassword: string;

  updateSuccesMessage: string;
  updateError: string;


  accounts: Account[];


  UserType: typeof UserType = UserType;

  constructor(private userService: UserService,
              private loginService: LoginService,
              private accountService: AccountService,
              private route: ActivatedRoute,
              private http: Http) { }
  

  ngOnInit(): void {
      if (this.loginService.isLoggedInAny()) {
          console.log("Login");
          this.route.params.forEach((params: Params) => {             
              let id = params['userId'];
              this.userService.setUserId(id);
              this.userService.getUser().then(user => this.user = user);
       });
    } 
  }

  logout(): void{
  //shld be logout(userName)
      this.loginService.logoutAny();
  }

  changePassword(): void {

      this.otpModal.open();
  }

  updatePassword(): void{
      var _this = this;
      var headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(this.user.emailAddress) });
      this.http.post("api/login/ChangePassword", JSON.stringify({ oldPassword: this.oldPassword, newPassword: this.newPassword, confirmPassword: this.confirmPassword }), { headers: headers }).toPromise().then(function (response) {
          if (response.status == 200) {
              _this.updateSuccesMessage = "Updation of Password Successful.";
          }
          else {
              _this.updateError = "Updation of password failure. Tyr again";
          }
      }).catch(function (error) {
          this.updateError = "Updation Error";
          });
  }
  
}