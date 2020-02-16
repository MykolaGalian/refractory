import { Component, OnInit } from '@angular/core';
import { RefractoryService } from '../../../shared/refractory.service';
import { Refractory } from '../../../model/refractory/refractory';
import { Router } from '@angular/router';

@Component({
  selector: 'app-post-byteg',
  templateUrl: './post-byteg.component.html',
  styleUrls: ['./post-byteg.component.css']
})
export class PostBytegComponent implements OnInit {

  constructor(private postService: RefractoryService, private router: Router) { }

  ngOnInit() {
  }

  populateForm(pd: Refractory) {       // метод обновляет данные во временном обьекте (postService.post) типа Post на основании обьекта выделенного из списка pd в представлении
    this.postService.post = Object.assign({}, pd);   // Object.assign - предотвращает корректировку полей в
    this.postService.tempPostId = pd.Id;
    this.router.navigate(['/read-post']);
  }

}
