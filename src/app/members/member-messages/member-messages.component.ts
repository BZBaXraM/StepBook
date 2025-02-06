import { Component, input } from '@angular/core';
import { ChatComponent } from '../../chat/chat.component';
import { MessageFormComponent } from '../../message-form/message-form.component';
import { MatDivider } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';

@Component({
    selector: 'app-member-messages',
    imports: [
        ChatComponent,
        MessageFormComponent,
        MatDivider,
        MatExpansionModule,
    ],
    templateUrl: './member-messages.component.html',
    styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent {
	username = input.required<string>();
}
