    import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';

import { User } from '../model/user';
import { UserBuilder } from '../model/user-builder';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class UserService {
    private userId: string;
    private userUrl = "api/users/userEmail?userEmail=";
    private headers: Headers;
    private user: User;

    constructor(private http: Http,
        private userBuilder: UserBuilder) { }

    setUserId(userId: string): void {
        this.userId = userId;
        this.headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(this.userId) });
    }

    getUser(): Promise<User> {
        if (this.user != null) {
            return Promise.resolve(this.user);
        }
        return this.http.get(this.userUrl + this.userId, { headers: this.headers })
				   .toPromise()
				   .then(response => this.handleUserInfo(response.json()))
				   .catch(this.handleError);
    }

    createApplicationUser(): void {

    }

    createUser(): void {

    }

    updateUserInfo(user: User): void {
        this.user = user;
        this.http.post(this.userUrl + this.user.emailAddress, JSON.stringify(user), { headers: this.headers }).toPromise().then(function (response) {
            console.log("success");
        }).catch(this.handleError);
    }

    handleUpdateInfo(): void {

    }

    handleUserInfo(response: any): User {
        this.user = this.userBuilder.user()
            .withFirstName(response['firstName'])
            .withLastName(response['lastName'])
            .withFullName(response['fullName'])
            .withDob(response['dateOfBirth'])
            .withAddress(response['address'])
            .withContactNumber(response['phoneNumber'])
            .withEmailAddress(response['emailAddress'])
            .withSsn(response['ssn'])
            .withType(response['type'])
            .withUserID(response['userId'])
            .withGender(response['gender'])
            .build();
        
        return this.user;
    }

	private handleError(error: any): Promise<any> {
		console.error('An error occurred', error); // for demo purposes only
		return Promise.reject(error.message || error);
    }

}