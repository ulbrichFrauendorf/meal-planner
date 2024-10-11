import { Component, inject } from '@angular/core';
import {
    FormBuilder,
    FormGroup,
    FormsModule,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
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
import { WithDestroy } from 'app/_core/mixins/with-destroy-mixin';
import { LayoutService } from 'app/layout/service/app.layout.service';
import { AccountsClient, ChangePasswordCommand } from 'app/web-api-client';

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
    templateUrl: './change-password.component.html',
    styleUrl: '../../auth-shared.scss',
})
export class ChangePasswordComponent extends WithDestroy() {
    passwordChangeForm: FormGroup;

    router = inject(Router);

    constructor(
        public layoutService: LayoutService,
        private formBuilder: FormBuilder,
        private accountsClient: AccountsClient,
        private messageService: MessageService
    ) {
        super();

        this.passwordChangeForm = this.formBuilder.group({
            currentPassword: ['', Validators.required],
            newPassword: ['', Validators.required],
            confirmNewPassword: ['', Validators.required],
        });
    }

    onSubmit(): void {
        if (this.passwordChangeForm.valid) {
            const { newPassword, confirmNewPassword } =
                this.passwordChangeForm.value;

            if (newPassword !== confirmNewPassword) {
                this.messageService.add({
                    severity: 'error',
                    summary: 'Password Error',
                    detail: 'The new and confirm passwords do not match.',
                    key: 'global',
                });
                return;
            }

            const command = this.passwordChangeForm
                .value as ChangePasswordCommand;

            this.accountsClient.changePassword(command).subscribe({
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
            });
        }
    }

    onCancel() {
        throw new Error('Method not implemented.');
    }
}
