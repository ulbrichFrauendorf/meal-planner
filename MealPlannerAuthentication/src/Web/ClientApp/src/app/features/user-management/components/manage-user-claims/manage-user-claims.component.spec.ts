import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageUserClaimsComponent } from './manage-user-claims.component';

describe('ManageUserClaimsComponent', () => {
  let component: ManageUserClaimsComponent;
  let fixture: ComponentFixture<ManageUserClaimsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageUserClaimsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ManageUserClaimsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
