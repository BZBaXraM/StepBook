import { AfterContentInit, Component } from '@angular/core';

@Component({
    selector: 'app-admin',
    imports: [],
    templateUrl: './admin.component.html',
    styleUrl: './admin.component.css'
})
export class AdminComponent implements AfterContentInit {
	ngAfterContentInit(): void {
		window.location.href = 'https://www.youtube.com/watch?v=IbgbMcFvyOY';
	}
}
