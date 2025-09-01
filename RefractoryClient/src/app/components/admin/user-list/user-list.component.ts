import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../services/user.service';
import { UserDetails } from '../../../models/user.models';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  private userService = inject(UserService);

  get service() {
    return this.userService;
  }

  ngOnInit(): void {
    this.userService.getAllUserProfiles();
  }

  populateForm(pd: UserDetails): void {
    this.userService.userDetails = Object.assign({}, pd);
  }
}