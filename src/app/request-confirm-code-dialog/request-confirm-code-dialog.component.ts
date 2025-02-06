import { Component, Inject, inject, signal } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { AccountService } from '../services/account.service';
import { MatDialogRef } from '@angular/material/dialog';
import { RequestConfirmationCode } from '../models/request-confirmation-code.model';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-request-confirm-code-dialog',
    imports: [FormsModule, MatDialogModule, MatFormFieldModule, MatInputModule],
    templateUrl: './request-confirm-code-dialog.component.html',
    styleUrl: './request-confirm-code-dialog.component.css'
})
export class RequestConfirmCodeDialogComponent {
	private accountService = inject(AccountService);
	dialogRef = inject(MatDialogRef<RequestConfirmCodeDialogComponent>);
	model = signal<RequestConfirmationCode>({ Email: '' });
	private router = inject(Router);
	private toast = inject(ToastrService);

	constructor(@Inject(MAT_DIALOG_DATA) public data: { email: string }) {
		this.model().Email = data.email;
	}

	async requestConfirmationCode() {
		this.accountService.requestConfirmationCode(this.model()).subscribe({
			next: () => this.router.navigateByUrl('/confirm-email'),
			error: (e) => this.toast.error(e.error),
		});
	}

	close() {
		this.dialogRef.close();
	}
}
