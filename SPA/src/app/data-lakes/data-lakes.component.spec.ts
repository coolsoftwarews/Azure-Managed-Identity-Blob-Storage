import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataLakesComponent } from './data-lakes.component';

describe('DataLakesComponent', () => {
  let component: DataLakesComponent;
  let fixture: ComponentFixture<DataLakesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DataLakesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DataLakesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
