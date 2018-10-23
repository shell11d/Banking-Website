import { TransactionBuilder } from './transaction-builder';

export enum TransactionType {
    Credit,
    Debit
}

export enum TransactionStatus {
    Processing,
    Processed,
    Blocked,
    Reverted
}

export class Transaction {
    
    readonly type: TransactionType;
    readonly id: number;
    readonly date: Date;
    readonly description: string;
    readonly amount: number;
    readonly fromAccount: number;
    readonly toAccount: number;

    constructor(transactionBuilder: TransactionBuilder) {
        this.type = transactionBuilder.transactionType;
        this.id = transactionBuilder.transactionId;
        this.date = transactionBuilder.transactionDate;
        this.description = transactionBuilder.transactionDescription;
        this.amount = transactionBuilder.transactionAmount;
        this.fromAccount = transactionBuilder.fromAccount;
        this.toAccount = transactionBuilder.toAccount;
    }

}