import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RefListComponent } from './ref-list/ref-list.component';

@Component({
  selector: 'app-moderator',
  standalone: true,
  imports: [CommonModule, RefListComponent],
  templateUrl: './moderator.component.html',
  styleUrls: ['./moderator.component.css']
})
export class ModeratorComponent implements OnInit {

  ngOnInit(): void {
  }
}