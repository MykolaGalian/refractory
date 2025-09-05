import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EditRefractory } from '../models/refractory.models';
import { NgForm } from '@angular/forms';
import { Refractory, Refcalc, RefcalcResult } from '../models/refractory.models';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../environments/environment';
import { BehaviorSubject } from 'rxjs';

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
  
  private _refractory = new BehaviorSubject<Refractory | null>(null);
  public refractory$ = this._refractory.asObservable();
  
  get refractory(): Refractory | null {
    return this._refractory.value;
  }
  
  set refractory(value: Refractory | null) {
    this._refractory.next(value);
  }
  readonly rootUrl = environment.apiUrl + '/refractory';
  refractoryForGetId: Refractory | null = null;
  bodyForNewRefractory: string | null = null;

  constructor(private http: HttpClient, private router: Router, private toastr: ToastrService) { }

  // Маппинг данных с API (camelCase) в Angular модель (PascalCase)
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

  GetRefractories() {
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
    return this.http.get(this.rootUrl + '/getallrefractories', {headers: reqHeader}).
    subscribe((res: any) => {
      this.refractories = res.map((item: any) => this.mapRefractoryFromApi(item));

      if (this.refractories) {
        for (let i = 0; i < this.refractories.length; i++) {
          if (this.refractories[i].RefractoryPicture && this.refractories[i].UserInfo?.Login) {
            this.refractories[i].Src = this.rootUrl + '/image/get?imageName=' + this.refractories[i].RefractoryPicture + '&userLogin=' + this.refractories[i].UserInfo.Login;
          } else {
            this.refractories[i].Src = '';
          }
        }
      }
    })
  }

  GetRefractoryById(RefId: number) {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    this.http.get(this.rootUrl + '/' + RefId, {headers: tokenHeader}).
    subscribe({
      next: (res: any) => {
        this.refractory = this.mapRefractoryFromApi(res);
        
        if (this.refractory.RefractoryPicture && this.refractory.UserInfo?.Login) {
          this.refractory.Src = this.rootUrl + '/image/get?imageName=' + this.refractory.RefractoryPicture + '&userLogin=' + this.refractory.UserInfo.Login;
        } else {
          this.refractory.Src = '';
        }
      },
      error: (error) => {
        // API error handling can be added here if needed
      }
    })
  }

  GetAllRefractories() {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.get(this.rootUrl + '/getallrefractoriesmoder', {headers: tokenHeader}).
    subscribe((res: any) => {
      this.moderRefractories = res.map((item: any) => this.mapRefractoryFromApi(item));
      if (this.moderRefractories) {
        for (let i = 0; i < this.moderRefractories.length; i++) {
          const userLogin = this.moderRefractories[i].UserInfo?.Login || localStorage.getItem('Login') || 'default';
          if (this.moderRefractories[i].RefractoryPicture && userLogin) {
            this.moderRefractories[i].Src = this.rootUrl + '/image/get?imageName=' + this.moderRefractories[i].RefractoryPicture + '&userLogin=' + userLogin;
          } else {
            this.moderRefractories[i].Src = '';
          }
        }
      }
    })
  }

  // Отправка всех данных включая описание
  AddRefractory(data: any, fileToUpload: File, description: string) {
    console.log('Service - received description:', description);
    const formData: FormData = new FormData();
    formData.append('refPicture', fileToUpload, fileToUpload.name); // API ожидает 'refPicture'

    this.bodyForNewRefractory = description;
    formData.append("type", data.value.Type || "");
    formData.append("brand", data.value.Brand || "");
    formData.append("description", description || "");
    
    console.log('FormData description:', formData.get('description'));

    formData.append("density", data.value.Density || "0");
    formData.append("maxWorkTemperature", data.value.MaxWorkTemperature || "0");
    formData.append("lime", data.value.Lime || "0");
    formData.append("alumina", data.value.Alumina || "0");
    formData.append("silica", data.value.Silica || "0");
    formData.append("magnesia", data.value.Magnesia || "0");
    formData.append("carbon", data.value.Carbon || "0");
    formData.append("price", data.value.Price || "0");

    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.post(this.rootUrl + '/add', formData, {headers: tokenHeader, responseType: 'text'});
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
    return this.http.put(this.rootUrl + '/' + refId, body, {headers: tokenHeader, responseType: 'text'});
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

    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
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
      this.refractoryForGetId = this.mapRefractoryFromApi(res);
      this.AddBodyForRefractory(this.refractoryForGetId)
    });
  }

  DeleteRefractory(Id: number) {
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.delete(this.rootUrl + '/' + Id, {headers: tokenHeader, responseType: 'text'});
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
    formData.append("type", type);
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });

    return this.http.post(this.rootUrl + '/getrefractoriesbytype', formData, {headers: reqHeader}).
    subscribe((res: any) => {
      this.refractoryByTeg = res.map((item: any) => this.mapRefractoryFromApi(item));
      if (this.refractoryByTeg) {
        for (let i = 0; i < this.refractoryByTeg.length; i++) {
          if (this.refractoryByTeg[i].RefractoryPicture && this.refractoryByTeg[i].UserInfo?.Login) {
            this.refractoryByTeg[i].Src = this.rootUrl + '/image/get?imageName=' + this.refractoryByTeg[i].RefractoryPicture + '&userLogin=' + this.refractoryByTeg[i].UserInfo.Login;
          } else {
            this.refractoryByTeg[i].Src = '';
          }
        }
      }
      this.router.navigateByUrl('/ref-byteg');
    });
  }

  GetRefrCalc(data: Refcalc) {
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
 
    // Преобразуем строки в числа для API (PascalCase)
    const body = {
      a1: parseFloat(data.a1) || 0,
      a2: parseFloat(data.a2) || 0,
      b1: parseFloat(data.b1) || 0,
      b2: parseFloat(data.b2) || 0,
      BrickLength: parseFloat(data.BrickLength) || 0,
      TopDiameter: parseFloat(data.TopDiameter) || 0,
      BottomDiameter: parseFloat(data.BottomDiameter) || 0,
      RowNumber: parseFloat(data.RowNumber) || 0,
      Density: parseFloat(data.Density) || 0,
      Price: parseFloat(data.Price) || 0
    };
    
    console.log('Calc body being sent:', body);

    return this.http.post(this.rootUrl + '/calcRefRow', body, {headers: reqHeader}).
    subscribe({
      next: (res: any) => {
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
      },
      error: (err) => {
        console.error('Calc error:', err);
        this.toastr.error('Помилка розрахунку: ' + err.status);
        this.refCalcRes = null;
      }
    });
  }

}