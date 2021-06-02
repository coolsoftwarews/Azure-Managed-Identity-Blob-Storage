import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import {HttpClientModule} from '@angular/common/http';

import { BlobsComponent } from './blobs/blobs.component';
import { DataLakesComponent } from './data-lakes/data-lakes.component';



@NgModule({

  declarations: [
    AppComponent,
    BlobsComponent,
    DataLakesComponent
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
