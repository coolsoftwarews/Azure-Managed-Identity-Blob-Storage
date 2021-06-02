import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlobsComponent } from './blobs.component';

describe('BlobsComponent', () => {
  let component: BlobsComponent;
  let fixture: ComponentFixture<BlobsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BlobsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BlobsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
