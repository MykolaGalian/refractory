import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { RefractoryService } from '../../services/refractory.service';
import { Refractory } from '../../models/refractory.models';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  private router = inject(Router);
  private userService = inject(UserService);
  private refractoryService = inject(RefractoryService);

  private fileToUpload: File | null = null;

  get service() {
    return this.userService;
  }

  ngOnInit(): void {
    this.userService.getUserProfile();
  }

  onLogout(): void {
    this.userService.LogOut().subscribe({
      next: (res: any) => {
        console.log("LoggedOut");
      },
      error: (err) => {
        console.log(err);
      }
    });

    localStorage.removeItem('access_token');
    localStorage.removeItem('Login');
    this.refractoryService.refractory = null;
    this.refractoryService.refractories = null;
    this.refractoryService.moderRefractories = null;
    this.userService.userDetails = null;
    this.userService.usersDetails = null;
    this.router.navigate(['/login']);
  }

  addPost(): void {
    this.router.navigateByUrl('/ref-add');
  }

  userManager(): void {
    this.router.navigateByUrl('/admin');
  }

  postModerator(): void {
    this.router.navigateByUrl('/moder');
  }

  onProfileEdit(): void {
    this.router.navigateByUrl('/profile-edit');
  }

  populateForms(pd: Refractory): void {
    this.refractoryService.refractory = Object.assign({}, pd);
    this.router.navigate(['/user-ref']);
  }

  handleFileInput(files: FileList | null): void {
    if (files && files.length > 0) {
      this.fileToUpload = files.item(0);
      const reader = new FileReader();
      reader.onload = (event: any) => {
        this.userService.imageUrl = event.target.result;
      };
      if (this.fileToUpload) {
        reader.readAsDataURL(this.fileToUpload);
      }
      
      if (this.fileToUpload !== null) {
        this.userService.UpdateAvatar(this.fileToUpload).subscribe();
      }
    }
  }
}