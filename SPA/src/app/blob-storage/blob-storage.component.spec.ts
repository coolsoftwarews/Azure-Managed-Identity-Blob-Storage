import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlobStorageComponent } from './blob-storage.component';

describe('BlobStorageComponent', () => {
  let component: BlobStorageComponent;
  let fixture: ComponentFixture<BlobStorageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BlobStorageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BlobStorageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
