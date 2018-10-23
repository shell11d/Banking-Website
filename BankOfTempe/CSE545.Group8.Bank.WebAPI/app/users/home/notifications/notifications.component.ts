import { Component, OnInit } from '@angular/core';
import { Transaction, TransactionType } from '../model/transaction';
import { TransactionsService } from '../shared/transactions.service';
import { Headers, Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { UserService } from '../shared/user.service';
import { User, UserType } from '../model/user';
import { Router } from '@angular/router';

@Component({
    selector: 'details-view',
    templateUrl: './app/users/home/notifications/notifications.component.html',
    styleUrls: ['./app/users/home/notifications/notifications.component.css']
})

export class NotificationsComponent implements OnInit {

	transactions: Transaction[];
    TransactionType: typeof TransactionType = TransactionType;
    currentUser: User;
    noTranMessage = '';

    constructor(private transactionsService: TransactionsService, private userService: UserService, private http: Http, private router: Router) { }


    ngOnInit(): void {
        var _this = this;
        this.userService.getUser().then(function (user) {
            _this.currentUser = user;
            
            var headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(_this.currentUser.emailAddress) });

            if (_this.currentUser.type == UserType.Employee) {
                _this.http.get("api/transaction/banker/pending", { headers: headers }).toPromise().then(function (response) {
                    _this.transactions = response.json() as Transaction[];
                    if (_this.transactions.length < 1 || _this.transactions == null) {
                        _this.noTranMessage = "No transactions pending for approval";
                    }
                }).catch(function (error) {
                    _this.noTranMessage = "No transactions pending for approval";
                });
            } else if (_this.currentUser.type == UserType.SystemManager) {
                _this.http.get("api/transaction/manager/pending", { headers: headers }).toPromise().then(function (response) {
                    _this.transactions = response.json() as Transaction[];
                    if (_this.transactions.length < 1 || _this.transactions == null) {
                        _this.noTranMessage = "No transactions pending for approval";
                    }
                }).catch(function (error) {
                    _this.noTranMessage = "No transactions pending for approval";
                });
            }

        });
 
    }


    OnTransactionApproved(transaction: Transaction): void {
        var headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(this.currentUser.emailAddress) });
        var _this = this;
        if (this.currentUser.type == UserType.Employee) {
            this.http.post("api/transaction/approve/banker/" + transaction.id, { headers: headers }).toPromise().then(function (response) {
                if (response.status == 200) {
                   //
                }
            }).catch(function (error) {
                
                });
        } else if (this.currentUser.type == UserType.SystemManager) {
            this.http.post("api/transaction/manager/banker/" + transaction.id, { headers: headers }).toPromise().then(function (response) {

            }).catch(function (error) { });
        }
       
	}

}