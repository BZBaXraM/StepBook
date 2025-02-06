import {
	Component,
	ElementRef,
	inject,
	input,
	viewChild,
	OnInit,
} from '@angular/core';
import { NgForm } from '@angular/forms';
import { EmojiEvent } from '@ctrl/ngx-emoji-mart/ngx-emoji';
import { BucketService } from '../services/bucket.service';
import { MessageService } from '../services/message.service';
import { NgClass, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PickerComponent } from '@ctrl/ngx-emoji-mart';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
    selector: 'app-message-form',
    imports: [
        FormsModule,
        NgClass,
        NgIf,
        PickerComponent,
        MatFormFieldModule,
        MatInputModule,
        MatIconModule,
        MatButtonModule,
        MatTooltipModule,
    ],
    templateUrl: './message-form.component.html',
    styleUrl: './message-form.component.css'
})
export class MessageFormComponent implements OnInit {
	readonly messageForm = viewChild<NgForm>('messageForm');
	readonly scrollMe = viewChild<ElementRef<HTMLElement>>('scrollMe');
	messageService = inject(MessageService);
	bucketService = inject(BucketService);
	messageContent = '';
	showEmojiMenu = false;
	username = input.required<string>();
	private mediaRecorder: MediaRecorder | null = null;
	isRecording = false;
	audioChunks: Blob[] = [];

	ngOnInit() {
		this.restoreDraftMessage();
	}

	private restoreDraftMessage() {
		const savedDraftKey = `draftMessage_${this.username()}`;
		const savedMessage = localStorage.getItem(savedDraftKey);
		if (savedMessage) {
			this.messageContent = savedMessage;
		}
	}

	toggleEmojiMenu() {
		this.showEmojiMenu = !this.showEmojiMenu;
	}

	addEmoji(event: EmojiEvent) {
		if (event && event.emoji && event.emoji.native) {
			this.messageContent = this.messageContent || '';
			this.messageContent += event.emoji.native;
			this.saveDraftMessage();
			this.showEmojiMenu = false;
		}
	}

	saveDraftMessage() {
		if (this.username()) {
			const savedDraftKey = `draftMessage_${this.username()}`;
			if (this.messageContent.trim()) {
				localStorage.setItem(savedDraftKey, this.messageContent);
			} else {
				localStorage.removeItem(savedDraftKey);
			}
		}
	}

	sendMessage() {
		if (this.messageContent.trim()) {
			this.messageService
				.sendMessage(this.username(), this.messageContent)
				.then(() => {
					const savedDraftKey = `draftMessage_${this.username()}`;
					localStorage.removeItem(savedDraftKey);

					this.messageForm()?.resetForm();
					this.messageContent = '';
					this.scrollToBottom();
				});
		}
	}

	onMessageContentChange() {
		this.saveDraftMessage();
	}

	sendFile(file: File) {
		this.bucketService.addFile(file).subscribe((response: string) => {
			const fileUrl = response;
			this.messageService
				.sendMessage(this.username(), '', fileUrl)
				.then(() => {
					this.scrollToBottom();
				});
		});
	}

	onFileChange(event: Event) {
		const target = event.target as HTMLInputElement;
		if (target.files && target.files.length > 0) {
			const file = target.files[0];
			this.sendFile(file);
		}
	}

	getMessages() {
		this.messageService.getMessageThread(this.username());
	}

	private scrollToBottom() {
		const scrollMe = this.scrollMe();
		if (scrollMe) {
			scrollMe.nativeElement.scrollTop =
				scrollMe.nativeElement.scrollHeight;
		}
	}

	async startRecording() {
		try {
			const stream = await navigator.mediaDevices.getUserMedia({
				audio: true,
			});
			this.mediaRecorder = new MediaRecorder(stream);
			this.audioChunks = [];

			this.mediaRecorder.ondataavailable = (event: BlobEvent) => {
				if (event.data.size > 0) {
					this.audioChunks.push(event.data);
				}
			};

			this.mediaRecorder.onstop = async () => {
				const audioBlob = new Blob(this.audioChunks, {
					type: 'audio/webm',
				});
				const file = new File(
					[audioBlob],
					`audio-message-${Date.now()}.webm`,
					{
						type: 'audio/webm',
					}
				);
				await this.sendAudioMessage(file);
			};

			this.mediaRecorder.start();
			this.isRecording = true;
		} catch (error) {
			console.error('Error accessing microphone:', error);
		}
	}

	stopRecording() {
		if (this.mediaRecorder && this.isRecording) {
			this.mediaRecorder.stop();
			this.isRecording = false;
			const tracks = this.mediaRecorder.stream.getTracks();
			tracks.forEach((track: MediaStreamTrack) => track.stop());
		}
	}

	async sendAudioMessage(file: File) {
		this.bucketService.addFile(file).subscribe({
			next: (fileUrl) => {
				if (typeof fileUrl === 'string') {
					this.messageService
						.sendMessage(this.username(), '', fileUrl)
						.then(() => {
							this.messageContent = '';
							this.scrollToBottom();
						});
				}
			},
			error: (error) => console.error('Error uploading audio:', error),
		});
	}
}
