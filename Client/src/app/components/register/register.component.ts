import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  private userService = inject(UserService);
  private router = inject(Router);

  get formModel(): FormGroup {
    return this.userService.formModel;
  }

  ngOnInit(): void {
    this.userService.formModel.reset();
  }

  onSubmit(): void {
    if (this.formModel.valid) {
      this.userService.register().subscribe({
        next: (res: any) => {
          if (res !== null) {
            this.userService.formModel.reset();
            alert('Створено нового користувача.');
            this.router.navigateByUrl('/login');
          }
        },
        error: (err) => {
          alert(`HTTP status code: ${err.status}`);
        }
      });
    }
  }
}