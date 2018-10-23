export class SecurityQuestions {
    userId: string;
    securityQuestion1: string;
    securityAnswer1: string;
    securityQuestion2: string;
    securityAnswer2: string;
    securityQuestion3: string;
    securityAnswer3: string;

    constructor(userId: string, securityQuestion1: string, securityAnswer1: string, securityQuestion2: string, securityAnswer2: string, securityQuestion3: string, securityAnswer3: string) {
        this.securityQuestion1 = securityQuestion1;
        this.securityAnswer1 = securityAnswer1;
        this.securityQuestion2 = securityQuestion2;
        this.securityAnswer2 = securityAnswer2;
        this.securityQuestion3 = securityQuestion3;
        this.securityAnswer3 = securityAnswer3;
        this.userId = userId;
    }
}