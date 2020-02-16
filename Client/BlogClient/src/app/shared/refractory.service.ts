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
  tempPostId: number = 0;
  postsByTeg:  Refractory[] = null;
  moderPosts:  Refractory[] = null;
  posts: Refractory[] = null;
  post: Refractory =null;
  readonly rootUrl = 'https://localhost:44302/api/refractory';
  postForGetId: Refractory = null;
  bodyForNewPost: string = null;

  constructor(private http: HttpClient, private router: Router) { }

  GetPosts() {

       var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });

       return this.http.get(this.rootUrl + '/getallrefractories', {headers:reqHeader}).

       subscribe((res: any) => {
           this.posts = res as Refractory[];

           for (let i = 0; i < this.posts.length; i++) {
           this.posts[i].Src = this.rootUrl +'/image/get?imageName=' + this.posts[i].RefractoryPicture +'&userLogin=' + this.posts[i].UserInfo.Login;
          }
         })
       }

       GetPostsById(RefId: number) {
        var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
           return this.http.get(this.rootUrl + '/' +RefId, {headers:tokenHeader}).

           subscribe((res: any) => {
               this.post = res as Refractory;
               this.post.Src = this.rootUrl +'/image/get?imageName=' + this.post.RefractoryPicture +'&userLogin=' + this.post.UserInfo.Login;

             })
           }

  GetAllPosts(){
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
       return this.http.get(this.rootUrl + '/getallrefractoriesmoder', {headers:tokenHeader}).

       subscribe((res: any) => {
           this.moderPosts = res as Refractory[];


           for (let i = 0; i < this.moderPosts.length; i++) {
           this.moderPosts[i].Src = this.rootUrl +'/image/get?imageName=' + this.moderPosts[i].RefractoryPicture +'&userLogin=' + this.moderPosts[i].UserInfo.Login;
          }
       })
  }

   AddPost(data: any, fileToUpload: File)  {
      const formData: FormData = new FormData();
      formData.append('RefPicture', fileToUpload, fileToUpload.name);
      this.bodyForNewPost = data.value.body;
      formData.append("Brand", data.value.title);
      formData.append("Type", data.value.hashtags);

    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.post(this.rootUrl+'/add', formData, {headers: tokenHeader});
  }

  EditPost(postId: number, form: NgForm) {
    const body: EditRefractory = {
      RefractoryDescription: form.value.PostBody, //PostBody - Body
      RefractoryBrand: form.value.PostTitle,
      RefractoryType: form.value.Hashtags
    }
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.put(this.rootUrl + '/'+postId , body, {headers: tokenHeader});
  }

  //for new post add text body, after send picture and get post Id
  AddBodyForPost(post: Refractory) {
    const body: EditRefractory = {
      RefractoryDescription: this.bodyForNewPost, //PostBody - Body
      RefractoryBrand: post.RefractoryBrand,
      RefractoryType: post.RefractoryType
    }
    var tokenHeader = new HttpHeaders({'Authorization': 'Bearer  ' + localStorage.getItem('access_token')});
    return this.http.put(this.rootUrl + '/'+post.Id , body, {headers: tokenHeader}).
    subscribe((res: any) => {
      this.router.navigate(['/profile']);
    });
  }


  GetPostByTitle(data: any){
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });

    const body: EditRefractory = {
      RefractoryDescription: data.value.body,
      RefractoryBrand: data.value.title, // send title
      RefractoryType: data.value.hashtags
    }

       return this.http.post(this.rootUrl + '/getrefbybrand', body , {headers:reqHeader}).

       subscribe((res: any) => {
           this.postForGetId = res as Refractory;   // getting Post and PostId from it
           this.AddBodyForPost(this.postForGetId)
  });
}



  DeletePost(Id: number) {
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
              this.postsByTeg = res as Refractory[];
              for (let i = 0; i < this.postsByTeg.length; i++) {
                this.postsByTeg[i].Src = this.rootUrl +'/image/get?imageName=' + this.postsByTeg[i].RefractoryPicture +'&userLogin=' + this.postsByTeg[i].UserInfo.Login;
              }
              this.router.navigateByUrl('/post-byteg');
        });
  }



}


