import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {

   // API url
   baseApiUrl = "https://localhost:44358/api/"
  constructor(private http:HttpClient) { }

  upload(file: any):Observable<any> {
    return this.http.post(this.baseApiUrl + "PrintQueue/SendFiles", {priority: "1", filename: file.name})
}

 search(name: string){
   return this.http.get(this.baseApiUrl + `PrintQueue?fileName=${name}`)
 }
}
