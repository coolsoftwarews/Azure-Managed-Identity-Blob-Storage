import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataLakeStorageComponent } from './data-lake-storage.component';

describe('DataLakeStorageComponent', () => {
  let component: DataLakeStorageComponent;
  let fixture: ComponentFixture<DataLakeStorageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DataLakeStorageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DataLakeStorageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
