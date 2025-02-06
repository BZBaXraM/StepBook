import {
	Component,
	inject,
	OnDestroy,
	OnInit,
	signal,
	viewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Member } from '../../models/member.model';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { MessageService } from '../../services/message.service';
import { PresenceService } from '../../services/presence.service';
import { AccountService } from '../../services/account.service';
import { HubConnectionState } from '@microsoft/signalr';
import { ReportService } from '../../services/report.service';
import { Report } from '../../models/report.model';
import { MatDialog } from '@angular/material/dialog';
import { ReportContentDialogComponent } from '../../report-content-dialog/report-content-dialog.component';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-member-detail',
    imports: [
        TabsModule,
        GalleryModule,
        TimeagoModule,
        DatePipe,
        MemberMessagesComponent,
    ],
    templateUrl: './member-detail.component.html',
    styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit, OnDestroy {
	readonly memberTabs = viewChild<TabsetComponent>('memberTabs');
	presenceService = inject(PresenceService);
	private route = inject(ActivatedRoute);
	private router = inject(Router);
	private messageService = inject(MessageService);
	private accountService = inject(AccountService);
	private reportService = inject(ReportService);
	private toast = inject(ToastrService);
	member: Member = {} as Member;
	images: GalleryItem[] = [];
	activeTab?: TabDirective;
	report = signal<Report>({ Reason: '' });
	private dialog = inject(MatDialog);

	openDialog() {
		const dialogRef = this.dialog.open(ReportContentDialogComponent, {
			data: { member: this.member },
		});

		dialogRef.afterClosed().subscribe((result) => {
			if (result) {
				this.toast.success('Report submitted successfully');
				this.router.navigateByUrl('/members/' + this.member.Username);
			}
		});
	}

	ngOnInit(): void {
		this.route.data.subscribe({
			next: (data) => {
				this.member = data['member'];
				this.member &&
					this.member.Photos?.map((photo) => {
						this.images.push(
							new ImageItem({
								src: photo.Url,
								thumb: photo.Url,
							})
						);
					});
			},
		});

		this.route.paramMap.subscribe({
			next: () => this.onRouteParamsChange(),
		});

		this.route.queryParams.subscribe({
			next: (params) => {
				params['tab'] && this.selectTab(params['tab']);
			},
		});
	}

	selectTab(heading: string) {
		const memberTabs = this.memberTabs();
		if (memberTabs) {
			const tab = memberTabs.tabs.find((t) => t.heading === heading);
			if (tab) tab.active = true;
		}
	}

	reportUser() {
		this.reportService
			.addReportToUser(this.member.Username, this.report())
			.subscribe(() => {
				this.router.navigateByUrl('/members/' + this.member.Username);
			});
	}

	async onRouteParamsChange() {
		const user = this.accountService.currentUser();
		if (!user) return;
		if (
			this.messageService.hubConnection?.state ===
				HubConnectionState.Connected &&
			this.activeTab?.heading === 'Messages'
		) {
			this.messageService.hubConnection?.stop().then(() => {
				this.messageService.createHubConnection(
					user,
					this.member.Username
				);
			});
		}
	}

	async onTabActivated(data: TabDirective) {
		this.activeTab = data;
		await this.router.navigate([], {
			relativeTo: this.route,
			queryParams: { tab: this.activeTab.heading },
			queryParamsHandling: 'merge',
		});
		if (this.activeTab.heading === 'Messages' && this.member) {
			const user = this.accountService.currentUser();
			if (!user) return;
			await this.messageService.createHubConnection(
				user,
				this.member.Username
			);
		} else {
			this.messageService.stopHubConnection();
		}
	}

	ngOnDestroy(): void {
		this.messageService.stopHubConnection();
	}
}
