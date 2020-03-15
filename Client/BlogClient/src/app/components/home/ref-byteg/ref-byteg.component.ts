import { Component, OnInit } from '@angular/core';
import { RefractoryService } from '../../../shared/refractory.service';
import { Refractory } from '../../../model/refractory/refractory';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ref-byteg',
  templateUrl: './ref-byteg.component.html',
  styleUrls: ['./ref-byteg.component.css']
})
export class RefractoryBytegComponent implements OnInit {

  constructor(private refractoryService: RefractoryService, private router: Router) { }

  ngOnInit() {
  }

  populateForm(pd: Refractory) {       // метод обновляет данные во временном обьекте (postService.post) типа Post на основании обьекта выделенного из списка pd в представлении
    this.refractoryService.refractory = Object.assign({}, pd);   // Object.assign - предотвращает корректировку полей в
    this.refractoryService.tempRefractoryId = pd.Id;
    this.router.navigate(['/read-post']);
  }

}
