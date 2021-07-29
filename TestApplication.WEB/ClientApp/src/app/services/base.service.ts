import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BaseService {
  errorMessage: { status: any; message: string };
  protected baseEndPoint = environment.baseEndPoint;
  protected httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  constructor(
  ) { }

  server4xxError(error: Response | any) {
    let isLogin = false;

    if (error.url !== undefined && error.url !== null) {
      isLogin = error.url.includes('api/Account/authenticate');
    }

    if (error.status === 0) {
      this.errorMessage = {
        message: 'Please check your internet connection',
        status: error.status
      };
    } else if (error.status === 401 && isLogin) {
      localStorage.removeItem('TokenId');
      this.errorMessage = {
        message: 'Invalid username or password',
        status: error.status
      };
    } else if (
      error.status === 401 ||
      error.status === 403 ||
      error.status === 404 ||
      error.status === 408
    ) {
      localStorage.removeItem('TokenId');
      this.errorMessage = {
        message: 'Your login time has been expired, login again',
        status: error.status
      };
    } else {
      this.errorMessage = {
        message: error.error.message,
        status: error.status
      };
    }
    const errorMsg = Object.assign({}, this.errorMessage);
    return throwError(errorMsg);
  }
}
