import { UserType, User } from './user';
import { Account } from './account';

export class UserBuilder {

    userType: UserType;
    firstName: string;
    lastName: string;
    fullName: string;
    userId: string;
    gender: string;
    dob: Date;
    address: string;
    phoneNumber: number;
    ssn: number;
    emailAddress: string;
    accounts: Account[];

    user(): UserBuilder {
        return new UserBuilder();
    }

    withType(userType: string): UserBuilder {
        if (userType == "Individual") {
            this.userType = UserType.Individual;
        } else if (userType == "Merchant") {
            this.userType = UserType.Merchant;
        } else if (userType == "Administrator") {
            this.userType = UserType.Administrator;
        } else if (userType == "Employee") {
            this.userType = UserType.Employee;
        } else if (userType == "SystemManager") {
            this.userType = UserType.SystemManager;
        }
        return this;
    }

    withFirstName(firstName: string): UserBuilder {
        this.firstName = firstName;
        return this;
    }

    withLastName(lastName: string): UserBuilder {
        this.lastName = lastName;
        return this;
    }

    withFullName(fullName: string): UserBuilder {
        this.fullName = fullName;
        return this;
    }

    withUserID(userId: string): UserBuilder {
        this.userId = userId;
        return this;
    }

    withGender(gender: string): UserBuilder {
        this.gender = gender;
        return this;
    }

    withDob(dob: string): UserBuilder {
        this.dob = new Date(dob);
        return this;
    }

    withAddress(address: string): UserBuilder {
        this.address = address;
        return this;
    }

    withContactNumber(phoneNumber: string): UserBuilder {
        this.phoneNumber = parseInt(phoneNumber);
        return this;
    }

    withSsn(ssn: number): UserBuilder {
        this.ssn = ssn;
        return this;
    }

    withEmailAddress(emailAddress: string): UserBuilder {
        this.emailAddress = emailAddress;
        return this;
    }

    withAccounts(accounts: Account[]): UserBuilder {
        this.accounts = accounts;
        return this;
    }

    build(): User {
        return new User(this);
    }

}