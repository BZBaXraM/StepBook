import { Component, computed, inject, input } from '@angular/core';
import { Member } from '../../models/member.model';
import { RouterLink } from '@angular/router';
import { LikesService } from '../../services/likes.service';
import { PresenceService } from '../../services/presence.service';

@Component({
    selector: 'app-member-card',
    imports: [RouterLink],
    templateUrl: './member-card.component.html',
    styleUrl: './member-card.component.css'
})
export class MemberCardComponent {
	private likeService = inject(LikesService);
	member = input.required<Member>();
	private presenceService = inject(PresenceService);
	hasLiked = computed(() =>
		this.likeService.likeIds().includes(this.member().Id)
	);
	isOnline = computed(() =>
		this.presenceService.onlineUsers().includes(this.member().Username)
	);

	toggleLike() {
		this.likeService.toggleLike(this.member().Id).subscribe({
			next: () => {
				if (this.hasLiked()) {
					this.likeService.likeIds.update((ids) =>
						ids.filter((id) => id !== this.member().Id)
					);
				} else {
					this.likeService.likeIds.update((ids) => [
						...ids,
						this.member().Id,
					]);
				}
			},
		});
	}
}
