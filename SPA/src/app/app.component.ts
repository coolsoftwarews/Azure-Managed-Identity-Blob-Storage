import { Component} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

 
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
      alert('Upload Complete!');
    },
      (err)=>{ 
        alert(err);
        console.log(err);
      });

  }

  onDelete()
  {
    let resourceId = "";
    return this._httpClient.delete(`${environment.apiUrl}/api/documents/${resourceId}`).subscribe(x=>{

    });
  }

  onDownload()
  {
    let resourceId = "";
    return this._httpClient.get(`${environment.apiUrl}/api/documents/${resourceId}`, {responseType: 'blob'}).subscribe(x=>{

    });
  }
}
