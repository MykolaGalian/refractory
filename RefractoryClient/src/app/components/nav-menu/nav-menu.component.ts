import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-nav-menu',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  private userService = inject(UserService);
  private router = inject(Router);

  get userDetails() {
    return this.userService.userDetails;
  }

  get isLoggedIn(): boolean {
    return localStorage.getItem('access_token') !== null;
  }

  ngOnInit(): void {
  }

  onLogout(): void {
    this.userService.LogOut().subscribe({
      next: () => {
        localStorage.removeItem('access_token');
        this.router.navigate(['/login']);
      },
      error: (error) => {
        localStorage.removeItem('access_token');
        this.router.navigate(['/login']);
      }
    });
  }
}