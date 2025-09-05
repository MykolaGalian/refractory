import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { RefractoryService } from '../../../services/refractory.service';
import { CommentService } from '../../../services/comment.service';
import { Observable } from 'rxjs';
import { Refractory } from '../../../models/refractory.models';

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
  
  refractory$: Observable<Refractory | null>;

  get refractory() {
    return this.refractoryService.refractory;
  }

  constructor() {
    this.refractory$ = this.refractoryService.refractory$;
  }

  ngOnInit(): void {
    
    if (this.refractoryService.tempRefractoryId) {
      this.refractoryService.GetRefractoryById(this.refractoryService.tempRefractoryId);
    }
    this.userLogin = this.refractoryService.refractory?.UserInfo?.Login || '';
    
    // Подписка на изменения
    this.refractory$.subscribe(refractory => {
    });
  }

  AutorizeCheck(): boolean {
    return localStorage.getItem('access_token') === null;
  }

  OnAddComment(comment: string): void {
    if (comment.trim() && this.refractoryService.refractory) {
      const refId = this.refractoryService.refractory.Id;
      this.commentService.AddComment(refId, comment).subscribe({
        next: (data: any) => {
          
          // Добавляем комментарий локально
          if (this.refractoryService.refractory) {
            const currentUser = {
              Id: 0,
              Login: localStorage.getItem('Login') || '',
              Name: localStorage.getItem('Name') || '',
              LastName: localStorage.getItem('LastName') || '',
              Position: localStorage.getItem('Position') || '',
              UserAvatar: '',
              Email: '',
              Refractories: [],
              DateRegistration: new Date(),
              IsAdmin: false,
              IsModerator: false,
              IsBlocked: false
            };
            
            const newComment = {
              Id: Date.now(), // временный ID
              CommentBody: comment,
              RefId: refId,
              DateCreation: new Date(),
              LastEdit: new Date(),
              UserInfoId: currentUser.Id,
              UserInfo: currentUser
            };
            
            const updatedRefractory = {
              ...this.refractoryService.refractory,
              Comments: [...this.refractoryService.refractory.Comments, newComment]
            };
            
            this.refractoryService.refractory = updatedRefractory;
          }
          
          this.commentText = '';
        },
        error: (error) => {
          alert(`HTTP status code: ${error.status}`);
        }
      });
    }
  }

  calcRef(data: any): void {
    this.refractoryService.refractory = Object.assign({}, data);
    this.router.navigateByUrl('/ref-calc');
  }
}