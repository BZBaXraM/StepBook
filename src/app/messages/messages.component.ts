import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { MessageService } from '../services/message.service';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { FormsModule, NgModel } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { Message } from '../models/message.model';
import { RouterLink } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatTableModule } from '@angular/material/table';
import { PaginationModule, PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
    selector: 'app-messages',
    imports: [
        ButtonsModule,
        FormsModule,
        TimeagoModule,
        RouterLink,
        MatButtonToggleModule,
        MatIconModule,
        MatCardModule,
        MatTableModule,
        PaginationModule,
    ],
    templateUrl: './messages.component.html',
    styleUrl: './messages.component.css'
})
export class MessagesComponent implements OnInit {
	messageService = inject(MessageService);
	container = 'Inbox';
	pageNumber = 1;
	pageSize = 5;
	currentPage = 1;
	isOutbox = this.container === 'Outbox';

	ngOnInit(): void {
		this.getMessages();
	}

	getMessages() {
		this.messageService.getMessages(
			this.pageNumber,
			this.pageSize,
			this.container
		);
	}

	deleteMessage(id: number) {
		this.messageService.deleteMessage(id).subscribe({
			next: () => {
				this.messageService.paginatedResult.update((prev) => {
					if (prev && prev.items) {
						prev.items.splice(
							prev.items.findIndex((m) => m.id === id),
							1
						);
						return prev;
					}
					return prev;
				});
			},
		});
	}

	pageChanged(event: PageChangedEvent) {
		this.currentPage = event.page;
		this.pageNumber = event.page;
		this.getMessages();
	}

	getRoute(message: Message) {
		if (this.container === 'Outbox') {
			return `/members/${message.RecipientUsername}`;
		} else {
			return `/members/${message.SenderUsername}`;
		}
	}
}
