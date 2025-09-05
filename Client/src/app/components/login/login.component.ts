import { Component, OnInit } from '@angular/core';
import { NgForm, FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  formModel = {
    Login: '',
    Password: ''
  };

  constructor(private service: UserService, private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
    if (localStorage.getItem('access_token') != null)
      this.router.navigate(['/profile']);
  }

  onSubmit(form: NgForm) {
    this.service.login(form.value).subscribe(
      (res: any) => {
        localStorage.setItem('access_token', res.access_token);
        localStorage.setItem('Login', form.value.Login);

        this.service.GetUser().subscribe(
          (res: any) => {
            this.router.navigateByUrl('/profile');
          },
          err => {
            if (err.status === 400) {
              localStorage.removeItem('access_token');
              localStorage.removeItem('Login');
              this.toastr.error('Вас заблоковано, зверніться до адміністратора');
              this.router.navigateByUrl('/login');
            }
          },
        );
      },
      err => {
        this.toastr.error('HTTP status code', err.status);
      }
    );
  }
}
