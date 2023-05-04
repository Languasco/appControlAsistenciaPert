import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignarPersonalOTComponent } from './asignar-personal-ot.component';

describe('AsignarPersonalOTComponent', () => {
  let component: AsignarPersonalOTComponent;
  let fixture: ComponentFixture<AsignarPersonalOTComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsignarPersonalOTComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignarPersonalOTComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
