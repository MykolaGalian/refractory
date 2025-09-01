import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { RefractoryService } from '../../../services/refractory.service';
import { Refractory } from '../../../models/refractory.models';

@Component({
  selector: 'app-ref-byteg',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './ref-byteg.component.html',
  styleUrls: ['./ref-byteg.component.css']
})
export class RefractoryBytegComponent implements OnInit {
  private refractoryService = inject(RefractoryService);
  private router = inject(Router);

  get refractoryByTeg() {
    return this.refractoryService.refractoryByTeg;
  }

  ngOnInit(): void {
    // Component initialization
  }

  populateForm(pd: Refractory): void {
    this.refractoryService.refractory = Object.assign({}, pd);
    this.refractoryService.tempRefractoryId = pd.Id;
    this.router.navigate(['/read-ref']);
  }
}