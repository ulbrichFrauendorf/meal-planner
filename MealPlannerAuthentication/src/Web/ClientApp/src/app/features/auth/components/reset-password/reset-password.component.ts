import { Component, inject, OnInit } from '@angular/core';
import {
    FormBuilder,
    FormGroup,
    FormsModule,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { MessagesModule } from 'primeng/messages';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { WithDestroy } from '../../../../_core/mixins/with-destroy-mixin';
import { LayoutService } from '@layout/service/app.layout.service';
import {
    AccountsClient,
    ChangePasswordCommand,
    ResetPasswordCommand,
} from '../../../../web-api-client';

@Component({
    selector: 'app-reset-password',
    standalone: true,
    imports: [
        ToolbarModule,
        ButtonModule,
        TableModule,
        ToastModule,
        DialogModule,
        ConfirmDialogModule,
        InputTextModule,
        MessagesModule,
        ReactiveFormsModule,
        FormsModule,
        FloatLabelModule,
    ],
    templateUrl: './reset-password.component.html',
    styleUrl: '../../auth-shared.scss',
})
export class ResetPasswordComponent extends WithDestroy() implements OnInit {
    resetPasswordForm: FormGroup;
    token: string;

    constructor(
        public layoutService: LayoutService,
        private route: ActivatedRoute,
        private fb: FormBuilder,
        private router: Router,
        private messageService: MessageService,
        private accountsClient: AccountsClient
    ) {
        super();
    }

    ngOnInit(): void {
        // Get the token from the URL
        this.token = this.route.snapshot.queryParams['token'];

        console.log(this.token);

        // Handle the case where the token is missing or invalid
        if (!this.token || this.token.trim() === '') {
            console.log('fucked');
            this.messageService.add({
                severity: 'error',
                summary: 'Token Error',
                detail: 'No valid token provided. Redirecting to home page.',
                key: 'global',
            });
            this.router.navigate(['/']);
            return;
        }
        console.log('init form');
        this.resetPasswordForm = this.fb.group({
            newPassword: ['', Validators.required],
            confirmNewPassword: ['', Validators.required],
        });
    }

    onSubmit(): void {
        if (this.resetPasswordForm.valid) {
            const { newPassword, confirmNewPassword } =
                this.resetPasswordForm.value;

            if (newPassword !== confirmNewPassword) {
                this.messageService.add({
                    severity: 'error',
                    summary: 'Password Error',
                    detail: 'The new and confirm passwords do not match.',
                    key: 'global',
                });
                return;
            }

            const command = {
                resetToken: this.token,
                newPassword,
                confirmNewPassword,
            } as ResetPasswordCommand;

            this.accountsClient.resetPassword(command).subscribe({
                next: () => {
                    this.messageService.add({
                        severity: 'success',
                        summary: 'Password Changed',
                        detail: 'Your password has been changed successfully',
                        key: 'global',
                    });
                },
                complete: () => {
                    this.router.navigate(['/auth/login']);
                },
                error: (error) => {
                    this.messageService.add({
                        severity: 'error',
                        summary: 'Password Change Failed',
                        detail: 'An error occurred while resetting your password.',
                        key: 'global',
                    });
                },
            });
        }
    }
}
