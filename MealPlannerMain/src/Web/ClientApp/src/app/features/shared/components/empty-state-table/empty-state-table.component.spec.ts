import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmptyStateTableComponent } from './empty-state-table.component';

describe('EmptyStateTableComponent', () => {
  let component: EmptyStateTableComponent;
  let fixture: ComponentFixture<EmptyStateTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmptyStateTableComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EmptyStateTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
