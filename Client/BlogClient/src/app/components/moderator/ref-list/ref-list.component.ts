import { Component, OnInit } from '@angular/core';
import { RefractoryService } from '../../../shared/refractory.service';
import { CommentService } from '../../../shared/comment.service';
import { UserService } from '../../../shared/user.service';
import { ModeratorService } from '../../../shared/moderator.service';
import { Refractory } from 'src/app/model/refractory/refractory';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ref-list',
  templateUrl: './ref-list.component.html',
  styleUrls: ['./ref-list.component.css']
})
export class RefractoryListComponent implements OnInit {

  constructor(private router: Router, private refService: RefractoryService, private postService: RefractoryService, private commentService: CommentService,
              private service: UserService, private moderService: ModeratorService) { }

  ngOnInit() {
    this.postService.GetAllRefractories();
  }

  populateForm(pd: Refractory) {       // метод обновляет данные во временном обьекте (refService.refractory) типа Refractory на основании обьекта выделенного из списка pd в представлении
    this.refService.refractory = Object.assign({}, pd);   // Object.assign
    this.refService.tempRefractoryId = pd.Id;
    this.router.navigate(['/read-post']);
  }


}
