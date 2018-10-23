import { Component, OnInit } from '@angular/core';
import { Account } from '../model/account';
import { User } from '../model/user';
import { AccountService } from '../shared/account.service';
import { MerchantService } from '../shared/merchant.service';
import { TransactionsService } from '../shared/transactions.service';

@Component({
    selector: 'details-view',
    templateUrl: './app/users/home/payments/payments.component.html',
    styleUrls: ['./app/users/home/payments/payments.component.css']
})

export class PaymentsComponent implements OnInit{

    accounts: Account[];
    selectedAccount: Account;
    merchants: User[];
    amount: number;
    selectedAccountId: number;
    selectedMerchant: User;
    description: string;
    otp: number;

    constructor(private accountService:AccountService,
        private merchantService: MerchantService,
        private transactionsService: TransactionsService) { }

    getAccounts(): void {
        this.accountService.
             getAccounts().
             then(accounts => this.accounts = accounts);
    }

    getMerchants(): void {
        this.merchantService.
             getMerchants().
             then(merchants => this.merchants = merchants).catch(error => console.log(error));
    }

    ngOnInit() : void {
        this.getAccounts();
        this.getMerchants();
    }

    abc(): void {
        this.transactionsService.makeTransfer(this.accounts[0].accountId, this.merchants[0].accounts[0].accountId, this.amount, this.description, this.otp);
    }

}