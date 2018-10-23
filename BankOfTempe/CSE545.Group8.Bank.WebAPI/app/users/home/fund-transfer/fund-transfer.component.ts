import { Component, OnInit } from '@angular/core';
import {NgSwitch , NgSwitchDefault} from '@angular/common';
import { Account, AccountType } from '../model/account';
import { AccountService } from '../shared/account.service';
import { TransactionsService } from '../shared/transactions.service';

//TODO: needs to be removed
enum TransferType {
    Account,
    Email,
    Mobile
}

@Component({
    selector: 'details-view',
    templateUrl: './app/users/home/fund-transfer/fund-transfer.component.html',
    styleUrls: ['./app/users/home/fund-transfer/fund-transfer.component.css'],
    inputs: ['NgSwitch', 'NgSwitchDefault']
})

export class FundTransferComponent implements OnInit {

    AccountType: typeof AccountType = AccountType;
    TransferType : typeof TransferType = TransferType;
    transferType: TransferType;
    accounts: Account[];
    receiverDetails: any;
    amount: number;
    selectedAccount: Account;
    description: string;
    selectedAccountId: number;
    otp: number;

    constructor(private accountService:AccountService,
                private transactionsService: TransactionsService) { }

    onSubmit(): void {
        var _this = this;
        alert(this.selectedAccountId);
        if (this.transferType == TransferType.Account) {
            this.transactionsService.makeTransfer(this.selectedAccountId, this.receiverDetails as number, this.amount, this.description, _this.otp);
        } else if (this.transferType == TransferType.Email) {
            this.accountService.getAccountByMail(this.receiverDetails as string).then(function (account: Account) {
                _this.transactionsService.makeTransfer(_this.selectedAccountId, account.accountId, _this.amount, _this.description, _this.otp);
            });
        } else if (this.transferType == TransferType.Mobile) {
            this.accountService.getAccountByPhoneNumber(this.receiverDetails as string).then(function (account: Account) {
                _this.transactionsService.makeTransfer(_this.selectedAccountId, account.accountId, _this.amount, _this.description, _this.otp);
            });
        }
    }

    onAccountSelect(account: Account) {
        this.selectedAccount = account;
    }

    getAccounts(): void {
        this.accountService.
            getAccounts().
            then(accounts => this.onGetAccounts(accounts));
    }

    onGetAccounts(accounts: Account[]): void {
        console.log(accounts[0].accountId);
        this.accounts = accounts;
    }   

    ngOnInit() : void {
        this.transferType = TransferType.Account;
        this.getAccounts();
    }

}