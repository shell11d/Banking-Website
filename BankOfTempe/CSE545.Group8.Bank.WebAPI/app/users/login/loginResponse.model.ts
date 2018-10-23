export class LoggingResponse{
    constructor(public access_token: string, public token_type: string, public expires_in: number, public error: string, public error_description: string) {
        this.access_token = access_token;
        this.token_type = token_type;
        this.expires_in = expires_in;

	}
}