import { TransactionType, Transaction } from './transaction'

export class TransactionBuilder {

    transactionType: TransactionType;
    transactionId: number;
    transactionDate: Date;
    transactionDescription: string;
    transactionAmount: number;
    fromAccount: number;
    toAccount: number;

    transaction(): TransactionBuilder {
        return new TransactionBuilder();
    }

    withTransactionType(transactionType: TransactionType) : TransactionBuilder {
        this.transactionType = transactionType;
        return this;
    }

    withId(transactionId: number) : TransactionBuilder {
        this.transactionId = transactionId;
        return this;
    }

    withDate(transactionDate: Date) : TransactionBuilder {
        this.transactionDate = transactionDate;
        return this;
    }

    withDescription(transactionDescription: string) : TransactionBuilder {
        this.transactionDescription = transactionDescription;
        return this;
    }

    withAmount(transactionAmount: number) : TransactionBuilder {
        this.transactionAmount = transactionAmount;
        return this;
    }

    withFromAccount(fromAccount: number) : TransactionBuilder {
        this.fromAccount = fromAccount;
        return this;
    }

    withToAccount(toAccount: number) : TransactionBuilder {
        this.toAccount = toAccount;
        return this;
    }

    build() {
        return new Transaction(this);
    }

}