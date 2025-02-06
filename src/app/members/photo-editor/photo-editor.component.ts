import { Component, inject, input, OnInit, output } from '@angular/core';
import { Member } from '../../models/member.model';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../services/account.service';
import { environment } from '../../../environments/environment';
import { MembersService } from '../../services/members.service';
import { Photo } from '../../models/photo.model';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatCardModule } from '@angular/material/card';
import { catchError, of } from 'rxjs';

@Component({
    selector: 'app-photo-editor',
    imports: [
        CommonModule,
        MatProgressBarModule,
        MatButtonModule,
        MatIconModule,
        MatTooltipModule,
        MatCardModule,
    ],
    templateUrl: './photo-editor.component.html',
    styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent implements OnInit {
	private accountService = inject(AccountService);
	private memberService = inject(MembersService);

	member = input.required<Member>();
	memberChange = output<Member>();

	baseUrl = environment.apiUrl;
	isUploading = false;
	uploadProgress = 0;
	selectedFile: File | null = null;

	ngOnInit(): void {}

	onFileSelected(event: any) {
		const file = event.target.files[0];
		if (file) {
			this.selectedFile = file;
			this.uploadPhoto();
		}
	}

	uploadPhoto() {
		if (!this.selectedFile) return;

		this.isUploading = true;
		this.uploadProgress = 0;

		const formData = new FormData();
		formData.append('file', this.selectedFile);

		this.memberService.uploadPhoto(formData).subscribe({
			next: (event: any) => {
				if (event.type === 1) {
					this.uploadProgress = Math.round(
						(100 * event.loaded) / event.total
					);
				}
				if (event.body) {
					const photo = event.body;
					const updatedMember = { ...this.member() };
					updatedMember.Photos.push(photo);
					this.memberChange.emit(updatedMember);
					this.isUploading = false;
					this.selectedFile = null;
				}
			},
			error: () => {
				this.isUploading = false;
				this.uploadProgress = 0;
			},
		});
	}

	setMainPhoto(photo: Photo) {
		console.log('Setting main photo:', photo);
		this.memberService
			.setMainPhoto(photo.Id)
			.pipe(
				catchError((error) => {
					console.error('Error setting main photo:', error);
					return of(null);
				})
			)
			.subscribe({
				next: (response) => {
					if (response) {
						const user = this.accountService.currentUser();
						if (user) {
							user.PhotoUrl = photo.Url;
							this.accountService.setCurrentUser(user);
						}
						const updatedMember = { ...this.member() };
						updatedMember.PhotoUrl = photo.Url;
						updatedMember.Photos.forEach((p) => {
							p.IsMain = p.Id === photo.Id;
						});
						this.memberChange.emit(updatedMember);
						console.log('Main photo set successfully');
					}
				},
			});
	}

	deletePhoto(photo: Photo) {
		this.memberService.deletePhoto(photo.Id).subscribe({
			next: () => {
				const updatedMember = { ...this.member() };
				updatedMember.Photos = updatedMember.Photos.filter(
					(p) => p.Id !== photo.Id
				);
				this.memberChange.emit(updatedMember);
			},
		});
	}
}
