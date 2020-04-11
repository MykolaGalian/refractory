import { Component, OnInit } from '@angular/core';
import { RefractoryService } from '../../../shared/refractory.service';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { CommentService } from '../../../shared/comment.service';
import { ToastrService } from 'ngx-toastr';

export interface ZoneOfRefractory { // обьект для выпадающего списка - зоны применения огнеупоров
  value: string;
}


@Component({
  selector: 'app-user-ref',
  templateUrl: './user-ref.component.html',
  styleUrls: ['./user-ref.component.css']
})
export class UserRefractoryComponent implements OnInit {

  private refractoryOnEdit:boolean=false;
  private commentText: string = '';

  constructor(private refractoryService: RefractoryService, private router: Router,
              private commentService: CommentService, private toastr: ToastrService) {}

  ngOnInit() {  }

  ZonesOfRefractory: ZoneOfRefractory[] = [ 
    { value: "Metal"},
    { value: "Slag"},
    { value: "Flange"},
    { value: "Transition"}   
  ] 

  OnRefractoryEdit(form:NgForm) {
    this.refractoryService.EditRefractory(this.refractoryService.refractory.Id, form).subscribe((data: any) => {
        this.refractoryOnEdit = false;
        this.toastr.success('Стаття по вогнетриву оновлена');
        },
        Error => {console.log(Error);
          this.toastr.error('HTTP status code', Error.status);
        });
  }

  OnDeleteRefractory() {
    this.refractoryService.DeleteRefractory(this.refractoryService.refractory.Id).subscribe((data: any) => {
      this.toastr.success('Стаття по вогнетриву видалена');
        this.router.navigate(['/profile']);
      },
      Error => {console.log(Error);
        this.toastr.error('HTTP status code', Error.status);
      });
  }

  OnAddComment(comment:string) {
    this.commentService.AddComment(this.refractoryService.refractory.Id, comment).subscribe((data: any) => {
      this.toastr.success('Відгук додано');
      
      this.refractoryService.GetRefractoryById(this.refractoryService.refractory.Id);
        this.commentText = '';
      },
      Error => {
        this.toastr.error('HTTP status code', Error.status);
      });
  }

  OnDeleteComment(commentId: number){

    this.commentService.RemoveComment(commentId).subscribe((data: any) => {
      this.toastr.success('Відгук видалено');
      
      this.refractoryService.GetRefractoryById(this.refractoryService.refractory.Id);
    },
    Error => {
      this.toastr.error('HTTP status code', Error.status);
    });
  }

  calcRef(data: any){
    this.refractoryService.refractory = Object.assign({}, data); 
    this.router.navigateByUrl('/ref-calc');    
  }

}
