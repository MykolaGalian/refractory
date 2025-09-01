import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AdminService } from '../../../services/admin.service';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-admin-commands',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-commands.component.html',
  styleUrls: ['./admin-commands.component.css']
})
export class AdminCommandsComponent implements OnInit {
  private router = inject(Router);
  private adminService = inject(AdminService);
  private userService = inject(UserService);

  get service() {
    return this.userService;
  }

  get adminSvc() {
    return this.adminService;
  }

  ngOnInit(): void {
  }

  regNewUser(): void {
    this.router.navigateByUrl('/register');
  }

  blockAccount(): void {
    this.adminService.BlockAccount();
  }

  unblockAccount(): void {
    this.adminService.UnblockAccount();
  }
}