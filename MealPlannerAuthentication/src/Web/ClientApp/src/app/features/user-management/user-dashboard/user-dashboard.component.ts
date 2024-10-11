import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
} from '@angular/core';
import { ConfirmationService, MessageService, SharedModule } from 'primeng/api';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { AsyncPipe, CommonModule } from '@angular/common';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { ToolbarModule } from 'primeng/toolbar';
import { ToastModule } from 'primeng/toast';
import { ChipModule } from 'primeng/chip';
import { IdentityUserVM, IdentitiesClient } from 'app/web-api-client';
import { WithDestroy } from 'app/_core/mixins/with-destroy-mixin';
import { NewUserFormComponent } from '@users/components/new-user-form/new-user-form.component';

@Component({
    selector: 'app-user-dashboard',
    templateUrl: './user-dashboard.component.html',
    styleUrl: './user-dashboard.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
    imports: [
        ToastModule,
        ToolbarModule,
        SharedModule,
        ButtonModule,
        TableModule,
        ConfirmDialogModule,
        AsyncPipe,
        CommonModule,
        ChipModule,
    ],
    providers: [ConfirmationService, DialogService],
})
export class UserDashboardComponent extends WithDestroy() {
    ref: DynamicDialogRef | undefined;

    displayUserForm$: BehaviorSubject<boolean>;

    formSavedSubject$ = new BehaviorSubject<boolean>(false);

    users$: Observable<IdentityUserVM[]>;

    submitted: boolean;

    statuses: any[];

    //error messages
    constructor(
        private cdr: ChangeDetectorRef,
        private identitiesClient: IdentitiesClient,
        private messageService: MessageService,
        private confirmationService: ConfirmationService,
        private dialogService: DialogService
    ) {
        super();
    }

    ngOnInit() {
        this.users$ = this.formSavedSubject$
            .asObservable()
            .pipe(switchMap(() => this.identitiesClient.getIdentityUsers()));
    }

    deleteIdentityUser(user: IdentityUserVM) {
        this.confirmationService.confirm({
            message:
                'Are you sure you want to delete ' +
                user.identityUser.userName +
                '?',
            header: 'Confirm',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.identitiesClient
                    .deleteIdentityUser(user.identityUser.id)
                    .pipe(tap(() => this.formSavedSubject$.next(true)))
                    .subscribe();
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: 'User Deleted',
                    life: 3000,
                });
            },
        });
    }

    showEditUserDialog(user: IdentityUserVM) {
        this.ref = this.dialogService.open(NewUserFormComponent, {
            header: 'Edit User',
            width: '50rem',
            contentStyle: { overflow: 'auto' },
            breakpoints: {
                '960px': '75vw',
                '640px': '90vw',
            },
            data: {
                userId: user.identityUser.id,
            },
        });

        this.ref.onClose.subscribe((data: any) => {
            if (data) {
                let summary_and_detail = {
                    summary: 'User Updated.',
                    detail: `${data.userName} has been updated.`,
                };

                this.messageService.add({
                    severity: 'success',
                    ...summary_and_detail,
                    life: 3000,
                });

                this.formSavedSubject$.next(true);
                this.cdr.detectChanges();
            }
        });
    }

    showAddUserDialog() {
        this.ref = this.dialogService.open(NewUserFormComponent, {
            header: 'Add New User',
            width: '50rem',
            contentStyle: { overflow: 'auto' },
            breakpoints: {
                '960px': '75vw',
                '640px': '90vw',
            },
        });

        this.ref.onClose.subscribe((data: any) => {
            if (data) {
                let summary_and_detail = {
                    summary: 'User Updated.',
                    detail: `${data.userName} have been updated.`,
                };

                this.messageService.add({
                    severity: 'success',
                    ...summary_and_detail,
                    life: 3000,
                });

                this.formSavedSubject$.next(true);
            }
        });
    }
}
