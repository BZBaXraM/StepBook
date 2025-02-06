import { Component, inject, signal } from '@angular/core';
import { AdminService } from '../services/admin.service';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-tron',
    imports: [
        CommonModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        FormsModule,
        RouterLink,
    ],
    templateUrl: './tron.component.html',
    styleUrl: './tron.component.css'
})
export class TronComponent {
	private adminService = inject(AdminService);
	username = '';
	private toastr = inject(ToastrService);

	addToBlackList(username: string) {
		this.adminService.addToBlackList(username).subscribe({
			next: () => this.toastr.success('User added to blacklist'),
			error: (error) =>
				this.toastr.error(
					error.error || 'Failed to add user to blacklist'
				),
		});
	}

	removeFromBlackList(username: string) {
		this.adminService.removeFromBlackList(username).subscribe({
			next: () => this.toastr.success('User removed from blacklist'),
			error: (error) =>
				this.toastr.error(
					error.error || 'Failed to remove user from blacklist'
				),
		});
	}

	deleteUserAccount(username: string) {
		this.adminService.deleteUserAccount(username).subscribe({
			next: () => this.toastr.success('Account deleted successfully'),
			error: (error) =>
				this.toastr.error(
					error.error || 'Failed to delete user account'
				),
		});
	}
}
