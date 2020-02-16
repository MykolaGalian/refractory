import { Component, OnInit } from '@angular/core';
import { RefractoryService } from '../../../shared/refractory.service';
import { CommentService } from '../../../shared/comment.service';
import { UserService } from '../../../shared/user.service';
import { ModeratorService } from '../../../shared/moderator.service';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.css']
})
export class PostListComponent implements OnInit {

  constructor(private postService: RefractoryService, private commentService: CommentService,
              private service: UserService, private moderService: ModeratorService) { }

  ngOnInit() {
    this.postService.GetAllPosts();
  }


}
