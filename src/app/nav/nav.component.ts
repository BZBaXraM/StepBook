import {
	ChangeDetectorRef,
	Component,
	inject,
	OnDestroy,
	OnInit,
	HostListener,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { MessageService } from '../services/message.service';
import { PresenceService } from '../services/presence.service';
import { Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { HasRoleDirective } from '../directives/has-role.directive';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';

@Component({
	selector: 'app-nav',
	imports: [
		FormsModule,
		BsDropdownModule,
		MatButtonModule,
		RouterLink,
		RouterLinkActive,
		HasRoleDirective,
		MatToolbarModule,
		MatMenuModule,
		MatCardModule,
		MatDividerModule,
	],
	templateUrl: './nav.component.html',
	styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit, OnDestroy {
	title = 'StepBook';
	accountService = inject(AccountService);
	private router = inject(Router);
	private cdr = inject(ChangeDetectorRef);
	messageService = inject(MessageService);
	haveMessages = false;
	newMessagesCount = 0;
	notificationMessage: string = '';
	http = inject(HttpClient);
	private eventSubscription!: Subscription;
	presenceService = inject(PresenceService);
	isDropdownOpen = false;
	isMobile = false;
	isMenuOpen = false;

	@HostListener('window:resize', ['$event'])
	onResize() {
		this.checkScreenSize();
	}

	ngOnInit() {
		this.checkScreenSize();
		this.eventSubscription =
			this.presenceService.eventObservable$.subscribe(() => {
				this.getNewMessagesCount();
			});
		this.getNewMessagesCount();
		this.cdr.detectChanges();
	}

	ngOnDestroy() {
		if (this.eventSubscription) {
			this.eventSubscription.unsubscribe();
		}
	}

	getNewMessagesCount() {
		if (this.accountService.currentUser()) {
			this.messageService.getNewMessagesCount().subscribe((count) => {
				this.newMessagesCount = count;
				this.haveMessages = count > 0;
			});
		}
	}

	async logout() {
		this.messageService.stopHubConnection();
		this.accountService.logout();
		await this.router.navigateByUrl('/login');
	}

	toggleDropdown() {
		this.isDropdownOpen = !this.isDropdownOpen;
	}

	private checkScreenSize() {
		this.isMobile = window.innerWidth < 768; // 768px - breakpoint для мобильных устройств
	}

	toggleMenu() {
		this.isMenuOpen = !this.isMenuOpen;
	}
}
