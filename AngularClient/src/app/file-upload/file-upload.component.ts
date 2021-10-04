import { Component, OnInit } from '@angular/core';
import { FileUploadService } from './file-upload.service';
import { FileResponse } from './Models/fileResponse';
import { consumedFileResponse } from './Models/consumedFileResponse';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent implements OnInit {

  loading: boolean = false; 
  file!: File; 
  responseFile!: FileResponse;
  consumedResponseFile!: consumedFileResponse;

  constructor(private fileUploadService: FileUploadService) { }

  ngOnInit(): void {
  }

  onChange(event: any) {
    this.file = event.target.files[0];
}

 onUpload() {
  this.loading = !this.loading;
  this.fileUploadService.upload(this.file).subscribe(
      (response: any) => {
          if (response) {
              this.loading = false; 
              this.responseFile = response;
          }
      }
  );
}

searchDocument(name: string) {
  this.fileUploadService.search(name).subscribe(
    (response: any) => {
      this.consumedResponseFile = response;
    }, (error) => {
      console.log(error);
    }
  )
}
}
