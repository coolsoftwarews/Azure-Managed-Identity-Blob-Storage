import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import {HttpClient, HttpClientModule} from '@angular/common/http'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { BlobStorageComponent } from './blob-storage/blob-storage.component';
import { DataLakeStorageComponent } from './data-lake-storage/data-lake-storage.component';

@NgModule({
  declarations: [
    AppComponent,
    BlobStorageComponent,
    DataLakeStorageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
