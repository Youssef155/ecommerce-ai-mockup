import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, ReplaySubject, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { RegisterDto } from '../models/RegisterDto';
import { JwtUserData, TokenData } from '../models/TokenData';
import { LoginDto } from '../models/LoginDto';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly APIUrl = 'https://localhost:7256/api/account';

  constructor(private http: HttpClient, private router: Router) {
  }

  register(model: RegisterDto): Observable<any> {
    return this.http.post(`${this.APIUrl}/Register`, model);
  }

  Login(model : any): Observable<any>{
    return this.http.post(`${this.APIUrl}/login`, model);
  }
}