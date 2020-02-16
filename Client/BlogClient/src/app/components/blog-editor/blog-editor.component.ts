import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { RefractoryService } from '../../shared/refractory.service';

import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-blog-editor',
  templateUrl: './blog-editor.component.html',
  styleUrls: ['./blog-editor.component.css']
})
export class BlogEditorComponent implements OnInit {

  constructor(private postService: RefractoryService, private router: Router, private toastr: ToastrService) { }

 fileToUpload: File = null;
 imageUrl: any = null;


    ngOnInit() {

    }

    handleFileInput(file: FileList) {
      this.fileToUpload = file.item(0);
      var reader = new FileReader();
      reader.onload = (event: any) => {
        this.imageUrl = event.target.result;
      };
      reader.readAsDataURL(this.fileToUpload);

    }

    onSubmit(form: NgForm) {

      if (this.fileToUpload !== null) {
        this.postService.AddPost(form,  this.fileToUpload).subscribe((data: any) => {

          this.postService.GetPostByTitle(form);
          });

       }
     }



     maxLength(e) {
      if(e.editor.getLength() > 10000) {
      e.editor.deleteText(10, e.editor.getLength());
      }
    }

}
