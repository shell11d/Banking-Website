import { UserBuilder } from './user-builder';
import { Account } from './account';

export enum UserType {
  Individual,
  Employee,
  Merchant,
  SystemManager,
  Administrator
}

export class User {

    readonly type: UserType;
    readonly firstName: string;
    readonly lastName: string;
    readonly fullName: string;
    readonly userId: string;
    readonly gender: string;
    readonly dob: Date;
    readonly address: string;
    readonly phoneNumber: number;
    readonly ssn: number;
    readonly emailAddress: string;
    readonly accounts: Account[];

    constructor(userBuilder: UserBuilder) {
        this.type = userBuilder.userType;
        this.firstName = userBuilder.firstName;
        this.lastName = userBuilder.lastName;
        this.fullName = userBuilder.fullName;
        this.userId = userBuilder.userId;
        this.gender = userBuilder.gender;
        this.dob = userBuilder.dob;
        this.address = userBuilder.address;
        this.phoneNumber = userBuilder.phoneNumber;
        this.ssn = userBuilder.ssn;
        this.emailAddress = userBuilder.emailAddress;
        this.accounts = userBuilder.accounts;
    }

}
