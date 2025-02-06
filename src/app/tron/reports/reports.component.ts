import { Component, inject, OnInit } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { DatePipe } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule } from '@angular/material/paginator';

@Component({
    selector: 'app-reports',
    imports: [MatTableModule, MatCardModule, DatePipe, MatPaginatorModule],
    templateUrl: './reports.component.html',
    styleUrl: './reports.component.css'
})
export class ReportsComponent implements OnInit {
	adminService = inject(AdminService);

	getReports() {
		this.adminService.getReports();
	}

	ngOnInit(): void {
		this.getReports();
	}
}
