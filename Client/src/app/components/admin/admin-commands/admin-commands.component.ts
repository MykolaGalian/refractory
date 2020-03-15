import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../shared/admin.service';
import { UserService } from '../../../shared/user.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-admin-commands',
  templateUrl: './admin-commands.component.html',
  styleUrls: ['./admin-commands.component.css']
})
export class AdminCommandsComponent implements OnInit {

  constructor(private router: Router,private adminService: AdminService, private service: UserService) { }

  ngOnInit() {
  }

  regNewUser(){
    this.router.navigateByUrl('/registration');
  }

}
