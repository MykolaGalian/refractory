import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {
  private router = inject(Router);
  private userService = inject(UserService);

  get service() {
    return this.userService;
  }

  ngOnInit(): void {
    console.log(this.userService.userDetails?.Id);
    console.log(this.userService.userDetails?.Login);
    this.initEditProfile();
  }

  initEditProfile(): void {
    if (this.userService.userDetails) {
      this.userService.editProfile = {
        Id: this.userService.userDetails.Id,
        Login: this.userService.userDetails.Login,
        Name: this.userService.userDetails.Name,
        LastName: this.userService.userDetails.LastName,
        Email: this.userService.userDetails.Email,
        Position: this.userService.userDetails.Position,
        UserAvatar: this.userService.userDetails.UserAvatar,
        DateRegistration: this.userService.userDetails.DateRegistration,
        Refractories: this.userService.userDetails.Refractories,
        IsAdmin: this.userService.userDetails.IsAdmin,
        IsModerator: this.userService.userDetails.IsModerator,
        IsBlocked: this.userService.userDetails.IsBlocked
      };
    }
  }

  OnSubmit(): void {
    this.userService.EditUserProfile().subscribe({
      next: (res: any) => {
        this.userService.getUserProfile();
        alert('Профіль користувача оновлено');
        console.log("updated");
      },
      error: (err) => {
        console.log(err);
        alert(`HTTP status code: ${err.status}`);
      }
    });
  }

  OnDeleteAcc(): void {
    if (confirm('Ви впевнені, що хочете видалити акаунт?')) {
      this.userService.DeleteAccount().subscribe({
        next: (res: any) => {
          alert('Профіль користувача видалено');
          console.log("deleted");
          localStorage.removeItem('access_token');
          localStorage.removeItem('Login');
          this.router.navigate(['/login']);
        },
        error: (err) => {
          console.log(err);
          alert(`HTTP status code: ${err.status}`);
        }
      });
    }
  }
}