import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationLandingComponent } from './administration-landing.component';

describe('AdministrationLandingComponent', () => {
  let component: AdministrationLandingComponent;
  let fixture: ComponentFixture<AdministrationLandingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdministrationLandingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdministrationLandingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
