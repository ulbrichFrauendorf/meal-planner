import { Component } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogActionsComponent } from "../dialog-actions/dialog-actions.component";

@Component({
   selector: 'app-confirmation-dialog',
   standalone: true,
   imports: [ButtonModule, ConfirmDialogModule, DialogActionsComponent],
   templateUrl: './confirmation-dialog.component.html',
   styleUrl: './confirmation-dialog.component.scss',
})
export class ConfirmationDialogComponent {}
