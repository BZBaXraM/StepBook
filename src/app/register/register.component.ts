import { Component, OnInit, inject, output } from '@angular/core';
import {
	AbstractControl,
	FormControl,
	FormGroup,
	ReactiveFormsModule,
	ValidatorFn,
	Validators,
} from '@angular/forms';
import { AccountService } from '../services/account.service';
import { Router } from '@angular/router';
import { MatRadioModule } from '@angular/material/radio';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ToastrService } from 'ngx-toastr';
import { RequestConfirmCodeDialogComponent } from '../request-confirm-code-dialog/request-confirm-code-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrl: './register.component.css',
    imports: [
        ReactiveFormsModule,
        MatRadioModule,
        MatFormFieldModule,
        MatInputModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatIconModule,
        MatButtonModule,
    ]
})
export class RegisterComponent implements OnInit {
	private accountService = inject(AccountService);
	private router = inject(Router);
	private toast = inject(ToastrService);
	cancelRegister = output<boolean>();
	registerForm: FormGroup = new FormGroup({
		Gender: new FormControl('Male'),
		Email: new FormControl('', [Validators.email, Validators.required]),
		Username: new FormControl('', Validators.required),
		FirstName: new FormControl('', Validators.required),
		LastName: new FormControl('', Validators.required),
		DateOfBirth: new FormControl('', Validators.required),
		City: new FormControl('', Validators.required),
		Country: new FormControl('', Validators.required),
		Password: new FormControl('', [
			Validators.required,
			Validators.minLength(8),
			Validators.maxLength(30),
		]),
		ConfirmPassword: new FormControl('', [
			Validators.required,
			this.matchValues('Password'),
		]),
	});
	maxDate = new Date();
	validationErrors: string[] | undefined = [];
	passwordFieldType = 'password';
	private dialog = inject(MatDialog);

	ngOnInit(): void {
		this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);

		const savedFormData = localStorage.getItem('registerFormData');
		if (savedFormData) {
			const formData = JSON.parse(savedFormData);
			delete formData.Password;
			delete formData.ConfirmPassword;
			this.registerForm.patchValue(formData);
		}

		this.registerForm.valueChanges.subscribe((formData) => {
			const dataToSave = { ...formData };
			delete dataToSave.Password;
			delete dataToSave.ConfirmPassword;
			localStorage.setItem(
				'registerFormData',
				JSON.stringify(dataToSave)
			);
		});
	}

	matchValues(matchTo: string): ValidatorFn {
		return (control: AbstractControl) => {
			return control.value === control.parent?.get(matchTo)?.value
				? null
				: { isMatching: true };
		};
	}

	async register() {
		const dob = this.getDateOnly(
			this.registerForm.get('DateOfBirth')?.value
		);
		this.registerForm.patchValue({ DateOfBirth: dob });
		this.accountService.register(this.registerForm.value).subscribe({
			next: (_) => {
				localStorage.removeItem('registerFormData');
				localStorage.setItem(
					'registerEmail',
					this.registerForm.get('email')?.value
				);
				this.dialog.open(RequestConfirmCodeDialogComponent, {
					data: { email: this.registerForm.get('email')?.value },
				});
			},
			error: (error) => {
				this.validationErrors = error;
				this.toast.error(this.validationErrors!.toString());
			},
		});
	}

	cancel() {
		this.cancelRegister.emit(false);
	}

	private getDateOnly(dob: string) {
		if (!dob) return;
		return new Date(dob).toISOString().slice(0, 10);
	}

	togglePasswordVisibility() {
		this.passwordFieldType =
			this.passwordFieldType === 'password' ? 'text' : 'password';
	}
}
