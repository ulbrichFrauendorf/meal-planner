import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecipeAddIngredientsDialogComponent } from './recipe-add-ingredients-dialog.component';

describe('RecipeAddIngredientsDialogComponent', () => {
  let component: RecipeAddIngredientsDialogComponent;
  let fixture: ComponentFixture<RecipeAddIngredientsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecipeAddIngredientsDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecipeAddIngredientsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
