import { Component } from '@angular/core';
import { Transaction, TransactionType } from '../model/transaction';
import { TransactionsService } from '../shared/transactions.service';
import { Account, AccountType } from '../model/account';
import { AccountService } from '../shared/account.service';
import { Headers, Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { UserService } from '../shared/user.service';
import { User, UserType } from '../model/user';
import { Router } from '@angular/router';

@Component({
    selector: 'details-view',
    templateUrl: './app/users/home/add-account/add-account.component.html',
    styleUrls: ['./app/users/home/add-account/add-account.component.css']
})

@Injectable()
export class AddAccountComponent {

    CustomerId: number;
    Type: string;
    AvailableBalance: number;
    selAccount: string;
    validUser: boolean;
    adduserName: string;
    addUserError: string;
    public addAccountMessage='';
    public invalidUser='';
    public invalidAccountCreation='';

    AccountType: typeof AccountType = AccountType;
    accountAser: User;

    constructor(private accountService: AccountService, private http: Http, private userService: UserService, private router:Router) { }
    private headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem("currentUser") });

    searchUser(): void {
        var userUrl = "api/users/userEmail?userEmail=" + this.adduserName;
        var _this = this;
        this.http.get(userUrl, { headers: this.headers })
        .toPromise().then(function (response) {
            _this.accountAser = _this.userService.handleUserInfo(response.json());
            if (_this.accountAser != null && (_this.accountAser.type == UserType.Individual || _this.accountAser.type == UserType.Merchant)) {
                _this.validUser = true;
            } 
            else {
                _this.addUserError = "Invalid User. Try Again";
            }    
            })
            .catch(function (error) {
                _this.invalidUser = "Invalid User Name. try again";
            });
    }

    createUserAccount(): void {
        var _this1 = this;
        this.accountService.createAccount(this.accountAser.userId, this.selAccount, this.AvailableBalance).then(function (response) {
            _this1.addAccountMessage = "Account created successfull with account number: " + response;
        }).catch(function (error: any) {
            _this1.invalidAccountCreation = "Account cannot be created ";
            });

    }

    selectedAccountToAdd(selectedAccountType: string): void {
    }

    doneAccount(): void {
        this.router.navigate(["/mainmenu"]);
    }
}