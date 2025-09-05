import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { QuillModule } from 'ngx-quill';
import { RefractoryService } from '../../services/refractory.service';

export interface ZoneOfRefractory {
  value: string;
}

@Component({
  selector: 'app-ref-add',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, QuillModule],
  templateUrl: './ref-add.component.html',
  styleUrls: ['./ref-add.component.css']
})
export class RefractoryAddComponent implements OnInit {
  private refractoryService = inject(RefractoryService);
  private router = inject(Router);

  ZonesOfRefractory: ZoneOfRefractory[] = [
    { value: "Metal" },
    { value: "Slag" },
    { value: "Flange" },
    { value: "Transition" }
  ];

  fileToUpload: File | null = null;
  imageUrl: any = null;
  description: string = '';

  ngOnInit(): void {
  }

  handleFileInput(files: FileList | null): void {
    if (files && files.length > 0) {
      this.fileToUpload = files.item(0);
      const reader = new FileReader();
      reader.onload = (event: any) => {
        this.imageUrl = event.target.result;
      };
      if (this.fileToUpload) {
        reader.readAsDataURL(this.fileToUpload);
      }
    }
  }

  onSubmit(form: NgForm): void {
    console.log('Form values:', form.value);
    console.log('Description from component:', this.description);
    
    if (!this.description || this.description.trim() === '') {
      alert('Будь ласка, введіть опис вогнетриву');
      return;
    }
    
    if (this.fileToUpload !== null) {
      this.refractoryService.AddRefractory(form, this.fileToUpload, this.description).subscribe({
        next: (data: any) => {
          this.router.navigate(['/profile']);
        },
        error: (error) => {
          console.error('Error details:', error);
          alert(`Помилка: ${error.status}`);
        }
      });
    } else {
      alert('Будь ласка, завантажте фото');
    }
  }

  maxLength(e: any): void {
    if (e.editor && e.editor.getLength() > 10000) {
      e.editor.deleteText(10, e.editor.getLength());
    }
  }
}