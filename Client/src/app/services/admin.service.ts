import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserService } from './user.service';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  readonly rootUrl = environment.apiUrl + '/admin';

  constructor(private http: HttpClient, private service: UserService, private toastr: ToastrService) { }

  BlockAccount(): void {
    if (this.service.userDetails) {
      var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
      this.http.get(this.rootUrl + "/block/user/" + this.service.userDetails.Login, {headers: tokenHeader, responseType: 'text'}).
      subscribe(
        (res: any) => {
          this.toastr.success('Користувача заблоковано');
          this.service.getAllUserProfiles();
         },
        err => {
          this.toastr.error('HTTP status code', err.status);
        }
      );
    }
  }

  UnblockAccount(): void {
    if (this.service.userDetails) {
      var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
      this.http.get(this.rootUrl + "/unblock/user/" + this.service.userDetails.Login, {headers: tokenHeader, responseType: 'text'}).
      subscribe(
        (res: any) => {
          this.toastr.success('Користувач розблокован');
          this.service.getAllUserProfiles();
         },
        err => {
          this.toastr.error('HTTP status code', err.status);
        },
      );
    }
  }
}