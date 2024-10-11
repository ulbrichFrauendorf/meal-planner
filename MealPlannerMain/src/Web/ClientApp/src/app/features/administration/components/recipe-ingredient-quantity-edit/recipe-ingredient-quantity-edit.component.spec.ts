import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecipeIngredientQuantityEditComponent } from './recipe-ingredient-quantity-edit.component';

describe('RecipeIngredientQuantityEditComponent', () => {
  let component: RecipeIngredientQuantityEditComponent;
  let fixture: ComponentFixture<RecipeIngredientQuantityEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecipeIngredientQuantityEditComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecipeIngredientQuantityEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
