
export enum AccountType {
  Savings,
  Checking,
  Creadit
}

export class Account {
    public type: string;
    public accountId: number;
    public availableBalance: number;

    constructor(accountId: number, type: string, availableBalance:number) {
        this.accountId = accountId;
        this.type = type;
        this.availableBalance = availableBalance;
    }

}

