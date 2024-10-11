export class LoginQueryParams {
    ReturnUrl?: string;

    constructor(init?: Partial<LoginQueryParams>) {
        Object.assign(this, init);
    }
}
