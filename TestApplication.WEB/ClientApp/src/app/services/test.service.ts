import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import { catchError } from 'rxjs/operators';
import { ImageUploadModel, UploadResultModel } from '../models/ImageUploadModel';

@Injectable({
  providedIn: 'root'
})
export class TestService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  uploadImage(model: ImageUploadModel) {

    const formData = new FormData();
    for (const key of Object.keys(model)) {
      const value = model[key];
      if(typeof value === 'object') {
        formData.append(key, value.data);
      }else{
        formData.append(key, value);
      }
    }
    
    return this.http
      .post(
        `${this.baseEndPoint}/image-upload`,
        formData
      )
      .pipe(catchError(this.server4xxError));

  }

  getImages() {

    return this.http
      .get<Array<UploadResultModel>>(
        `${this.baseEndPoint}/image-upload`
      )
      .pipe(catchError(this.server4xxError));

  }
}
