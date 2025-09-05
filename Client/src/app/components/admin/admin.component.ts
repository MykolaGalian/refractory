import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminCommandsComponent } from './admin-commands/admin-commands.component';
import { UserListComponent } from './user-list/user-list.component';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, AdminCommandsComponent, UserListComponent],
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  ngOnInit(): void {
  }
}