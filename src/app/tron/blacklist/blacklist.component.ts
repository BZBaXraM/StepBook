import { Component, inject, OnInit } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule } from '@angular/material/paginator';
import {NgIf} from "@angular/common";

@Component({
    selector: 'app-blacklist',
    imports: [MatTableModule, MatCardModule, MatPaginatorModule, NgIf],
    templateUrl: './blacklist.component.html',
    styleUrl: './blacklist.component.css'
})
export class BlacklistComponent implements OnInit {
	adminService = inject(AdminService);

	getBlackList() {
		this.adminService.getBlackList();
	}

	ngOnInit(): void {
		this.getBlackList();
	}
}
