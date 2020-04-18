import { Component, OnInit } from '@angular/core';
import {NgForm} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../shared/user.service';
import { Router } from '@angular/router';
import { UserDetails } from 'src/app/model/user/user-details';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  formModel = {
    Login: '',
    Password: ''};    

  constructor(private service: UserService, private toastr: ToastrService, private router: Router) { }

  ngOnInit() {
    if (localStorage.getItem('access_token') != null)
   
    this.router.navigate(['/profile']);
  }

  onSubmit(form: NgForm) {

    
    this.service.login(form.value).subscribe(
      (res: any) => {
        console.log(res);
        localStorage.setItem('access_token', res.access_token);//запись токена в localStorage браузера
        localStorage.setItem('Login', form.value.Login);    
        console.log(form.value.Login);           
        
           this.service.GetUser().subscribe(
          (res:any) => {
            console.log("res",res);
            this.router.navigateByUrl('/profile');
           },
          err => {            
            console.log(err);
            if(err.status === 400) {          
              localStorage.removeItem('access_token'); //удаление токена в localStorage браузера
              localStorage.removeItem('Login');     
               this.toastr.error('Вас заблоковано, зверніться до адміністратора' );
               this.router.navigateByUrl('/login');
            }
           }, 
        ); 
        
      },  
        err => {
            console.log("this errror",err);              
            this.toastr.error('HTTP status code', err.status);
      }
      
    );
  }

}
