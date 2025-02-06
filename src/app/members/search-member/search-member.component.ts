import { Component, inject, signal } from '@angular/core';
import { MembersService } from '../../services/members.service';
import { Member } from '../../models/member.model';
import { MemberCardComponent } from '../member-card/member-card.component';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NgFor } from '@angular/common';

@Component({
	selector: 'app-search-member',
	imports: [MemberCardComponent, FormsModule, NgFor],
	templateUrl: './search-member.component.html',
	styleUrl: './search-member.component.css',
})
export class SearchMemberComponent {
	private memberService = inject(MembersService);
	private router = inject(Router);

	userName = signal<string>('');
	members = signal<Member[]>([]);
	isSearching = signal<boolean>(false);

	search() {
		if (!this.userName()) return;

		this.isSearching.set(true);
		this.memberService.getMember(this.userName()).subscribe({
			next: (member) => {
				this.members.set([member]);
				this.isSearching.set(false);
			},
			error: () => {
				this.members.set([]);
				this.isSearching.set(false);
			},
		});
	}

	reset() {
		this.userName.set('');
		this.members.set([]);
	}

	navigateToMember(username: string) {
		this.router.navigate(['/members', username]);
	}
}
