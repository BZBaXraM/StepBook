import { Component, Inject, inject, signal } from '@angular/core';
import { ReportService } from '../services/report.service';
import { FormsModule } from '@angular/forms';
import { Report } from '../models/report.model';
import {
	MatDialogRef,
	MAT_DIALOG_DATA,
	MatDialogModule,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
    selector: 'app-report-content-dialog',
    imports: [FormsModule, MatDialogModule, MatFormFieldModule],
    templateUrl: './report-content-dialog.component.html',
    styleUrls: ['./report-content-dialog.component.css']
})
export class ReportContentDialogComponent {
	private reportService = inject(ReportService);
	dialogRef = inject(MatDialogRef<ReportContentDialogComponent>);
	report = signal<Report>({ Reason: '' });
	memberUsername: string;

	constructor(
		@Inject(MAT_DIALOG_DATA) public data: { member: { Username: string } }
	) {
		this.memberUsername = data.member.Username;
	}

	addReport() {
		if (this.memberUsername) {
			this.reportService
				.addReportToUser(this.memberUsername, this.report())
				.subscribe({
					next: () => {
						this.dialogRef.close(this.report());
					},
					error: (err) => {
						console.error('Error saving report:', err);
					},
				});
		} else {
			console.error('Member username is undefined.');
		}
	}

	cancel() {
		this.dialogRef.close();
	}
}
