import { Component, OnInit } from '@angular/core';
import { RefractoryService } from '../../../shared/refractory.service';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { CommentService } from '../../../shared/comment.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-post',
  templateUrl: './user-post.component.html',
  styleUrls: ['./user-post.component.css']
})
export class UserPostComponent implements OnInit {

  private refractoryOnEdit:boolean=false;
  private commentText: string = '';

  constructor(private refractoryService: RefractoryService, private router: Router,
              private commentService: CommentService, private toastr: ToastrService) {}

  ngOnInit() {  }

  OnRefractoryEdit(form:NgForm) {
    this.refractoryService.EditRefractory(this.refractoryService.refractory.Id, form).subscribe((data: any) => {
        this.refractoryOnEdit = false;
        this.toastr.success('Post edited');
        },
        Error => {console.log(Error);
          this.toastr.error('HTTP status code', Error.status);
        });
  }

  OnDeleteRefractory() {
    this.refractoryService.DeleteRefractory(this.refractoryService.refractory.Id).subscribe((data: any) => {
      this.toastr.success('Post deleted');
        this.router.navigate(['/profile']);
      },
      Error => {console.log(Error);
        this.toastr.error('HTTP status code', Error.status);
      });
  }

  OnAddComment(comment:string) {
    this.commentService.AddComment(this.refractoryService.refractory.Id, comment).subscribe((data: any) => {
      this.toastr.success('Comment Added');
        this.commentText = '';
      },
      Error => {
        this.toastr.error('HTTP status code', Error.status);
      });
  }

  OnDeleteComment(commentId: number){

    this.commentService.RemoveComment(commentId).subscribe((data: any) => {
      this.toastr.success('Comment deleted');
      this.refractoryService.GetRefractoryById(this.refractoryService.tempRefractoryId);
    },
    Error => {
      this.toastr.error('HTTP status code', Error.status);
    });


  }

}
