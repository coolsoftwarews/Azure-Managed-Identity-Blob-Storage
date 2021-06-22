import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BlobStorageComponent } from './blob-storage/blob-storage.component';
import { DataLakeStorageComponent } from './data-lake-storage/data-lake-storage.component';

const routes: Routes = [
{path: "blob-storage", component: BlobStorageComponent},
{path: "datalake-storage", component: DataLakeStorageComponent}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
