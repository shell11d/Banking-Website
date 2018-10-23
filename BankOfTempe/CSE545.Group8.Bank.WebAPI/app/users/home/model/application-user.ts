
export class ApplicationUser {
    readonly firstName: string;
    readonly lastName: string;
    readonly username: string;
    readonly password: string;
    readonly confirmPassword: string;
    readonly role: string;

    constructor(firstName: string, lastName: string,
                username: string, password: string,
                confirmPassword: string, role: string) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.username = username;
        this.password = password;
        this.confirmPassword = confirmPassword;
        this.role = role;
    }
}