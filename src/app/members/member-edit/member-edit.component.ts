import {
	Component,
	HostListener,
	inject,
	OnInit,
	ViewChild,
} from '@angular/core';
import { Member } from '../../models/member.model';
import { AccountService } from '../../services/account.service';
import { MembersService } from '../../services/members.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PhotoEditorComponent } from '../photo-editor/photo-editor.component';
import { Router } from '@angular/router';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTabGroup } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
	selector: 'app-member-edit',
	imports: [
		TabsModule,
		FormsModule,
		PhotoEditorComponent,
		TimeagoModule,
		DatePipe,
		MatCardModule,
		MatIconModule,
		MatButtonModule,
		MatTabsModule,
		MatTabGroup,
		MatFormFieldModule,
		MatInputModule,
	],
	templateUrl: './member-edit.component.html',
	styleUrl: './member-edit.component.css',
})
export class MemberEditComponent implements OnInit {
	@ViewChild('editForm') editForm?: NgForm;
	router = inject(Router);
	member?: Member;
	private accountService = inject(AccountService);
	private memberService = inject(MembersService);
	private toastr = inject(ToastrService);

	@HostListener('window:beforeunload', ['$event'])
	notify($event: BeforeUnloadEvent) {
		if (this.editForm?.dirty) {
			$event.preventDefault();
		}
	}

	ngOnInit(): void {
		this.getMember();
		this.updateMember();
	}

	getMember() {
		const user = this.accountService.currentUser();
		// if (!user) return;
		this.memberService.getMember(user!.Username).subscribe({
			next: (member) => {
				this.member = member;
				console.log(member);
			},
		});
	}

	updateMember() {
		this.memberService.updateMember(this.editForm?.value).subscribe({
			next: (_) => {
				this.toastr.success('Profile updated successfully!');
				this.editForm?.reset(this.member);
			},
		});
	}

	// deleteAccount() {
	// 	this.accountService.deleteAccount().subscribe({
	// 		next: (_) => {
	// 			// this.toastr.success('Account deleted');
	// 			this.router.navigateByUrl('/login');
	// 			this.editForm?.reset(this.member);
	// 		},
	// 	});
	// }

	onMemberChange(event: Member) {
		this.member = event;
	}
}
