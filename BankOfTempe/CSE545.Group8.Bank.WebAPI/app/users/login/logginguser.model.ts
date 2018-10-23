export class LoggingUser{
    constructor(public userName: string, public password: string, public grant_type:string){
        this.userName = userName;
        this.password = password;
        this.grant_type = grant_type;
	}
}