import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CookieConsertComponent } from './cookie-consert.component';

describe('CookieConsertComponent', () => {
  let component: CookieConsertComponent;
  let fixture: ComponentFixture<CookieConsertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CookieConsertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CookieConsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
