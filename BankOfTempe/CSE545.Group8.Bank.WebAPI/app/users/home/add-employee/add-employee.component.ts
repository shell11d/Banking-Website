import { Component, OnInit } from '@angular/core';
import { CustomerProfile } from '../model/customer-profile';
import { UserType } from '../model/user';
import { Headers, Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Account } from '../model/account';
import { AccountType } from '../model/account';
import { Router } from '@angular/router';
import { User } from '../model/user';
import { UserService } from '../shared/user.service';





import 'rxjs/add/operator/toPromise';

export class createResponse {
    url: string;
    id: string;
    userName: string;
    fullName: string;
    email: string;
    emailConfirmed: boolean;
    preSharedKey: string;
    roles: string[];
    claims: string[];

}

export class SecurityQuestions {
    userId: string;
    securityQuestion1: string;
    securityAnswer1: string;
    securityQuestion2: string;
    securityAnswer2: string;
    securityQuestion3: string;
    securityAnswer3: string;

    constructor(userId: string, securityQuestion1: string, securityAnswer1: string, securityQuestion2: string, securityAnswer2: string, securityQuestion3: string, securityAnswer3: string) {
        this.securityQuestion1 = securityQuestion1;
        this.securityAnswer1 = securityAnswer1;
        this.securityQuestion2 = securityQuestion2;
        this.securityAnswer2 = securityAnswer2;
        this.securityQuestion3 = securityQuestion3;
        this.securityAnswer3 = securityAnswer3;
        this.userId = userId;
    }


}


@Component({
    selector: 'details-view',
    templateUrl: './app/users/home/create-user/create-user.component.html',
    styleUrls: ['./app/users/home/create-user/create-user.component.css']
})


@Injectable()
export class AddEmployeeComponent implements OnInit {

    UserType: typeof UserType = UserType;

    public userProfile = new CustomerProfile(null, null, null, 0, null, null, null, 0, null);
    public securityProfile = new SecurityQuestions(null, null, null, null, null, null, null);

    currentUser: User;
    noUserError: string;

    constructor(private http: Http, private router: Router, private userService: UserService) { }

    public genders = [
        { value: 'Female', display: 'Female' },
        { value: 'Male', display: 'Male' }
    ];

    public states = ["Arizona", "Utah", "California", "Florida", "NewYork"];
    public userRoles = ["Banker", "Manager"]


    validOnlineAccount: boolean;
    createError: string;
    createAccountError: string;
    createMessage: string;
    onlineResponse: createResponse;

    account: Account[];


    emailId: string;
    firstName: string;
    lastName: string;
    password: string;
    confirmPassword: string;
    accounttype: string;




    ngOnInit(): void {
        var _this = this;
        this.userService.getUser().then(function (user) {
            _this.currentUser = user;
        }).catch(function (error) {
            _this.noUserError = "No user exists, please login again.";
        });
    }

    createOnlineAccount(): void {
        var headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(this.currentUser.emailAddress) });
        var _this = this;
        this.createError = null;

        var onlineAccount = {
            email: this.emailId,
            firstName: this.firstName,
            userName: this.emailId,
            lastName: this.lastName,
            password: this.password,
            UserTypes: [this.accounttype],
            confirmPassword: this.confirmPassword

        }
        if (this.password === this.confirmPassword) {
            this.http.post("api/login/create", JSON.stringify(onlineAccount), { headers: headers }).toPromise().then(function (response) {
                _this.onlineResponse = response.json() as createResponse;
                var userId = _this.onlineResponse.id;
                if (userId != '') {
                    _this.userProfile.emailAddress = _this.emailId;
                    _this.userProfile.firstName = _this.firstName;
                    _this.userProfile.lastName = _this.lastName;
                    _this.userProfile.type = _this.accounttype;
                    _this.userProfile.UserId = userId;
                    _this.securityProfile.userId = userId;
                    _this.validOnlineAccount = true;
                }

            }).catch(this.handleOnlineError);
        }
        else {

            this.createError = "Passwords must match";
        }
    }

    createBankAccount(): void {
        var headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(this.currentUser.emailAddress) });
        this.createError = null;
        var _this = this;
        this.account = [new Account(null, 'Checking', null)];
        var accountString = JSON.stringify(this.account);

        this.userProfile.emailAddress = this.emailId;

        this.http.put("api/users", JSON.stringify(this.userProfile), { headers: headers }).toPromise().then(function (response) {
            if (response.ok) {
                if (_this.checkSecQ()) {
                    _this.http.put("api/userInfo", JSON.stringify(_this.securityProfile), { headers: headers }).toPromise().then(function (response) {
                        _this.http.put("api/users/accounts/checking", JSON.stringify({ customerId: _this.userProfile.UserId, type: "Checking" }), { headers: headers }).toPromise().then(function (response) {
                            _this.createMessage = "Checkings Account with account number: " + response.json() + "  and Online account has been successfully created.";
                        }).catch(_this.handleAccountError);
                    }).catch(_this.handleAccountError);
                }
                else {
                    _this.createAccountError = "Security Answers must not be same";
                }

            }

        }).catch(this.handleAccountError);


    }

    doneAccount(): void {
        this.router.navigate(["/mainmenu"]);
    }
    private checkSecQ(): boolean {
        if ((this.securityProfile.securityAnswer1 === this.securityProfile.securityAnswer2) || (this.securityProfile.securityAnswer3 == this.securityProfile.securityAnswer2) || (this.securityProfile.securityAnswer3 === this.securityProfile.securityAnswer1)) {
            return false;
        }
        return true;
    }
    private handleAccountError(error: any): void {
        console.error('An error occurred', error);
        var errorMesage = error._body.message;
        if (errorMesage) {
            this.createAccountError = errorMesage;
        } else {
            this.createAccountError = "Error creating Account";

        }
    }

    private handleOnlineError(error: any): void {
        var errorMesage = error._body.message;
        if (errorMesage) {
            this.createError = errorMesage;
        } else {
            this.createError = "Error creating Online Account";

        }
    }

}