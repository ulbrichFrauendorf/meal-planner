import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecipeMaximizeResultComponent } from './recipe-maximize-result.component';

describe('RecipeMaximizeResultComponent', () => {
  let component: RecipeMaximizeResultComponent;
  let fixture: ComponentFixture<RecipeMaximizeResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecipeMaximizeResultComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecipeMaximizeResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
