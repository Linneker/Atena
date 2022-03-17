import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CadastroTipoProdutoComponent } from './cadastro-tipo-produto.component';

describe('CadastroTipoProdutoComponent', () => {
  let component: CadastroTipoProdutoComponent;
  let fixture: ComponentFixture<CadastroTipoProdutoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CadastroTipoProdutoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CadastroTipoProdutoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
