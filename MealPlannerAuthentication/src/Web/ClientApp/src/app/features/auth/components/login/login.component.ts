import { Component, inject } from '@angular/core';
import { CheckboxModule } from 'primeng/checkbox';
import { LayoutService } from '@layout/service/app.layout.service';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import {
    FormBuilder,
    FormGroup,
    FormsModule,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';
import { FloatLabelModule } from 'primeng/floatlabel';
import { CommonModule } from '@angular/common';
import {
    AccountsClient,
    ForgotPasswordCommand,
    LoginCommand,
    LoginDto,
    LoginResponse,
} from '../../../../web-api-client';
import { catchError, Observable, of, tap } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { WithDestroy } from '../../../../_core/mixins/with-destroy-mixin';
import { LoginQueryParams } from '../../login-query-params';
import { MessagesModule } from 'primeng/messages';
import { ConfirmationService, MessageService } from 'primeng/api';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [
        CommonModule,
        CheckboxModule,
        CardModule,
        ButtonModule,
        InputTextModule,
        FormsModule,
        ReactiveFormsModule,
        FloatLabelModule,
        MessagesModule,
        ConfirmDialogModule,
    ],
    providers: [ConfirmationService],
    templateUrl: './login.component.html',
    styleUrl: '../../auth-shared.scss',
})
export class LoginComponent extends WithDestroy() {
    private readonly oidcSecurityService = inject(OidcSecurityService);

    constructor(
        public layoutService: LayoutService,
        private route: ActivatedRoute,
        private accountsClient: AccountsClient,
        private fb: FormBuilder,
        private confirmationService: ConfirmationService,
        private messageService: MessageService
    ) {
        super();
        this.route.queryParams.subscribe((params) => {
            this.loginQueryParams = new LoginQueryParams(params);

            if (
                this.loginQueryParams === null ||
                (typeof this.loginQueryParams === 'object' &&
                    Object.keys(this.loginQueryParams).length === 0)
            ) {
                this.oidcSecurityService.authorize();
            }
            this.accountsClient
                .getLoginData(this.loginQueryParams.ReturnUrl)
                .subscribe({
                    next: (data) => {
                        this.loginPreData = data;
                    },
                });
        });
        this.loginForm = this.fb.group({
            username: ['', Validators.required],
            password: ['', Validators.required],
            rememberLogin: [false],
        });
    }

    errorMessage: string | null = null;

    loginForm: FormGroup;
    loginQueryParams: LoginQueryParams;
    loginPreData: LoginDto;
    loginResponse$: Observable<LoginResponse>;

    onSubmit() {
        if (this.loginForm.valid) {
            const loginObj = {
                username: this.loginForm.value.username,
                password: this.loginForm.value.password,
                rememberLogin: this.loginForm.value.rememberLogin,
                returnUrl: this.loginQueryParams.ReturnUrl,
            };
            this.loginResponse$ = this.accountsClient
                .login(loginObj as LoginCommand)
                .pipe(
                    tap((response: any) => {
                        if (response?.redirectUrl) {
                            window.location.href = response.redirectUrl;
                        }
                        this.errorMessage = response.errorMessage;
                    }),
                    catchError(() => {
                        this.errorMessage =
                            'Login failed. Please check your credentials.';
                        return of(null); // Return an empty observable so the stream completes
                    })
                );

            this.loginResponse$.subscribe({
                next: (res: LoginResponse) => {
                    if (!res.isSuccess) {
                        this.messageService.add({
                            severity: 'error',
                            summary: 'Login Error',
                            detail: res.errorMessage,
                            key: 'global',
                        });
                    }
                },
            });
        }
    }

    onForgotPassword(): void {
        console.log('onForgot', this.loginForm.get('username'));
        const username = this.loginForm.get('username');

        if (username && username.valid) {
            console.log('valid username', username.value);

            this.confirmationService.confirm({
                message: 'Please confirm, to reset your password.',
                header: 'Are you sure?',

                accept: () => {
                    const forgotPasswordCommand: ForgotPasswordCommand = {
                        email: this.loginForm.value.username,
                    } as ForgotPasswordCommand;

                    this.accountsClient
                        .forgotPassword(forgotPasswordCommand)
                        .subscribe({
                            next: () => {
                                this.messageService.add({
                                    severity: 'success',
                                    summary: 'Password Reset',
                                    detail: 'Password reset instructions sent.',
                                    key: 'global',
                                });
                            },
                            error: () => {
                                this.messageService.add({
                                    severity: 'error',
                                    summary: 'Reset Error',
                                    detail: 'Failed to send password reset instructions.',
                                    key: 'global',
                                });
                            },
                        });
                },
            });
        } else {
            this.messageService.add({
                severity: 'warn',
                summary: 'Validation Error',
                detail: 'Please enter a valid username.',
                key: 'global',
            });
        }
    }
}
