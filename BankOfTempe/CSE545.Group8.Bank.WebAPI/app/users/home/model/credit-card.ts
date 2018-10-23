
export class CreditCard {
    readonly number: number;
    readonly limit: number;
    readonly bill: number;

    constructor(number:number, limit: number, bill:number) {
        this.number = number;
        this.limit = limit;
        this.bill = bill;
    }
}