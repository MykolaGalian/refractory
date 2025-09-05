import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { RefractoryService } from '../../../services/refractory.service';
import { CommentService } from '../../../services/comment.service';
import { UserService } from '../../../services/user.service';
import { ModeratorService } from '../../../services/moderator.service';
import { Refractory } from '../../../models/refractory.models';

@Component({
  selector: 'app-ref-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './ref-list.component.html',
  styleUrls: ['./ref-list.component.css']
})
export class RefListComponent implements OnInit {
  private router = inject(Router);
  private refService = inject(RefractoryService);
  private postService = inject(RefractoryService);
  private commentService = inject(CommentService);
  private userService = inject(UserService);
  private moderService = inject(ModeratorService);

  get refractories() {
    return this.refService.moderRefractories;
  }

  get moderatorService() {
    return this.moderService;
  }

  ngOnInit(): void {
    this.postService.GetAllRefractories();
  }

  populateForm(pd: Refractory): void {
    this.refService.refractory = Object.assign({}, pd);
    this.refService.tempRefractoryId = pd.Id;
    this.router.navigate(['/read-ref']);
  }

  blockRefractory(id: number): void {
    this.moderService.BlockRefractory(id.toString());
  }

  unblockRefractory(id: number): void {
    this.moderService.UnblockRefractory(id.toString());
  }
}