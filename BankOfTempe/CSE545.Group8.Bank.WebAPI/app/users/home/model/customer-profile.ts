import { Account } from "./account";

export class CustomerProfile {
  UserId: string;
   firstName: string;
   lastName: string;
  readonly ssn: number;
  readonly dateOfBirth: Date;
  readonly gender: string;
  readonly address: string;
  readonly phoneNumber: number;
   type: string;
  emailAddress: string;


  constructor(UserId: string, firstName: string, lastName: string, ssn: number, dateOfBirth: Date, address: string, gender: string, phoneNumber: number, type:string) {
      this.firstName = firstName;
      this.lastName = lastName;
      this.ssn = ssn;
      this.dateOfBirth = dateOfBirth;
      this.address = address;
      this.phoneNumber = phoneNumber;
      this.type = type;
    }
}