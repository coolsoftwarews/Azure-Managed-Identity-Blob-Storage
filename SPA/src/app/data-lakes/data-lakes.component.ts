import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

import * as fileSaver from 'file-saver';

@Component({
  selector: 'app-data-lakes',
  templateUrl: './data-lakes.component.html',
  styleUrls: ['./data-lakes.component.css']
})
export class DataLakesComponent implements OnInit {
  blobList: string[] = [];

  constructor(
    private _httpClient: HttpClient
  ){}
  
ngOnInit(): void {  
  this.onGetAll();

 }

public onUploadFile(event: any){

    let files = event.target.files as FileList;

    let formData = new FormData();

    Array.from(files).map((file, index)=>{

    return formData.append('file'+index, file, file.name);

    });

    return this._httpClient.post(`${environment.apiUrl}/api/datalakes`, formData).subscribe(x=>{
      this.onGetAll();  
      alert('Upload Complete!');
      },
      (err)=>{ 
        console.log(err);
        alert(err);
     
    });
  }

  public onDelete(resourceId: string)
  {
    
       return this._httpClient.delete(`${environment.apiUrl}/api/datalakes/${resourceId}`).subscribe(x=>{
          alert("Deleted Successfully");
          this.onGetAll();
       });
  }

  public onDownload(resourceId: string)
  {
      return this._httpClient.get(`${environment.apiUrl}/api/datalakes/${resourceId}`, {responseType: 'blob'}).subscribe(x=>{
        const url= window.URL.createObjectURL(new Blob([x]));
          
        fileSaver.saveAs(url, resourceId);
      });
  }

  
  public onGetAll()
  {
    return this._httpClient.get<string[]>(`${environment.apiUrl}/api/datalakes`).subscribe(x=>{
      this.blobList = x;
    },
    (err) =>  console.log(err)
    );
  }
}
