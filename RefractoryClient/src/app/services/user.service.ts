import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserDetails, ProfileViewModel, RegisterModel } from '../models/user.models';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private fb: FormBuilder, private http: HttpClient, private toastr: ToastrService) {
    this.formModel = this.fb.group({
      Login: ['', Validators.required],
      Email: ['', Validators.email],
      Name: [''],
      LastName: [''],
      Position: [''],
      Passwords: this.fb.group({
        Password: ['', [Validators.required, Validators.minLength(6)]],
        ConfirmPassword: ['', Validators.required]
      }, { validator: this.comparePasswords })
    });
  }

  editProfile: UserDetails | null = null;
  userDetails: UserDetails | null = null;
  usersDetails: UserDetails[] | null = null;  

  readonly BaseURI = environment.apiUrl.replace('/api', ''); // https://localhost:7128
  imageUrl: any = "/assets/img/unknown-user.png";
  readonly rootUrl = environment.apiUrl + '/'; // https://localhost:7128/api/

  formModel: any;

  comparePasswords(fb: FormGroup) {
    let confirmPswrdCtrl = fb.get('ConfirmPassword');
    let passwordCtrl = fb.get('Password');

    if (confirmPswrdCtrl && passwordCtrl && (confirmPswrdCtrl.errors == null || 'passwordMismatch' in confirmPswrdCtrl.errors)) {
      if (passwordCtrl.value != confirmPswrdCtrl.value)
        confirmPswrdCtrl.setErrors({ passwordMismatch: true });
      else
        confirmPswrdCtrl.setErrors(null);
    }
  }

  register() {
    var body = {
      Login: this.formModel.value.Login,
      Email: this.formModel.value.Email,
      FirstName: this.formModel.value.Name,  // API ожидает FirstName
      LastName: this.formModel.value.LastName,
      Password: this.formModel.value.Passwords.Password,
      ConfirmPassword: this.formModel.value.Passwords.ConfirmPassword
    };
    
    localStorage.removeItem('access_token');
    return this.http.post(this.rootUrl + 'accounts/Register', body);
  }

  login(formData: any) {
    var body = "username=" + formData.Login + "&password=" + formData.Password + "&grant_type=password";
    localStorage.removeItem('access_token');
    var reqHeader = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded', 'No-Auth': 'True'});
    return this.http.post(this.BaseURI + '/token', body, { headers: reqHeader });
  }

  getUserProfile() {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('access_token')});
    return this.http.get<ProfileViewModel>(this.rootUrl + 'users/profile', {headers: tokenHeader}).pipe(
      map(profile => this.convertProfileToUserDetails(profile))
    ).subscribe(
      (res: UserDetails) => {
        this.userDetails = res;
        // Сохраняем логин для использования в других сервисах
        localStorage.setItem('Login', this.userDetails.Login);
        if (this.userDetails.UserAvatar !== null) {
          this.imageUrl = this.rootUrl + 'users/image/get?imageName=' + this.userDetails.UserAvatar + '&userLogin=' + this.userDetails.Login;
        }
        if (this.userDetails.Refractories != null)
          for (let i of this.userDetails.Refractories) {
            i.Src = this.rootUrl + 'refractory/image/get?imageName=' + i.RefractoryPicture + '&userLogin=' + this.userDetails.Login;
          }
       },
      err => {
        console.log(err);
      },
    );
  }

  // Преобразование ProfileViewModel из API в UserDetails для совместимости
  private convertProfileToUserDetails(profile: ProfileViewModel): UserDetails {
    const userDetails = {} as UserDetails;
    userDetails.Id = profile.id;
    userDetails.Login = profile.login;
    userDetails.Name = profile.name;
    userDetails.LastName = profile.lastName;
    userDetails.Position = profile.position;
    userDetails.UserAvatar = profile.userAvatar;
    userDetails.Email = profile.email;
    userDetails.DateRegistration = profile.dateRegistration || new Date();
    userDetails.IsAdmin = profile.isAdmin;
    userDetails.IsModerator = profile.isModerator;
    userDetails.IsBlocked = profile.isBlocked;
    userDetails.Refractories = profile.refractories || [];
    return userDetails;
  }

  getAllUserProfiles() {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('access_token')});
    return this.http.get<ProfileViewModel[]>(this.rootUrl + 'users/allprofiles', {headers: tokenHeader}).pipe(
      map(profiles => profiles.map(p => this.convertProfileToUserDetails(p)))
    ).toPromise()
    .then(res => this.usersDetails = res as UserDetails[]);
  }

  GetUser(): Observable<UserDetails> {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('access_token')});
    return this.http.get<ProfileViewModel>(this.rootUrl + 'users/profile', {headers: tokenHeader}).pipe(
      map(profile => this.convertProfileToUserDetails(profile))
    );
  }

  UpdateAvatar(fileToUpload: File) {
    const endpoint = this.rootUrl + 'users/avatar/set';
    const formData: FormData = new FormData();
    formData.append('image', fileToUpload, fileToUpload.name); // API ожидает 'image', не 'Image'

    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('access_token')});
    return this.http.post(endpoint, formData, { headers: tokenHeader });
  }

  EditUserProfile() {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('access_token')});
    // Преобразуем в формат, ожидаемый новым API
    const body = {
      id: this.editProfile?.Id || 0,
      firstName: this.editProfile?.Name || '',
      lastName: this.editProfile?.LastName || '',
      email: this.editProfile?.Email || '',
      position: this.editProfile?.Position || ''
    };
    return this.http.put(this.rootUrl + 'users/profile/edit', body, { headers: tokenHeader });
  }

  DeleteAccount() {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('access_token')});
    return this.http.delete(this.rootUrl + 'users/profile/edit', { headers: tokenHeader });
  }

  LogOut() {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('access_token')});
    return this.http.get(this.rootUrl + 'accounts/Logout', { headers: tokenHeader });
  }
}