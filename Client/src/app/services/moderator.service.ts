import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { RefractoryService } from './refractory.service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ModeratorService {

  readonly rootUrl = environment.apiUrl + '/moderator';

  constructor(private http: HttpClient, private toastr: ToastrService, private refService: RefractoryService) { }

  BlockRefractory(RefId: string) {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.get(this.rootUrl + "/block/refractory/" + RefId, {headers: tokenHeader, responseType: 'text'}).subscribe
    (
      (res: any) => {
        this.toastr.success('Данi по вогнетриву заблоковано');
        this.refService.GetAllRefractories();
       },
      err => {
        this.toastr.error('HTTP status code', err.status);
      },
    );
  }

  UnblockRefractory(RefId: string) {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.get(this.rootUrl + "/unblock/refractory/" + RefId, {headers: tokenHeader, responseType: 'text'}).subscribe
    (
      (res: any) => {
        this.toastr.success('Данi по вогнетриву разблоковано');
        this.refService.GetAllRefractories();
       },
      err => {
        this.toastr.error('HTTP status code', err.status);
      },
    );
  }
}