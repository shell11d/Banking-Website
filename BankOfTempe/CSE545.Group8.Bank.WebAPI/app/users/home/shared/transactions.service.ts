import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { Transaction, TransactionType } from '../model/transaction';
import { TransactionBuilder } from '../model/transaction-builder';
import { Account } from '../model/account';
import { UserService } from './user.service';
import { User } from '../model/user';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class TransactionsService {

    private putTransactionUrl = "api/transaction";
    private headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem("currentUser") });

    constructor(private http: Http,
        private transactionBuilder: TransactionBuilder,
        private userService: UserService) { }

    makeTransfer(fromAccount: number, toAccount: number, amount: number, description: string, otp: number): void {
        let json = {
            "Type": "InternalTransfer",
            "Description": description,
            "Amount": amount,
            "FromAccountId": fromAccount,
            "ToAccountId": toAccount,
            "TransactionMethod": "DebitCard"
        };
        this.userService.getUser().then(user => this.onGetUser(user, json, otp));
    }


    debitCash(fromAccount: number, amount: number): void {
        //Post transaction class needed
        let transaction = this.transactionBuilder.transaction()
                                                 .withFromAccount(fromAccount)
                                                 .withAmount(amount)
                                                 .build();
    }

    creditCash(fromAccount: number, amount: number): void {
        //Post transaction class needed
        let transaction = this.transactionBuilder.transaction()
                                                 .withFromAccount(fromAccount)
                                                 .withAmount(amount)
                                                 .withTransactionType(TransactionType.Credit)
                                                 .build();
    }

    getTransaction(transactionId: number) : void {

    }

    getTransactionsByAccoun(accountId: number) : void {

    }

    getTransactionsByDate(accountId:number, fromDate: Date, toDate: Date): void {

    }

   /* getAllPendingTransactions() : Promise<Transaction[]> {
        return this.http.get(this.userUrl)
                   .toPromise()
                   .then(response => response.json().data[3] as Transaction[]);
    }*/

    approveTransaction(transaction: Transaction): void {

    }

    rejectTransaction(transaction: Transaction): void {

    }

    private onGetUser(user: User, json: any, otp: number): void {
        let headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(user.emailAddress), 'X-OTP' : String(otp)});
        this.http.put(this.putTransactionUrl, json, { headers: headers }).toPromise().then(function (response) {
            console.log("success");
        }).catch(function (error) {console.log(error) });
    }

    private handleError(error: any): void {
        console.log(error);
    }

}