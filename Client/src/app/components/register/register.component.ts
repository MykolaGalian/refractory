import { UserService } from '../../shared/user.service';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';



@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(public service: UserService, public toastr: ToastrService,  private router: Router) { }  


  ngOnInit() {
    this.service.formModel.reset();
    
  }

  onSubmit() {
    this.service.register().subscribe(
      (res: any) => {
        if (res !== null) {
          this.service.formModel.reset();
          console.log(res); 
          this.toastr.success('Створено нового користувача.');
          this.router.navigateByUrl('/login');
        }         
      },
      err => {
        console.log(err);              
        this.toastr.error('HTTP status code', err.status);
      }
    );
  }
}
