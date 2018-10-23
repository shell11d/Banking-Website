import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';

import { SecurityQuestions } from '../model/security-questions';
import { UserService } from './user.service';

import { User } from '../model/user';

@Injectable()
export class SecurityQuestionsService {

    private securityQuestionsGetUrl = "api/userInfo/userEmail?userEmail=";
    
    constructor(private http: Http,
                private userService: UserService) { }

    getSecurityQuestions(): Promise<SecurityQuestions> {
        return this.userService.getUser()
            .then(user => this.onGetUser(user as User))
                               .catch(this.handleError);
    }

    updateSecurityQuestions(): void {
        
    }

    private onGetUser(user: User): Promise<SecurityQuestions>  {
        let url = this.securityQuestionsGetUrl + user.emailAddress;
        console.log("url:" + url);
        return this.http.get(url)
            .toPromise()
            .then(response => this.onGetSecurityQuestions(response.json()))
            .catch(this.handleError);
    }
    
    private onGetSecurityQuestions(response: any): SecurityQuestions {
        let securityQuestions = response as SecurityQuestions;
        console.log(securityQuestions);
        return response as SecurityQuestions; 
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    }

}