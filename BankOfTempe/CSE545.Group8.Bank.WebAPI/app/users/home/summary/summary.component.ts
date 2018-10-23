import { Component, OnInit } from '@angular/core';

import { Account } from '../model/account';
import { AccountService } from '../shared/account.service';

@Component({
    selector: 'details-view',
    templateUrl: './app/users/home/summary/summary.component.html',
    styleUrls: ['./app/users/home/summary/summary.component.css'],
})

export class SummaryComponent implements OnInit {

    accounts: Account[];

    constructor(private accountService:AccountService) { }

    getAccounts(): void {
        this.accountService.
             getAccounts().
             then(accounts => this.accounts = accounts);
    }

    ngOnInit() : void {
        this.getAccounts();
    }
}