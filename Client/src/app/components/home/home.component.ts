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

  constructor(private refService: RefractoryService, private router: Router, private service: UserService) { }

  ngOnInit() {
    
    this.refService.GetRefractories();
    this.service.getUserProfile();
    this.refService.GetTegs();
  }


  populateForm(pd: Refractory) {       // метод обновляет данные во временном обьекте (refService.refractory) типа refractory на основании обьекта выделенного из списка pd в представлении
    this.refService.refractory = Object.assign({}, pd);   // Object.assign
    this.refService.tempRefractoryId = pd.Id;
    this.router.navigate(['/read-ref']);
  }

  searchByTeg(teg: string) {
    this.refService.SearchByTeg(teg);

  }
}

