import { Component, inject } from '@angular/core';
import { AccountService } from '../services/account.service';
import {
	FormBuilder,
	FormControl,
	FormGroup,
	ReactiveFormsModule,
	Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { NgIf } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { RequestConfirmCodeDialogComponent } from '../request-confirm-code-dialog/request-confirm-code-dialog.component';

@Component({
    selector: 'app-confirm-email',
    imports: [
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        ReactiveFormsModule,
        NgIf,
    ],
    templateUrl: './confirm-email.component.html',
    styleUrl: './confirm-email.component.css'
})
export class ConfirmEmailComponent {
	form: FormGroup = new FormGroup({
		Email: new FormControl('', [Validators.email, Validators.required]),
	});

	private dialog = inject(MatDialog);

	constructor(
		private accountService: AccountService,
		private formBuilder: FormBuilder,
		private router: Router
	) {
		this.form = this.formBuilder.group({
			code: ['', Validators.required],
		});
	}

	async confirmEmailCode() {
		this.accountService.confirmEmailCode(this.form.value).subscribe({
			next: (_) => {
				this.router.navigateByUrl('/login');
			},
			error: (error) => {
				console.error(`${JSON.stringify(error.error)}`);
			},
		});
	}

	openRequestConfirmCodeDialog() {
		const dialogRef = this.dialog.open(RequestConfirmCodeDialogComponent, {
			data: { email: this.form.value.Email },
			autoFocus: true,
		});

		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				this.confirmEmailCode();
			}
		});
	}

	closeDialog() {
		this.dialog.closeAll();
	}
}
