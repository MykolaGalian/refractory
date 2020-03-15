import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { EditRefractory } from '../model/refractory/editRefractory';

import { NgForm } from '@angular/forms';
import { Refractory } from '../model/refractory/refractory';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RefractoryService {

  private postOnEdit:boolean=false;

  tegslist: string[] =null;
  tempRefractoryId: number = 0;
  refractoryByTeg:  Refractory[] = null;
  moderRefractories:  Refractory[] = null;
  refractories: Refractory[] = null;
  refractory: Refractory =null;
  readonly rootUrl = 'https://localhost:44302/api/refractory';
  refractoryForGetId: Refractory = null;
  bodyForNewRefractory: string = null;

  constructor(private http: HttpClient, private router: Router) { }

  GetRefractories() {
       var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
       return this.http.get(this.rootUrl + '/getallrefractories', {headers:reqHeader}).

       subscribe((res: any) => {
           this.refractories = res as Refractory[];

           for (let i = 0; i < this.refractories.length; i++) {
           this.refractories[i].Src = this.rootUrl +'/image/get?imageName=' + this.refractories[i].RefractoryPicture +'&userLogin=' + this.refractories[i].UserInfo.Login;
          }
         })
       }

  GetRefractoryById(RefId: number) {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
      return this.http.get(this.rootUrl + '/' +RefId, {headers:tokenHeader}).

      subscribe((res: any) => {
        this.refractory = res as Refractory;
        this.refractory.Src = this.rootUrl +'/image/get?imageName=' + this.refractory.RefractoryPicture +'&userLogin=' + this.refractory.UserInfo.Login;
     })
   }

  GetAllRefractories(){
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
       return this.http.get(this.rootUrl + '/getallrefractoriesmoder', {headers:tokenHeader}).

       subscribe((res: any) => {
           this.moderRefractories = res as Refractory[];
           for (let i = 0; i < this.moderRefractories.length; i++) {
           this.moderRefractories[i].Src = this.rootUrl +'/image/get?imageName=' + this.moderRefractories[i].RefractoryPicture +'&userLogin=' + this.moderRefractories[i].UserInfo.Login;
          }
       })
  }

   AddRefractory(data: any, fileToUpload: File)  {
      const formData: FormData = new FormData();
      formData.append('RefPicture', fileToUpload, fileToUpload.name);
      this.bodyForNewRefractory = data.value.body;
      formData.append("Brand", data.value.title);
      formData.append("Type", data.value.hashtags);

    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.post(this.rootUrl+'/add', formData, {headers: tokenHeader});
  }

  EditRefractory(postId: number, form: NgForm) {
    const body: EditRefractory = {
      RefractoryDescription: form.value.PostBody, //PostBody - Body
      RefractoryBrand: form.value.PostTitle,
      RefractoryType: form.value.Hashtags
    }
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.put(this.rootUrl + '/'+postId , body, {headers: tokenHeader});
  }

  //for new refractory add text body, after send picture and get post Id
  AddBodyForRefractory(post: Refractory) {
    const body: EditRefractory = {
      RefractoryDescription: this.bodyForNewRefractory, 
      RefractoryBrand: post.RefractoryBrand,
      RefractoryType: post.RefractoryType
    }
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.put(this.rootUrl + '/'+post.Id , body, {headers: tokenHeader}).
    subscribe((res: any) => {
      this.router.navigate(['/profile']);
    });
  }


  GetRefractoryByTitle(data: any){
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });

    const body: EditRefractory = {
      RefractoryDescription: data.value.body,
      RefractoryBrand: data.value.title, // send title
      RefractoryType: data.value.hashtags
    }

       return this.http.post(this.rootUrl + '/getrefbybrand', body , {headers:reqHeader}).

       subscribe((res: any) => {
           this.refractoryForGetId = res as Refractory;   // getting Refractory and RefId from it
           this.AddBodyForRefractory(this.refractoryForGetId)
  });
}



  DeleteRefractory(Id: number) {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.delete(this.rootUrl + '/' + Id, {headers: tokenHeader} );
  }

  GetTegs(){
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
       return this.http.get(this.rootUrl + '/getalltypes', {headers:reqHeader}).

       subscribe((res: any) => {
           this.tegslist = res as string[];
  });
 }
  SearchByTeg(type: string){
    const formData: FormData = new FormData();
    formData.append("Type", type);
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});

    return this.http.post(this.rootUrl + '/getrefractoriesbytype',formData, {headers: tokenHeader} ).
            subscribe((res: any) => {
              this.refractoryByTeg = res as Refractory[];
              for (let i = 0; i < this.refractoryByTeg.length; i++) {
                this.refractoryByTeg[i].Src = this.rootUrl +'/image/get?imageName=' + this.refractoryByTeg[i].RefractoryPicture +'&userLogin=' + this.refractoryByTeg[i].UserInfo.Login;
              }
              this.router.navigateByUrl('/ref-byteg');
        });
  }



}


