import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BlobsComponent } from './blobs/blobs.component';
import { DataLakesComponent } from './data-lakes/data-lakes.component';

const routes: Routes = [
  { path: 'blob-sample', component: BlobsComponent },
  { path: 'datalake-sample', component: DataLakesComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { 


}
