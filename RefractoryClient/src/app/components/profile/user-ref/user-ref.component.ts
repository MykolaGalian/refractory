import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { QuillModule } from 'ngx-quill';
import { RefractoryService } from '../../../services/refractory.service';
import { CommentService } from '../../../services/comment.service';

export interface ZoneOfRefractory {
  value: string;
}

@Component({
  selector: 'app-user-ref',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, QuillModule],
  templateUrl: './user-ref.component.html',
  styleUrls: ['./user-ref.component.css']
})
export class UserRefractoryComponent implements OnInit {
  private refractoryService = inject(RefractoryService);
  private router = inject(Router);
  private commentService = inject(CommentService);

  refractoryOnEdit: boolean = false;
  commentText: string = '';

  get refractory() {
    return this.refractoryService.refractory;
  }

  ZonesOfRefractory: ZoneOfRefractory[] = [
    { value: "Metal" },
    { value: "Slag" },
    { value: "Flange" },
    { value: "Transition" }
  ];

  ngOnInit(): void {
  }

  OnRefractoryEdit(form: NgForm): void {
    if (this.refractoryService.refractory) {
      this.refractoryService.EditRefractory(this.refractoryService.refractory.Id, form).subscribe({
      next: (data: any) => {
        this.refractoryOnEdit = false;
        alert('Стаття по вогнетриву оновлена');
      },
      error: (error) => {
        console.log(error);
        alert(`HTTP status code: ${error.status}`);
      }
    });
    }
  }

  OnDeleteRefractory(): void {
    if (confirm('Ви впевнені, що хочете видалити цей вогнетрив?') && this.refractoryService.refractory) {
      this.refractoryService.DeleteRefractory(this.refractoryService.refractory.Id).subscribe({
        next: (data: any) => {
          alert('Стаття по вогнетриву видалена');
          this.router.navigate(['/profile']);
        },
        error: (error) => {
          console.log(error);
          alert(`HTTP status code: ${error.status}`);
        }
      });
    }
  }

  OnAddComment(comment: string): void {
    if (comment.trim() && this.refractoryService.refractory) {
      this.commentService.AddComment(this.refractoryService.refractory.Id, comment).subscribe({
        next: (data: any) => {
          alert('Відгук додано');
          if (this.refractoryService.refractory) {
            this.refractoryService.GetRefractoryById(this.refractoryService.refractory.Id);
          }
          this.commentText = '';
        },
        error: (error) => {
          alert(`HTTP status code: ${error.status}`);
        }
      });
    }
  }

  OnDeleteComment(commentId: number): void {
    if (confirm('Видалити коментар?')) {
      this.commentService.RemoveComment(commentId).subscribe({
        next: (data: any) => {
          alert('Відгук видалено');
          if (this.refractoryService.refractory) {
            this.refractoryService.GetRefractoryById(this.refractoryService.refractory.Id);
          }
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