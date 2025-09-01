import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { RefractoryService } from '../../services/refractory.service';
import { UserService } from '../../services/user.service';
import { Refractory } from '../../models/refractory.models';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  private refService = inject(RefractoryService);
  private router = inject(Router);
  private userService = inject(UserService);

  get refractories() {
    return this.refService.refractories;
  }

  get tegsList() {
    return this.refService.tegslist;
  }

  ngOnInit(): void {
    this.refService.GetRefractories();
    this.userService.getUserProfile();
    this.refService.GetTegs();
  }

  populateForm(pd: Refractory): void {
    this.refService.refractory = Object.assign({}, pd);
    this.refService.tempRefractoryId = pd.Id;
    this.router.navigate(['/read-ref']);
  }

  searchByTeg(teg: string): void {
    this.refService.SearchByTeg(teg);
  }
}