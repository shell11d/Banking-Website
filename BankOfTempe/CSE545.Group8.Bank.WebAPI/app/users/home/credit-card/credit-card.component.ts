import { Component, OnInit } from '@angular/core';
import { Transaction, TransactionType } from '../model/transaction';
import { User } from '../model/user';
import { TransactionsService } from '../shared/transactions.service';
import { UserService } from '../shared/user.service';
import { Http, Headers } from '@angular/http';
import { Account } from '../model/account';


class CreditCard {
    accountid: string;
    customerid: string;
    avavailablebabalance: string;
    createdate: string;
    closedate: string;
    creditlimit: number;
    cardnumber: string;
    cvv: string;
    dateofexpiry: string;
    dateofissue: string;
    nameoncard: string;
    status: string;
}

@Component({
    selector: 'details-view',
    templateUrl: './app/users/home/credit-card/credit-card.component.html',
    styleUrls: ['./app/users/home/credit-card/credit-card.component.css']
})

export class CreditCardComponent implements OnInit {

    private url = "";
    private headers: Headers;

    constructor(private userService: UserService,
        private http: Http,
        private transactionService: TransactionsService    ) { }


    ngOnInit(): void {
        this.userService.getUser().then(user => this.onGetUser(user));
    }

    onGetUser(user: User): void {
        let headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(user.emailAddress) });
        let url = "api/accounts/creditCard/" + user.userId;
        this.http.get(url, { headers: headers }).toPromise().then(response => console.log(response)).catch(error => console.log(error));
    }

    onGetCreditCard(account: Account): void {
    }

    onGetCreditCardDetails(): void {

    }

    makeTransfer(): void {
        
    }
	
}