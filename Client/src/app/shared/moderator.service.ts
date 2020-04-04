import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders} from '@angular/common/http'
import { ToastrService } from 'ngx-toastr';
import { RefractoryService } from './refractory.service';

@Injectable({
  providedIn: 'root'
})
export class ModeratorService {

  readonly rootUrl = 'https://localhost:44302/api/moder';

  constructor(private http: HttpClient, private toastr: ToastrService, private refService: RefractoryService) { }


  BlockRefractory(RefId: string) {
    console.log(RefId + " :refractory Id");

    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.get(this.rootUrl + "/block/refractory/" + RefId, {headers: tokenHeader}).subscribe
    (
      (res:any) => {
        this.toastr.success('Данi по вогнетриву заблоковано');
        this.refService.GetAllRefractories();
        console.log("updated");
       },
      err => {
        console.log(err);
        this.toastr.error('HTTP status code', err.status);
      },
    );

  }

  UnblockRefractory(RefId: string) {

    console.log(RefId + " :refractory Id");
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.get(this.rootUrl + "/unblock/refractory/" + RefId, {headers: tokenHeader}).subscribe
    (
      (res:any) => {
        this.toastr.success('Данi по вогнетриву разблоковано');
        this.refService.GetAllRefractories();
        console.log("updated");
       },
      err => {
        console.log(err);
        this.toastr.error('HTTP status code', err.status);
      },
    );



  }
}
