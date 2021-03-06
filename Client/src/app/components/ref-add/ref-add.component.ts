import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { RefractoryService } from '../../shared/refractory.service';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

export interface ZoneOfRefractory { // обьект для выпадающего списка - зоны применения огнеупоров
  value: string;
}

@Component({
  selector: 'app-ref-add',
  templateUrl: './ref-add.component.html',
  styleUrls: ['./ref-add.component.css']
})
export class RefractoryAddComponent implements OnInit {

  constructor(private refractoryService: RefractoryService, private router: Router, private toastr: ToastrService) { }


  ZonesOfRefractory: ZoneOfRefractory[] = [ 
    { value: "Metal"},
    { value: "Slag"},
    { value: "Flange"},
    { value: "Transition"}   
  ] 

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
        this.refractoryService.AddRefractory(form,  this.fileToUpload).subscribe((data: any) => {
          this.refractoryService.GetRefractoryByBrand(form);
          });
       }
     }

     maxLength(e) {
      if(e.editor.getLength() > 10000) {
      e.editor.deleteText(10, e.editor.getLength());
      }
    }
}
