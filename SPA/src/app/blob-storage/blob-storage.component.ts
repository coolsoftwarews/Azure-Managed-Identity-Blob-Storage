import { Component, OnInit } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from 'src/environments/environment';

import * as fileSaver from 'file-saver';


@Component({
  selector: 'app-blob-storage',
  templateUrl: './blob-storage.component.html',
  styleUrls: ['./blob-storage.component.css']
})
export class BlobStorageComponent implements OnInit {
  blobList: string[] = [];
  
  constructor(
          private _httpClient: HttpClient
  ) { }

  ngOnInit(): void {  
    this.onGetAll();
  
   }
  
  public onUploadFile(event: any){
  
      let files = event.target.files as FileList;
  
      let formData = new FormData();
  
      Array.from(files).map((file, index)=>{
  
      return formData.append('file'+index, file, file.name);
  
      });
  
      return this._httpClient.post(`${environment.apiUrl}/api/blobs`, formData).subscribe(x=>{
          alert('Upload Complete!');
          this.onGetAll();
        },
        (err)=>{ 
          console.log(err);
          alert(err);
       
      });
    }
  
    public onDelete(resourceId: string)
    {
      
         return this._httpClient.delete(`${environment.apiUrl}/api/blobs/${resourceId}`).subscribe(x=>{
            alert("Deleted Successfully");
            this.onGetAll();
         });
    }
  
    public onDownload(resourceId: string)
    {
        return this._httpClient.get(`${environment.apiUrl}/api/blobs/${resourceId}`, {responseType: 'blob'}).subscribe(x=>{
          const url= window.URL.createObjectURL(new Blob([x]));
            
          fileSaver.saveAs(url, resourceId);
        });
    }
  
    
    public onGetAll()
    {
      return this._httpClient.get<string[]>(`${environment.apiUrl}/api/blobs`).subscribe(x=>{
        this.blobList = x;
      });
    }

}
