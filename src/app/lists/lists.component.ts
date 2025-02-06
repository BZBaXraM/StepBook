import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { LikesService } from '../services/likes.service';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { FormsModule } from '@angular/forms';
import { MemberCardComponent } from '../members/member-card/member-card.component';
import { PageChangedEvent, PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
    selector: 'app-lists',
    imports: [
        ButtonsModule,
        FormsModule,
        MemberCardComponent,
        PaginationModule,
    ],
    templateUrl: './lists.component.html',
    styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit, OnDestroy {
	likeService = inject(LikesService);
	predicate = 'liked';
	pageNumber = 1;
	pageSize = 5;

	ngOnInit(): void {
		this.getLikes();
	}

	getTitle() {
		switch (this.predicate) {
			case 'liked':
				return 'Members you like';
			case 'likedBy':
				return 'Members who like you';
			default:
				return 'Mutual';
		}
	}

	getLikes() {
		this.likeService.getLikes(
			this.predicate,
			this.pageNumber,
			this.pageSize
		);
	}

	pageChanged(event: PageChangedEvent) {
		if (this.pageNumber !== event.page) {
			this.pageNumber = event.page;
			this.getLikes();
		}
	}

	ngOnDestroy(): void {
		this.likeService.paginatedResult.set(null);
	}
}
