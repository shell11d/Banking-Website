import { Injectable } from '@angular/core';
import { Account, AccountType } from '../model/account';
import { Headers, Http } from '@angular/http';

import { UserService } from './user.service';
import { User } from '../model/user';

import 'rxjs/add/operator/toPromise';

class CreateService {
    Type: string;
    CustomerId: string;
    InitialBalance: number;

    constructor(Type: string, CustomerId: string, InitialBalance: number) {
        this.Type = Type;
        this.CustomerId = CustomerId;
        this.InitialBalance = InitialBalance;
    }
}

@Injectable()
export class AccountService {
    private urlChecking = "api/users/accounts/checking";
    private urlSavings = "api/users/accounts/savings";
    private urlCreadit = "api/users/accounts/creditcard";
    private urlGetAccounts = "api/users/{userid}/accounts";
    private urlGetAccountByMail = "api/accounts/checking/userEmail?userEmail=";
    private urlGetAccountByPhoneNumber = "api/accounts/savings/phoneNumber?phone=";

    constructor(private http: Http,
        private userService: UserService) { }

    createAccount(userId: string, accountType: string, initialBalance: number): Promise<any> {
        var _this = this;
        this.userService.getUser().then(function (user: User) {
            let headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(user.emailAddress) });
            if (accountType == 'Checking') {
                return _this.CreateCheckingsAccount(user.userId, initialBalance, headers);
            }
            if (accountType == 'Savings') {
                return _this.CreateSavingsAccount(user.userId, initialBalance, headers);
            }
            if (accountType == 'CreditCard') {
                return _this.CreateCreditAccount(user.userId, initialBalance, headers);
            }
        }).catch(error => { return Promise.reject(error) });
        return Promise.reject("failure");
    }

    getAccounts(): Promise<Account[]> {
        return this.userService.getUser().then(user => this.onGetUser(user));
    }

    getAccountByMail(emailAddress: string): Promise<Account> {
        var _this = this;
        return this.userService.getUser().then(function (user: User) {
            let headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(user.emailAddress) });
            return _this.http.get(this.urlGetAccountByMail + emailAddress, { headers: headers }).toPromise().then(response => response.json() as Account).catch(this.handleError);
        });
    }

    getAccountByPhoneNumber(phoneNumber: string): Promise<Account> {
        var _this = this;
        return this.userService.getUser().then(function (user: User) {
            let headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(user.emailAddress) });
            return _this.http.get(_this.urlGetAccountByPhoneNumber + phoneNumber, { headers: headers }).toPromise().then(response => response.json() as Account).catch(this.handleError);
        });
    }

    private onGetUser(user: User): Promise<Account[]> {
        let headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(user.emailAddress) });
        return this.http.get('api/users/' + user.userId + '/accounts', { headers: headers }).toPromise().then(response =>
            response.json() as Account[]).catch(this.handleError);
    }

    private CreateCheckingsAccount(userId: string, initialBalance: number, headers: Headers): Promise<any> {
        return this.http.put(this.urlChecking, JSON.stringify(new CreateService('Checking', userId, initialBalance)), { headers: headers }).toPromise().then(function
                (response: any) {
            console.log("Success");
        }).catch(this.handleError);
    }

    private CreateSavingsAccount(userId: string, initialBalance: number, headers: Headers): Promise<any> {
        return this.http.put(this.urlChecking, JSON.stringify(new CreateService('Savings', userId, initialBalance)), { headers: headers }).toPromise().then(function
                (response: any) {
            console.log("Success");
        }).catch(this.handleError);
    }

    private CreateCreditAccount(userId: string, initialBalance: number, headers: Headers): Promise<any> {
        return this.http.put(this.urlChecking, JSON.stringify(new CreateService('CreditCard', userId, initialBalance)), { headers: headers }).toPromise().then(function (response: any) {
            console.log("Success");
        }).catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }

}