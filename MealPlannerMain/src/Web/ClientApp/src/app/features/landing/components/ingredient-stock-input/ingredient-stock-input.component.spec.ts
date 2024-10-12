import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IngredientStockInputComponent } from './ingredient-stock-input.component';

describe('IngredientStockInputComponent', () => {
  let component: IngredientStockInputComponent;
  let fixture: ComponentFixture<IngredientStockInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IngredientStockInputComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IngredientStockInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
