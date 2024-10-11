import { Component, OnInit } from '@angular/core';
import { MultiSelectModule } from 'primeng/multiselect';

@Component({
    selector: 'app-manage-user-claims',
    standalone: true,
    imports: [MultiSelectModule],
    templateUrl: './manage-user-claims.component.html',
    styleUrl: './manage-user-claims.component.scss',
})
export class ManageUserClaimsComponent implements OnInit{
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

}
