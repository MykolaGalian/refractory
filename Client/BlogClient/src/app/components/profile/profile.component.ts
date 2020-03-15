import { Component, OnInit } from '@angular/core';
import { UserService } from '../../shared/user.service';
import { Router} from '@angular/router';
import { RefractoryService } from '../../shared/refractory.service';
import { Refractory } from '../../model/refractory/refractory';



@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  private fileToUpload: File = null;

  constructor(private router: Router, private service: UserService, private postService: RefractoryService) { }

  ngOnInit() {

    this.service.getUserProfile();

  }

  onLogout() {
    this.service.LogOut().subscribe(
      (res:any) => {
        console.log("LogedOut");
       },
      err => {
        console.log(err);
      },
    );

    localStorage.removeItem('access_token');
    localStorage.removeItem('Login');
    this.postService.refractory=null;
    this.postService.refractories=null;
    this.postService.moderRefractories=null;
    this.service.userDetails=null;
    this.service.usersDetails=null;
    this.router.navigate(['/login']);
  }

  addPost(){
    this.router.navigateByUrl('/blog-editor');
  }
  userManager(){
    this.router.navigateByUrl('/admin');
  }

  postModerator(){
    this.router.navigateByUrl('/moder');
  }

  onProfileEdit(){
    this.router.navigateByUrl('/profile-edit');
  }

  populateForms(pd: Refractory) {       // метод обновляет данные во временном обьекте (postService.post) типа Post на основании обьекта выделенного из списка pd в представлении

    this.postService.refractory = Object.assign({}, pd);   // Object.assign - предотвращает корректировку полей в  Object.assign({}, pd);
    this.router.navigate(['/user-post']);
  }

  handleFileInput(file: FileList) {
    this.fileToUpload = file.item(0);
    var reader = new FileReader();
    reader.onload = (event: any) => {
      this.service.imageUrl = event.target.result;
    };
    reader.readAsDataURL(this.fileToUpload);
    if (this.fileToUpload !== null) {
      this.service.UpdateAvatar(this.fileToUpload).subscribe();
    }
  }


}
