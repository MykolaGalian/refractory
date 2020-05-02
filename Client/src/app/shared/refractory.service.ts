import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { EditRefractory } from '../model/refractory/editRefractory';

import { NgForm } from '@angular/forms';
import { Refractory } from '../model/refractory/refractory';
import { Router } from '@angular/router';
import { Refcalc } from '../model/refractory/refcalc';
import { RefcalcResult } from '../model/refractory/refcalcResult';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class RefractoryService {

  refcalc: Refcalc =null;
  refCalcRes: RefcalcResult = null;

  tegslist: string[] =null;
  tempRefractoryId: number = 0;
  refractoryByTeg:  Refractory[] = null;
  moderRefractories:  Refractory[] = null;
  refractories: Refractory[] = null;
  refractory: Refractory =null;
  readonly rootUrl = 'https://localhost:44302/api/refractory';
  refractoryForGetId: Refractory = null;
  bodyForNewRefractory: string = null;

  constructor(private http: HttpClient, private router: Router, private toastr: ToastrService) { }

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

  //первый шаг отправка картинки и Brand и Type (и все кроме описания), а description отправляем вторым шагом 
   AddRefractory(data: any, fileToUpload: File)  {
      const formData: FormData = new FormData();
      formData.append('RefPicture', fileToUpload, fileToUpload.name);

      this.bodyForNewRefractory = data.value.Description; // на первом шаге не отправляем
      formData.append("Type", data.value.Type);
      formData.append("Brand", data.value.Brand);

      formData.append("Density", data.value.Density);
      formData.append("MaxWorkTemperature", data.value.MaxWorkTemperature);
      formData.append("Lime", data.value.Lime);
      formData.append("Alumina", data.value.Alumina);
      formData.append("Silica", data.value.Silica);
      formData.append("Magnesia", data.value.Magnesia);
      formData.append("Carbon", data.value.Carbon);
      formData.append("Price", data.value.Price);


    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.post(this.rootUrl+'/add', formData, {headers: tokenHeader});
  }

  EditRefractory(refId: number, form: NgForm) { 
     
    const body: EditRefractory = {
      RefractoryDescription: form.value.Description, 
      RefractoryBrand: form.value.Brand,
      RefractoryType: form.value.Type,

      Density: form.value.Density,
      MaxWorkTemperature: form.value.MaxWorkTemperature,
      Lime: form.value.Lime,
      Alumina: form.value.Alumina,
      Silica: form.value.Silica,
      Magnesia: form.value.Magnesia,
      Carbon: form.value.Carbon,
      Price: form.value.Price
    }
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.put(this.rootUrl + '/'+refId , body, {headers: tokenHeader});
  }

  //for new refractory add text body, after send picture and get ref Id !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
  AddBodyForRefractory(ref: Refractory) {
    const body: EditRefractory = {
      RefractoryDescription: this.bodyForNewRefractory, 
      RefractoryBrand: ref.RefractoryBrand,
      RefractoryType: ref.RefractoryType,

      Density: ref.Density,
      MaxWorkTemperature: ref.MaxWorkTemperature,
      Lime: ref.Lime,
      Alumina: ref.Alumina,
      Silica: ref.Silica,
      Magnesia: ref.Magnesia,
      Carbon: ref.Carbon,
      Price: ref.Price
    }

    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.put(this.rootUrl + '/'+ ref.Id , body, {headers: tokenHeader}).
    subscribe((res: any) => {
      this.toastr.info('Вогнетрив додано');
      this.router.navigate(['/profile']);
    });
  }

  //title->brand (Маркування)
  GetRefractoryByBrand(form: any){
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
 
    const body: EditRefractory = {
      RefractoryDescription: form.value.Description,
      RefractoryBrand: form.value.Brand, 
      RefractoryType: form.value.Type,

      Density: form.value.Density,
      MaxWorkTemperature: form.value.MaxWorkTemperature,
      Lime: form.value.Lime,
      Alumina: form.value.Alumina,
      Silica: form.value.Silica,
      Magnesia: form.value.Magnesia,
      Carbon: form.value.Carbon,
      Price: form.value.Price
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

  GetRefrCalc(data: Refcalc){
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
 
    const body: Refcalc = data;
    
       return this.http.post(this.rootUrl + '/CalcRefRow', body , {headers:reqHeader}).

       subscribe((res: any) => {
           this.refCalcRes = res as RefcalcResult;  
            
           if(this.refCalcRes === null) {
            this.toastr.error('З даних виробів неможливо викласти ковш');
           }
           console.log(this.refCalcRes);
  });
}
}


