import { Component, OnInit } from '@angular/core';
import { RefractoryService } from '../../../shared/refractory.service';
import { CommentService } from '../../../shared/comment.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-read-ref',
  templateUrl: './read-ref.component.html',
  styleUrls: ['./read-ref.component.css']
})
export class ReadRefractoryComponent implements OnInit {

  private userLogin: string = '';
  private commentText: string = '';

  constructor(private refractoryService: RefractoryService, private commentService: CommentService, private toastr: ToastrService) { }

  ngOnInit() {
   this.userLogin =this.refractoryService.refractory.UserInfo.Login;

  }

  AutorizeCheck() {
    return (localStorage.getItem('access_token') === null)
  }

  OnAddComment(comment:string) {
    this.commentService.AddComment(this.refractoryService.refractory.Id, comment).subscribe((data: any) => {
      this.toastr.success('Відгук додано');
      this.refractoryService.GetRefractoryById(this.refractoryService.tempRefractoryId);
        this.commentText = '';
      },
      Error => {
        this.toastr.error('HTTP status code', Error.status);
      });
  } 

}
