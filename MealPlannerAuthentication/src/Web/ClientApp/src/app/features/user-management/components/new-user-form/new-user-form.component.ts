import { Component, OnChanges, OnInit } from '@angular/core';
import {
    FormBuilder,
    FormGroup,
    Validators,
    FormsModule,
    ReactiveFormsModule,
    FormControl,
} from '@angular/forms';
import { Message } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { catchError, of, takeUntil } from 'rxjs';
import { WithDestroy } from 'app/_core/mixins/with-destroy-mixin';
import {
    ClaimsClient,
    CreateIdentityUserCommand,
    IdentitiesClient,
    IdentityUserVM,
    UpdateIdentityUserCommand,
} from 'app/web-api-client';
import { ButtonModule } from 'primeng/button';
import { MessagesModule } from 'primeng/messages';
import { InputTextModule } from 'primeng/inputtext';
import { FloatLabelModule } from 'primeng/floatlabel';
import { FieldsetModule } from 'primeng/fieldset';
import { ChipsModule } from 'primeng/chips';
import { CommonModule } from '@angular/common';

const RoleClaimConst =
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
const SystemClaimConst =
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/system';

@Component({
    selector: 'app-new-user-form',
    templateUrl: './new-user-form.component.html',
    styleUrl: './new-user-form.component.scss',
    standalone: true,
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        FloatLabelModule,
        InputTextModule,
        MessagesModule,
        ButtonModule,
        FieldsetModule,
        ChipsModule,
    ],
})
export class NewUserFormComponent
    extends WithDestroy()
    implements OnInit, OnChanges
{
    userId: string | null;
    user: IdentityUserVM;
    userForm: FormGroup;
    claimsForm: FormGroup;
    emailErrors: Message[] | undefined;
    passwordErrors: Message[] | undefined;

    constructor(
        private fb: FormBuilder,
        private identitiesClient: IdentitiesClient,
        private claimsClient: ClaimsClient,
        public ref: DynamicDialogRef,
        public config: DynamicDialogConfig
    ) {
        super();
    }

    ngOnInit() {
        this.initializeForm();

        if (this.userId) {
            this.loadUserData();
        }
    }

    ngOnChanges(): void {
        if (this.user) {
            this.userForm.patchValue({
                email: this.user.identityUser.email,
            });
        }
    }

    initializeForm() {
        this.userId = this.config?.data?.userId;
        this.userForm = this.fb.group({
            email: ['', [Validators.required, Validators.email]],
            roleClaims: new FormControl<string[] | null>(null),
            systemClaims: new FormControl<string[] | null>(null),
        });
    }

    loadUserData() {
        this.identitiesClient.getIdentityUser(this.userId).subscribe({
            next: (user) => {
                this.user = user;
                console.log(user);
                this.userForm.patchValue({
                    email: user.identityUser.email,
                    roleClaims: user.roleClaims,
                    systemClaims: user.systemClaims,
                });
            },
            error: (err) => {
                console.error('Failed to load user data', err);
            },
        });
    }

    onCancel() {
        this.ref.close();
    }

    onSubmit() {
        if (this.userForm.valid) {
            if (this.userId) {
                // Update existing user
                const updateCommand: UpdateIdentityUserCommand = {
                    userId: this.userId,
                    ...this.userForm.value,
                };
                this.identitiesClient
                    .updateIdentityUser(this.userId, updateCommand)
                    .pipe(
                        catchError((err) => of([])),
                        takeUntil(this.destroy$)
                    )
                    .subscribe({
                        next: (response) => {
                            this.ref.close(response);
                        },
                    });
            } else {
                // Create new user
                const createCommand: CreateIdentityUserCommand =
                    this.userForm.value;
                this.identitiesClient
                    .createIdentityUser(createCommand)
                    .pipe(
                        catchError((err) => of([])),
                        takeUntil(this.destroy$)
                    )
                    .subscribe({
                        next: (response) => {
                            this.ref.close(response);
                        },
                    });
            }
        }
    }
}
