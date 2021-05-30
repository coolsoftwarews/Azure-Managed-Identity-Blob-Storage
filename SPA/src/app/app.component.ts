import { Component, ElementRef, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

 // @ViewChild('fileUpload') el:ElementRef;
  
  title = 'SPA';

  constructor(
                private _httpClient: HttpClient
  ){}

  onUploadFile(event: any){

    let files = event.target.files as FileList;

    let formData = new FormData();

    Array.from(files).map((file, index)=>{

      return formData.append('file'+index, file, file.name);

    });

    return this._httpClient.post(`${environment.apiUrl}/api/documents`, formData).subscribe(x=>{
      alert('Done');
    },
      (err)=>{ 
        alert(err);
        console.log(err);

      });

  }
}
