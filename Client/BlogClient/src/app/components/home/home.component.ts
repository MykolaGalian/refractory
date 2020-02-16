import { Component, OnInit } from '@angular/core';
import { RefractoryService } from '../../shared/refractory.service';
import { UserService } from '../../shared/user.service';

import { Refractory } from '../../model/refractory/refractory';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private postService: RefractoryService, private router: Router, private service: UserService) { }

  ngOnInit() {
    this.postService.GetPosts();
    this.service.getUserProfile();
    this.postService.GetTegs();
  }


  populateForm(pd: Refractory) {       // метод обновляет данные во временном обьекте (postService.post) типа Post на основании обьекта выделенного из списка pd в представлении
    this.postService.post = Object.assign({}, pd);   // Object.assign
    this.postService.tempPostId = pd.Id;
    this.router.navigate(['/read-post']);
  }

  searchByTeg(teg: string) {
    this.postService.SearchByTeg(teg);

  }


}

