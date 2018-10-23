import { Component, ViewChild, ViewEncapsulation, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from './login.service';
import { LoggingUser } from '../login/logginguser.model';
import { LoggingResponse } from './loginResponse.model';
import { ModalComponent } from 'ng2-bs3-modal/ng2-bs3-modal';
import { SecurityQuestions } from '../home/model/security-questions';
import { Headers, Http } from '@angular/http';
import { Injectable } from '@angular/core';

@Component ({
    selector: 'app-login',
    templateUrl:`app/users/login/login.component.html`,
    styleUrls:['app/users/login/login.component.css'],
    providers: [LoginService],
    encapsulation: ViewEncapsulation.None
})
    @Injectable()
export class LoginComponent implements OnInit{
    @ViewChild('modal')
    modal: ModalComponent;

    @ViewChild('otpModal')
    otpModal: ModalComponent;

    virtualkey: string;
    public loggingUser = new LoggingUser('', '', '');
    public securityQuestions = new SecurityQuestions(null, null, null, null, null, null, null);
    firstAnswer: string;
    secondAnswer: string;
    thirdAnswer: string;


    loggingResponse: LoggingResponse;
    loggingError: string;


    selectedQuestion1: number;
    selectedQuestion2: string;
    forgotEmailId:string;
    validated: boolean;


    secQuestionsValError:string;
    otpValue:number;
    otpValid = false;

    newPassword:string;
    confirmPassword:string;
    updateError:string;
    updateValid=false;
    updateSuccesMessage: string;
    forgotPassError: string;
    passwordSent: string;

    validLoggingUser: string;

    private securityQuestionsUrl = "api/userInfo/userEmail?userEmail=";
    private headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem("currentUser") });






    constructor(
        private router : Router,
        private loginService: LoginService,
        private http: Http
    ) {

        }

    ngOnInit(): void{
        if (this.loginService.isLoggedInAny()) {
             let email = localStorage.getItem("loggedInUser");
             console.log(email);
             let link = ['/mainmenu', email];
             this.router.navigate(link);
         }
     }

    open() {
        var _this = this;
        this.forgotPassError = '';
        if (this.loggingUser.userName != null && this.loggingUser.userName!='') {
            this.http.get(this.securityQuestionsUrl + this.loggingUser.userName, { headers: this.headers }).toPromise().then(function (response) {

                _this.securityQuestions = response.json() as SecurityQuestions;
                if (_this.securityQuestions != null && _this.securityQuestions.userId != '' && _this.securityQuestions.userId != null) {
                    _this.modal.open();
                }
                else {
                    _this.forgotPassError = "Enter a valid user name to reset password";
                }

            }).catch(function (error) {
                _this.forgotPassError = "Enter a valid user name to reset password";
            });
        } else {
            _this.forgotPassError = "Enter a User Name to proceed";
        }
    }


    login(): void {
        this.loggingUser.grant_type = "password";
        var _this = this;
        let link = ['/mainmenu', _this.loggingUser.userName];
       this.loginService.login(this.loggingUser).then(function (loginResponse) {
           if (typeof (loginResponse) != 'undefined' && loginResponse != null && loginResponse.access_token != '' && loginResponse.access_token != null) {
               localStorage.setItem("loggedInUser", _this.loggingUser.userName);
                localStorage.setItem(_this.loggingUser.userName, loginResponse.access_token);
                let link = ['/mainmenu', _this.loggingUser.userName];
                _this.router.navigate(link);
            } else {
                _this.loggingError = "Invalid User name or Password";
            }
       }).catch(function (error) {
           _this.loggingError = "Invalid User Name or Password combination";
           });

    }

    validateForgotReq(): void{
        this.validated = false;
        var _this = this;
        this.secQuestionsValError = "";
        var securityAnswers = {
            email: _this.loggingUser.userName,
            answer1: this.firstAnswer,
            answer2: this.secondAnswer,
            answer3: this.thirdAnswer,

        }
        this.http.post("api/userInfo/verify", JSON.stringify(securityAnswers), { headers: this.headers }).toPromise().then(function (response) {
            var validAnswers = response.json() as boolean;
            if (validAnswers == true) {
                _this.validated = true;
                _this.http.post("api/login/ForgotPassword", JSON.stringify({ email: _this.loggingUser.userName }), { headers: _this.headers }).toPromise().then(function (response) {
                    if (response.status == 200) {
                        _this.passwordSent = "A Password has been sent to your registered mailId. Use it to login";
                    }
                }).catch(function (error) {
                    _this.secQuestionsValError = "Error sending password.";
                    });
            } else {
                _this.secQuestionsValError = "Invalid Answers. Try again";
            }
            
        }).catch(function (error) {
            _this.secQuestionsValError = "Oops..Error in validating. Please try reloading.. ";
        });

    }
    selectedQuestion(questionId: string):void{
        //remove the selected and add deselected
    }

    validateOtp():void{
        
    }

    modalCancel():void{
        this.modal.dismiss();
       this. cleanModal();

    }

    updatePassword():void{
        //var _this = this;
        this.updateError=null;
        if(this.newPassword===this.confirmPassword){
           this.updateValid = true;

            if(this.updateValid){
               this.updateSuccesMessage = "Password is updated succesfully. Please ";


            }

        }
        else{

            this.updateError = "Passwords must match";
        }

    }

    cleanModal():void{
        this.selectedQuestion1 = null;
        this.firstAnswer = null;
        this.selectedQuestion2=null;
        this.secondAnswer=null;
        this.forgotEmailId=null;
    }

    loginBack():void{
        window.location.reload();
    }

}
