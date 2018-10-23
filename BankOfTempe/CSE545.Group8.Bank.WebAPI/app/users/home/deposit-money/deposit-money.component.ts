import { Component, OnInit } from '@angular/core';
import { Transaction, TransactionType } from '../model/transaction';
import { TransactionsService } from '../shared/transactions.service';

import { Account, AccountType } from '../model/account';
import { AccountService } from '../shared/account.service';
import { UserService } from '../shared/user.service';

import { Headers, Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { User, UserType } from '../model/user';
import { Router } from '@angular/router';

@Component({
    selector: 'details-view',
    templateUrl: './app/users/home/deposit-money/deposit-money.component.html',
    styleUrls: ['./app/users/home/deposit-money/deposit-money.component.css']
})

@Injectable()
export class DepositMoneyComponent  {


    accountsAvailable: Account[];


    accountToDeposit:Account;
    depositMoney:number;
    depositError:string;
    AccountType: typeof AccountType = AccountType;
    acntuserName: string;
    currentUser: User;
    depositUser: User;
    validActUser: boolean;
    public acntUserError = '';
    public invalidActUser = '';
    public hasNoAccounts = '';
    public depositSuccess = '';


    constructor(private accountService:AccountService,
        private transactionService: TransactionsService,
        private http: Http,
        private userService: UserService,
        private router:Router) { }



    getUser(): void {
        this.userService.getUser().then(user => this.currentUser=user);
    }

    ngOnInit(): void {
        this.getUser();
    }


    searchUser(): void {
        var userUrl = "api/users/userEmail?userEmail=" + this.acntuserName;
        var _this = this;
        var headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(this.currentUser.emailAddress) });
        this.http.get(userUrl, { headers: headers })
            .toPromise().then(function (response) {
                _this.depositUser = _this.userService.handleUserInfo(response.json());
                if (_this.depositUser != null && (_this.depositUser.type == UserType.Individual || _this.depositUser.type == UserType.Merchant)) {

                    var actUrl = "api/users/" + _this.acntuserName + "/accounts";
                    _this.http.get(userUrl, { headers: headers })
                        .toPromise().then(function (response) {
                            _this.accountsAvailable = response.json() as Account[];

                            if (_this.accountsAvailable != null && _this.accountsAvailable.length>0) {
                                _this.validActUser = true;
                            } else {
                                _this.hasNoAccounts = "User Has no accounts associated.";
                            }
                        })
                        .catch(function (error) {
                            _this.hasNoAccounts = "User Has no accounts associated";
                        });
                     
                    
                }
                else {
                    _this.acntUserError = "Invalid User. Try Again";
                }
            })
            .catch(function (error) {
                _this.invalidActUser = "Invalid User Name. try again";
            });
    }

    depositAmount(): void {
        var headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(this.currentUser.emailAddress) });
        var _this = this;
        if (this.accountToDeposit.type === "Checking") {
            this.http.post("api/users/accounts/checking/" + this.accountToDeposit.accountId, { headers: headers }).toPromise().then(function (response) {
                if (response.status == 200) {
                    _this.depositSuccess = "Deposit Money Successful";
                }
            }).catch(function (error) {
                _this.depositError = "Deposit Failure";
            });
        } else if (this.accountToDeposit.type === "Savings") {

            this.http.post("api/users/accounts/savings/" + this.accountToDeposit.accountId, { headers: headers }).toPromise().then(function (response) {
                if (response.status == 200) {
                    _this.depositSuccess = "Deposit Money Successful";
                }
            }).catch(function (error) {
                _this.depositError = "Deposit Failure";
            });
                
        } else {
            this.http.post("api/users/accounts/creditcard/" + this.accountToDeposit.accountId, { headers: headers }).toPromise().then(function (response) {
                if (response.status == 200) {
                    _this.depositSuccess = "Deposit Money Successful";
                }
            }).catch(function (error) {
                _this.depositError = "Deposit Failure";
            });
        }
        
    }

    doneDeposit(): void {
        this.router.navigate(["/mainmenu"]);
    }
	
}