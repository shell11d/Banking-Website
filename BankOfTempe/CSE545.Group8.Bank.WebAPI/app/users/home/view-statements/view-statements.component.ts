import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './../shared/account.service';
import { Account } from './../model/account';
import { ViewStatementsService } from './view-statements.service';

import { Transaction } from './../model/transaction';
 
@Component({
    selector: 'details-view',
    templateUrl: './app/users/home/view-statements/view-statements.component.html',
    styleUrls: ['./app/users/home/view-statements/view-statements.component.css']
})

    @Injectable()
export class ViewStatementsComponent{

	accounts: Account[];
	constructor(private accountService: AccountService,
        private viewStatementsService: ViewStatementsService, private http:Http) { }



	selectedAccountNumber:number;
	fromDate: Date;
	toDate: Date;
	datesError:string;
    statementStatus: string;
    accountError: boolean;
    accountErrorMessage: string;

    ngOnInit(): void{
        var _this = this;
        this.accountService.getAccounts().then(function (accounts) {
            if (accounts.length < 1 || accounts == null) {
                _this.accountError = true;
                _this.accountErrorMessage = "No accounts retrieved for user";
            }
        });

	}


    getStatements():void{
    	var _this = this;
    	_this.datesError = null;
        _this.statementStatus = null;
    	var difference = (_this.toDate.getTime() - _this.fromDate.getTime())/86400000;
    	if(difference < 0){
    		_this.datesError = "From Date is more than ToDate";
    	}
    	else if(new Date().getTime() - _this.toDate.getTime() < 0){
    		_this.datesError = "To date is a future date";
    	}
    	else if(difference > 60){
    		_this.datesError = "Select Date range in 2 months"; 
    	}
        else {
            this.http.get("api/transactions/" + this.selectedAccountNumber + "/download").toPromise().then(function (response) {
                if (response.status == 200) {
                    _this.statementStatus = "Transactions sent your mail";
                }
            }).catch(function (error) {
                _this.statementStatus = "No transactions for this account";
            });
            _this.statementStatus = "Required Statement Fetched";
        }
		
    }

}






