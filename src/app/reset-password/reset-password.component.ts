import { Component, inject, signal } from '@angular/core';
import { AccountService } from '../services/account.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { ResetPassword } from '../models/reset-password.model';
import { ToastrService } from 'ngx-toastr';

@Component({
	selector: 'app-reset-password',
	imports: [FormsModule, NgIf],
	templateUrl: './reset-password.component.html',
	styleUrl: './reset-password.component.css',
})
export class ResetPasswordComponent {
	private accountService = inject(AccountService);
	private toast = inject(ToastrService);
	model = signal<ResetPassword>({
		email: '',
		code: '',
		NewPassword: '',
	});
	loading = false;
	success = false;
	passwordFieldType = 'password';

	private router = inject(Router);

	async resetPassword() {
		this.loading = true;
		this.accountService.resetPassword(this.model()).subscribe({
			next: (_) => {
				this.success = true;
				this.loading = false;
				this.toast.success('Password reset successfully');
				this.router.navigateByUrl('/login');
			},
			error: (_) => {
				this.loading = false;
				this.toast.error('An error occurred');
			},
		});
	}

	reset() {
		this.loading = false;
		this.success = false;
	}

	togglePasswordVisibility() {
		this.passwordFieldType =
			this.passwordFieldType === 'password' ? 'text' : 'password';
	}
}
