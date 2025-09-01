import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EditRefractory } from '../models/refractory.models';
import { NgForm } from '@angular/forms';
import { Refractory, Refcalc, RefcalcResult } from '../models/refractory.models';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RefractoryService {

  refcalc: Refcalc | null = null;
  refCalcRes: RefcalcResult | null = null;

  tegslist: string[] | null = null;
  tempRefractoryId: number = 0;
  refractoryByTeg: Refractory[] | null = null;
  moderRefractories: Refractory[] | null = null;
  refractories: Refractory[] | null = null;
  refractory: Refractory | null = null;
  readonly rootUrl = environment.apiUrl + '/refractory';
  refractoryForGetId: Refractory | null = null;
  bodyForNewRefractory: string | null = null;

  constructor(private http: HttpClient, private router: Router, private toastr: ToastrService) { }

  GetRefractories() {
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
    return this.http.get(this.rootUrl + '/getallrefractories', {headers: reqHeader}).
    subscribe((res: any) => {
      this.refractories = res as Refractory[];

      for (let i = 0; i < this.refractories.length; i++) {
        this.refractories[i].Src = this.rootUrl + '/image/get?imageName=' + this.refractories[i].RefractoryPicture + '&userLogin=' + this.refractories[i].UserInfo.Login;
      }
    })
  }

  GetRefractoryById(RefId: number) {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('access_token')});
    return this.http.get(this.rootUrl + '/' + RefId, {headers: tokenHeader}).
    subscribe((res: any) => {
      this.refractory = res as Refractory;
      this.refractory.Src = this.rootUrl + '/image/get?imageName=' + this.refractory.RefractoryPicture + '&userLogin=' + this.refractory.UserInfo.Login;
    })
  }

  GetAllRefractories() {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('access_token')});
    return this.http.get(this.rootUrl + '/getallrefractoriesmoder', {headers: tokenHeader}).
    subscribe((res: any) => {
      this.moderRefractories = res as Refractory[];
      for (let i = 0; i < this.moderRefractories.length; i++) {
        // Временно используем userLogin из localStorage для изображений модератора
        const userLogin = this.moderRefractories[i].UserInfo?.Login || localStorage.getItem('Login') || 'default';
        this.moderRefractories[i].Src = this.rootUrl + '/image/get?imageName=' + this.moderRefractories[i].RefractoryPicture + '&userLogin=' + userLogin;
      }
    })
  }

  // Первый шаг отправка картинки и данных (кроме описания), description отправляем вторым шагом
  AddRefractory(data: any, fileToUpload: File) {
    const formData: FormData = new FormData();
    formData.append('refPicture', fileToUpload, fileToUpload.name); // API ожидает 'refPicture'

    this.bodyForNewRefractory = data.value.Description; // на первом шаге не отправляем
    formData.append("type", data.value.Type);
    formData.append("brand", data.value.Brand);

    formData.append("density", data.value.Density);
    formData.append("maxWorkTemperature", data.value.MaxWorkTemperature);
    formData.append("lime", data.value.Lime);
    formData.append("alumina", data.value.Alumina);
    formData.append("silica", data.value.Silica);
    formData.append("magnesia", data.value.Magnesia);
    formData.append("carbon", data.value.Carbon);
    formData.append("price", data.value.Price);

    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('access_token')});
    return this.http.post(this.rootUrl + '/add', formData, {headers: tokenHeader});
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
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('access_token')});
    return this.http.put(this.rootUrl + '/' + refId, body, {headers: tokenHeader});
  }

  // Для нового огнеупора добавляем текстовое описание после отправки картинки и получения ID
  AddBodyForRefractory(ref: Refractory) {
    const body: EditRefractory = {
      RefractoryDescription: this.bodyForNewRefractory || '', 
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

    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('access_token')});
    return this.http.put(this.rootUrl + '/' + ref.Id, body, {headers: tokenHeader}).
    subscribe((res: any) => {
      this.toastr.info('Вогнетрив додано');
      this.router.navigate(['/profile']);
    });
  }

  // title->brand (Маркування)
  GetRefractoryByBrand(form: any) {
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
    
    return this.http.post(this.rootUrl + '/getrefbybrand', body, {headers: reqHeader}).
    subscribe((res: any) => {
      this.refractoryForGetId = res as Refractory;   // getting Refractory and RefId from it
      this.AddBodyForRefractory(this.refractoryForGetId)
    });
  }

  DeleteRefractory(Id: number) {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer ' + localStorage.getItem('access_token')});
    return this.http.delete(this.rootUrl + '/' + Id, {headers: tokenHeader});
  }

  GetTegs() {
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
    return this.http.get(this.rootUrl + '/getalltypes', {headers: reqHeader}).
    subscribe((res: any) => {
      this.tegslist = res as string[];
    });
  }

  SearchByTeg(type: string) {
    const formData: FormData = new FormData();
    formData.append("type", type); // API ожидает 'type', а не 'Type'
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' }); // Поменял на No-Auth как в старом клиенте

    return this.http.post(this.rootUrl + '/getrefractoriesbytype', formData, {headers: reqHeader}).
    subscribe((res: any) => {
      this.refractoryByTeg = res as Refractory[];
      for (let i = 0; i < this.refractoryByTeg.length; i++) {
        this.refractoryByTeg[i].Src = this.rootUrl + '/image/get?imageName=' + this.refractoryByTeg[i].RefractoryPicture + '&userLogin=' + this.refractoryByTeg[i].UserInfo.Login;
      }
      this.router.navigateByUrl('/ref-byteg');
    });
  }

  GetRefrCalc(data: Refcalc) {
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
 
    // Преобразуем строки в числа для API
    const body = {
      a1: parseFloat(data.a1),
      a2: parseFloat(data.a2),
      b1: parseFloat(data.b1),
      b2: parseFloat(data.b2),
      brickLength: parseFloat(data.BrickLength),
      topDiameter: parseFloat(data.TopDiameter),
      bottomDiameter: parseFloat(data.BottomDiameter),
      rowNumber: parseFloat(data.RowNumber),
      density: parseFloat(data.Density),
      price: parseFloat(data.Price)
    };

    return this.http.post(this.rootUrl + '/calcRefRow', body, {headers: reqHeader}).
    subscribe((res: any) => {
      // Преобразуем ответ обратно в строки для совместимости
      if (res) {
        this.refCalcRes = {TotalA: '', TotalB: '', TotalMass: '', TotalPrice: ''} as RefcalcResult;
        this.refCalcRes.TotalA = res.totalA.toString();
        this.refCalcRes.TotalB = res.totalB.toString();
        this.refCalcRes.TotalMass = res.totalMass.toString();
        this.refCalcRes.TotalPrice = res.totalPrice.toString();
      } else {
        this.refCalcRes = null;
      }
            
      if (this.refCalcRes === null) {
        this.toastr.error('З даних виробів неможливо викласти ковш');
      }
      console.log(this.refCalcRes);
    });
  }
}