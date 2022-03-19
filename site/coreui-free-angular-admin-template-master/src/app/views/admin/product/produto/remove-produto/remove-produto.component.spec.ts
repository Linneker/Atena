import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveProdutoComponent } from './remove-produto.component';

describe('RemoveProdutoComponent', () => {
  let component: RemoveProdutoComponent;
  let fixture: ComponentFixture<RemoveProdutoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RemoveProdutoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RemoveProdutoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
