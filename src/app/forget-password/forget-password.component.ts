import { Component, inject, signal } from '@angular/core';
import { AccountService } from '../services/account.service';
import { FormsModule } from '@angular/forms';
import { ForgetPassword } from '../models/forget.password.model';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../environments/environment';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
@Component({
	selector: 'app-forget-password',
	imports: [FormsModule, MatButtonModule, MatIconModule, MatFormFieldModule],
	templateUrl: './forget-password.component.html',
	styleUrl: './forget-password.component.css',
})
export class ForgetPasswordComponent {
	private accountService = inject(AccountService);
	private baseUrl = environment.apiUrl;
	model = signal<ForgetPassword>({
		email: '',
		clientURI: this.baseUrl + '/Account/reset-password',
	});
	loading = false;
	success = false;
	private toast = inject(ToastrService);

	async forgetPassword() {
		console.log('forgetPassword method called');
		this.loading = true;
		this.accountService.forgetPassword(this.model()).subscribe({
			next: (response) => {
				console.log('Password reset request successful:', response);
				this.success = true;
				this.toast.success(
					'Please check your email for password reset instructions'
				);
			},
			error: (error) => {
				console.error('Error occurred:', error);
				this.toast.error(`${JSON.stringify(error.error)}`);
				this.loading = false;
			},
		});
	}

	reset() {
		this.loading = false;
		this.success = false;
	}
}
