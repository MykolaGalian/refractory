import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserDetails, ProfileViewModel, RegisterModel } from '../models/user.models';
import { Refractory } from '../models/refractory.models';
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
      Name: this.formModel.value.Name,  // API ожидает Name, не FirstName
      LastName: this.formModel.value.LastName,
      Position: this.formModel.value.Position || '',
      Password: this.formModel.value.Passwords.Password,
      ConfirmPassword: this.formModel.value.Passwords.ConfirmPassword
    };
    
    localStorage.removeItem('access_token');
    return this.http.post(this.rootUrl + 'accounts/Register', body, { responseType: 'text' });
  }

  login(formData: any) {
    var body = "username=" + formData.Login + "&password=" + formData.Password + "&grant_type=password";
    localStorage.removeItem('access_token');
    var reqHeader = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded', 'No-Auth': 'True'});
    return this.http.post(this.BaseURI + '/token', body, { headers: reqHeader });
  }

  getUserProfile() {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
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
            if (i.RefractoryPicture && this.userDetails?.Login) {
              i.Src = this.rootUrl + 'refractory/image/get?imageName=' + i.RefractoryPicture + '&userLogin=' + this.userDetails.Login;
            } else {
              i.Src = '';
            }
          }
       },
      err => {
      },
    );
  }

  // Маппинг refractory данных с API (camelCase) в Angular модель (PascalCase)
  private mapRefractoryFromApi(res: any): Refractory {
    return {
      Id: res.id,
      RefractoryDescription: res.refractoryDescription,
      RefractoryBrand: res.refractoryBrand,
      RefractoryPicture: res.refractoryPicture,
      UserInfoId: res.userInfoId,
      UserInfo: res.userInfo ? {
        Id: res.userInfo.id,
        Login: res.userInfo.login,
        Name: res.userInfo.name,
        LastName: res.userInfo.lastName,
        Position: res.userInfo.position,
        UserAvatar: res.userInfo.userAvatar,
        Email: res.userInfo.email,
        Refractories: res.userInfo.refractories || [],
        DateRegistration: res.userInfo.dateRegistration,
        IsAdmin: res.userInfo.isAdmin,
        IsModerator: res.userInfo.isModerator,
        IsBlocked: res.userInfo.isBlocked
      } : {
        Id: 0, Login: '', Name: '', LastName: '', Position: '', UserAvatar: '', 
        Email: '', Refractories: [], DateRegistration: new Date(), 
        IsAdmin: false, IsModerator: false, IsBlocked: false
      },
      DateCreate: res.dateCreate,
      LastEdit: res.lastEdit,
      Comments: res.comments ? res.comments.map((comment: any) => ({
        Id: comment.id,
        CommentBody: comment.commentBody,
        RefractoryId: comment.refractoryId,
        DateCreation: comment.dateCreation,
        UserInfoId: comment.userInfoId,
        UserInfo: comment.userInfo ? {
          Id: comment.userInfo.id,
          Login: comment.userInfo.login,
          Name: comment.userInfo.name,
          LastName: comment.userInfo.lastName,
          Position: comment.userInfo.position,
          UserAvatar: comment.userInfo.userAvatar,
          Email: comment.userInfo.email,
          Refractories: [],
          DateRegistration: comment.userInfo.dateRegistration,
          IsAdmin: comment.userInfo.isAdmin,
          IsModerator: comment.userInfo.isModerator,
          IsBlocked: comment.userInfo.isBlocked
        } : null
      })) : [],
      RefractoryType: res.refractoryType,
      IsBlocked: res.isBlocked,
      Density: res.density,
      MaxWorkTemperature: res.maxWorkTemperature,
      Lime: res.lime,
      Alumina: res.alumina,
      Silica: res.silica,
      Magnesia: res.magnesia,
      Carbon: res.carbon,
      Price: res.price,
      Src: '' // будет установлено позже
    };
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
    userDetails.Refractories = profile.refractories ? profile.refractories.map(ref => this.mapRefractoryFromApi(ref)) : [];
    return userDetails;
  }

  getAllUserProfiles() {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.get<ProfileViewModel[]>(this.rootUrl + 'users/allprofiles', {headers: tokenHeader}).pipe(
      map(profiles => profiles.map(p => this.convertProfileToUserDetails(p)))
    ).toPromise()
    .then(res => this.usersDetails = res as UserDetails[]);
  }

  GetUser(): Observable<UserDetails> {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.get<ProfileViewModel>(this.rootUrl + 'users/profile', {headers: tokenHeader}).pipe(
      map(profile => this.convertProfileToUserDetails(profile))
    );
  }

  UpdateAvatar(fileToUpload: File) {
    const endpoint = this.rootUrl + 'users/avatar/set';
    const formData: FormData = new FormData();
    formData.append('image', fileToUpload, fileToUpload.name); // API ожидает 'image', не 'Image'

    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.post(endpoint, formData, { headers: tokenHeader, responseType: 'text' });
  }

  EditUserProfile() {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    // Преобразуем в формат, ожидаемый API (ChangeProfileViewModel)
    const body = {
      Id: this.editProfile?.Id || 0,
      Login: this.editProfile?.Login || '',
      Name: this.editProfile?.Name || '',
      LastName: this.editProfile?.LastName || '',
      Email: this.editProfile?.Email || '',
      Position: this.editProfile?.Position || '',
      UserAvatar: this.editProfile?.UserAvatar || '',
      DateRegistration: this.editProfile?.DateRegistration || null,
      IsBlocked: this.editProfile?.IsBlocked || false
    };
    console.log('EditProfile body:', body);
    return this.http.put(this.rootUrl + 'users/profile/edit', body, { headers: tokenHeader, responseType: 'text' });
  }

  DeleteAccount() {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.delete(this.rootUrl + 'users/profile/edit', { headers: tokenHeader, responseType: 'text' });
  }

  LogOut() {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.get(this.rootUrl + 'accounts/Logout', { headers: tokenHeader, responseType: 'text' });
  }
}