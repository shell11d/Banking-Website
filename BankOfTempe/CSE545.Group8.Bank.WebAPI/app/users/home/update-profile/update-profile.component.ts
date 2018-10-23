import { Component, OnInit } from '@angular/core';

import { User } from '../model/user';
import { SecurityQuestions } from '../model/security-questions';

import { UserService } from '../shared/user.service';
import { SecurityQuestionsService } from '../shared/security-questions.service';

@Component({
    selector: 'details-view',
    templateUrl: './app/users/home/update-profile/update-profile.component.html',
    styleUrls: ['./app/users/home/update-profile/update-profile.component.css']
    
})

export class UpdateProfileComponent implements OnInit {

    user: User;

    constructor(private userService: UserService) { }

    updateProfile(): void {
        this.userService.updateUserInfo(this.user);
    }

    ngOnInit(): void {
        this.getUserInfo();
    }

    private getUserInfo(): void {
        this.userService.getUser().then(user => this.onGetProfile(user as User));
    }

    private onGetProfile(user: User): void {
        this.user = user;
    }

}