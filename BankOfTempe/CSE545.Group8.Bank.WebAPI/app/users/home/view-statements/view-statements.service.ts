import { Injectable } from '@angular/core';

@Injectable()
export class ViewStatementsService {

    requestStatemets(accountNumber: number, fromDate: Date,
    toDate: Date): void {
        console.log("ViewStatementsService" + accountNumber + ' '
                     + toDate + ' ' + fromDate);
    }

    private handleResponse(): void {

    }   

}