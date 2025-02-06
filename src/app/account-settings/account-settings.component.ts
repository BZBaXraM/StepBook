import { Component, inject, signal } from '@angular/core';
import { AccountService } from '../services/account.service';
import { ChangePassword } from '../models/change-password.model';
import { ToastrService } from 'ngx-toastr';
import { ChangeUsername } from '../models/change-username.model';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatTabsModule } from '@angular/material/tabs';

@Component({
    selector: 'app-account-settings',
    imports: [FormsModule, MatTabsModule],
    templateUrl: './account-settings.component.html',
    styleUrl: './account-settings.component.css'
})
export class AccountSettingsComponent {
	accountService = inject(AccountService);
	private router = inject(Router);
	toastr = inject(ToastrService);
	newUsername = signal<ChangeUsername>({
		newUsername: '',
	});
	newPassword = signal<ChangePassword>({
		CurrentPassword: '',
		NewPassword: '',
		ConfirmNewPassword: '',
	});
	passwordFieldType = 'password';

	changeUsername() {
		if (this.newUsername().newUsername.trim() === '') {
			this.toastr.error('Username cannot be empty');
			return;
		}
		this.accountService.changeUsername(this.newUsername()).subscribe({
			next: (_) => {
				this.toastr.success('Username changed successfully');
				this.newUsername.set({ newUsername: '' });
				this.router.navigateByUrl('/');
			},
		});
	}

	changePassword() {
		if (this.newPassword().CurrentPassword.trim() === '') {
			this.toastr.error('Current password cannot be empty');
			return;
		}
		if (this.newPassword().NewPassword.trim() === '') {
			this.toastr.error('New password cannot be empty');
			return;
		}
		this.accountService.changePassword(this.newPassword()).subscribe({
			next: (_) => {
				this.toastr.success('Password changed successfully');
				this.newPassword.set({
					CurrentPassword: '',
					NewPassword: '',
					ConfirmNewPassword: '',
				});
			},
		});
	}

	deleteAccount() {
		this.accountService.deleteAccount().subscribe({
			next: (_) => {
				this.router.navigateByUrl('/login');
			},
		});
	}

	togglePasswordFieldType() {
		this.passwordFieldType =
			this.passwordFieldType === 'password' ? 'text' : 'password';
	}
}
