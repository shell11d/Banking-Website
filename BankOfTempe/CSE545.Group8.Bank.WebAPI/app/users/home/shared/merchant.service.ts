import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';

import { User } from '../model/user';
import { UserService } from './user.service';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class MerchantService {

    constructor(private http: Http,
                private userService: UserService) { }	

    getMerchants(): Promise<any> {
        var _this = this;
        return this.userService.getUser().then(function (user: User) {
                let headers = new Headers({ 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + localStorage.getItem(user.emailAddress) });
                return _this.http.get('api/users/merchants', { headers: headers }).toPromise().then(response => response.json() as User[]).catch(error => console.log(error));
        });
    }

	private handleError(error: any): Promise<any> {
		console.error('An error occurred', error); // for demo purposes only
		return Promise.reject(error.message || error);
	}
}