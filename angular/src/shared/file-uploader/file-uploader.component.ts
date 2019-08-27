import { Component, OnInit, Output, EventEmitter, Input, SimpleChanges } from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-file-uploader',
    templateUrl: './file-uploader.component.html',
    styleUrls: ['./file-uploader.component.scss']
})
export class FileUploaderComponent implements OnInit {
    progress: number;
    message: string;
    fileSrc: string;
    @Input() originalFileUrl: string;
    @Input() saved: boolean;
    @Input() newFileName: string;
    @Input() baseServerUrl: string;
    @Output() public onUploadFinished = new EventEmitter();

    constructor(private http: HttpClient) {
        this.fileSrc = this.buildFileUrl(this.originalFileUrl);
    }

    ngOnInit() {
    }

    uploadFile = (files) => {
        if (files.length === 0) {
            return;
        }

        const fileToUpload = <File>files[0];
        const url = this.baseServerUrl + '/api/Files';
        const formData = new FormData();

        formData.append('file', fileToUpload, this.newFileName ? (this.newFileName + '-' + fileToUpload.name) : fileToUpload.name);

        this.http.post(url, formData, { reportProgress: true, observe: 'events' })
            .subscribe(event => {
                if (event.type === HttpEventType.UploadProgress) {
                    this.progress = Math.round(100 * event.loaded / event.total);
                } else if (event.type === HttpEventType.Response) {
                    const fileSource = event.body['result'].path;
                    this.message = 'Upload success.';
                    this.onUploadFinished.emit(fileSource);
                    this.fileSrc = this.buildFileUrl(fileSource);
                }
            },
                error => {
                    if (error.status = 400) {
                        this.message = 'Upload failed / unsupported or size';
                    }
                    console.log(error);
                });
    }

    ngOnChanges(changes: SimpleChanges) {
        if (changes.originalFileUrl) {
            this.fileSrc = this.buildFileUrl(changes.originalFileUrl.currentValue);
            this.message = null;
        } else {
            this.reset();
        }
    }

    buildFileUrl(fileUrl): string {
        return fileUrl ? this.baseServerUrl + '/' + fileUrl : null;
    }
    getExtension(filename): string {
        return filename.slice((filename.lastIndexOf(".") - 1 >>> 0) + 2);
    }

    reset(){
        this.message = null;
        this.fileSrc = null;
    }
}
