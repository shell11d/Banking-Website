export class ErrorResponse {
    constructor(public error: string, public error_description: string) {
        this.error = error;
        this.error_description = error_description;
    }
}