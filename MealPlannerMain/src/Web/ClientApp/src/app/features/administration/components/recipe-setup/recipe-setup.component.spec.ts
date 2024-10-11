import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecipeSetupComponent } from './recipe-setup.component';

describe('RecipeSetupComponent', () => {
  let component: RecipeSetupComponent;
  let fixture: ComponentFixture<RecipeSetupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecipeSetupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecipeSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
