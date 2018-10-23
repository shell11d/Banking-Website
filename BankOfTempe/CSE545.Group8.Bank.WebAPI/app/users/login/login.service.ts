import { Injectable } from '@angular/core';
import { LoggingUser } from './logginguser.model'; 
import { Router } from '@angular/router';
import { LoggingResponse } from './loginResponse.model';
import { Headers, Http } from '@angular/http';


import 'rxjs/add/operator/toPromise';


@Injectable()
export class LoginService {
  
    constructor(private router: Router, private http: Http) { }

    private loginHeader = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' });


    login(loggingUser: LoggingUser): Promise<LoggingResponse> {
        var requestData = "grant_type=password&userName=" + loggingUser.userName + "&password=" + loggingUser.password;
        return this.http.post("/oauth/token", requestData, this.loginHeader).toPromise().then(response => response.json() as LoggingResponse).catch(this.handleError);
  	}

  	logout(userName:string) {
            localStorage.removeItem(userName);
            localStorage.removeItem('loggedInUser');
  		this.router.navigate(["/login"]);
    }

    logoutAny() {
            localStorage.clear();
            this.router.navigate(["/login"]);
    }

  	isLoggedIn(userName:string){
            if (localStorage.getItem(userName) === null){
  			return false;
  		}
  		return true;
    }

    isLoggedInAny() {
            if (localStorage.length<1) {
                this.router.navigate(["/login"]);
                return false;
            }
            return true;
    }

    private handleError(error: any): void {
            console.error('An error occurred', error);
    }

}