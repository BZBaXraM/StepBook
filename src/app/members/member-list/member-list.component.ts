import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { MembersService } from '../../services/members.service';
import { MemberCardComponent } from '../member-card/member-card.component';
import { PageChangedEvent, PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { NgClass } from '@angular/common';

@Component({
    selector: 'app-member-list',
    imports: [
        MemberCardComponent,
        PaginationModule,
        FormsModule,
        NgClass,
        ButtonsModule,
    ],
    templateUrl: './member-list.component.html',
    styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit, OnDestroy {
	memberService = inject(MembersService);
	genderList = [
		{ value: 'Male', display: 'Males' },
		{ value: 'Female', display: 'Females' },
	];

	ngOnInit(): void {
		if (!this.memberService.paginatedResult()) {
			this.getMembers();
		}
	}

	getMembers() {
		this.memberService.getMembers();
	}

	resetFilters() {
		this.memberService.resetUserParams();
		this.getMembers();
	}

	pageChanged(event: PageChangedEvent) {
		if (this.memberService.userParams().PageNumber !== event.page) {
			this.memberService.userParams().PageNumber = event.page;
			this.getMembers();
		}
	}

	ngOnDestroy(): void {
		this.memberService.paginatedResult.set(null);
	}
}
