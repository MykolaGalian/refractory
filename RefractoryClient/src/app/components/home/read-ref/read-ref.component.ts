import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { RefractoryService } from '../../../services/refractory.service';
import { CommentService } from '../../../services/comment.service';

@Component({
  selector: 'app-read-ref',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './read-ref.component.html',
  styleUrls: ['./read-ref.component.css']
})
export class ReadRefractoryComponent implements OnInit {
  private refractoryService = inject(RefractoryService);
  private commentService = inject(CommentService);
  private router = inject(Router);

  private userLogin: string = '';
  commentText: string = '';

  get refractory() {
    return this.refractoryService.refractory;
  }

  ngOnInit(): void {
    this.userLogin = this.refractoryService.refractory?.UserInfo?.Login || '';
  }

  AutorizeCheck(): boolean {
    return localStorage.getItem('access_token') === null;
  }

  OnAddComment(comment: string): void {
    if (comment.trim()) {
      if (this.refractoryService.refractory) {
        this.commentService.AddComment(this.refractoryService.refractory.Id, comment).subscribe({
          next: (data: any) => {
            alert('Відгук додано');
            this.refractoryService.GetRefractoryById(this.refractoryService.tempRefractoryId);
            this.commentText = '';
          },
          error: (error) => {
            alert(`HTTP status code: ${error.status}`);
          }
        });
      }
    }
  }

  calcRef(data: any): void {
    this.refractoryService.refractory = Object.assign({}, data);
    this.router.navigateByUrl('/ref-calc');
  }
}