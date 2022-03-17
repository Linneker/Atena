import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveTipoProdutoComponent } from './remove-tipo-produto.component';

describe('RemoveTipoProdutoComponent', () => {
  let component: RemoveTipoProdutoComponent;
  let fixture: ComponentFixture<RemoveTipoProdutoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RemoveTipoProdutoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RemoveTipoProdutoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
