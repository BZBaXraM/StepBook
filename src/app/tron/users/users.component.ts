import { Component, inject, OnInit } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule } from '@angular/material/paginator';
import {NgIf} from "@angular/common";

@Component({
    selector: 'app-users',
    imports: [MatTableModule, MatCardModule, MatPaginatorModule, NgIf],
    templateUrl: './users.component.html',
    styleUrl: './users.component.css'
})
export class UsersComponent implements OnInit {
	adminService = inject(AdminService);

	ngOnInit() {
		this.getUsers();
	}

	getUsers() {
		this.adminService.getUsers();
	}
}
