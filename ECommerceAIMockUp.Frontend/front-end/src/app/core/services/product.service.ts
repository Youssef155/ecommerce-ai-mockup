import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private readonly APIUrl = 'https://localhost:7256/api/Product'; 

  constructor(private http : HttpClient) { }

  getProducts(): Observable<any>{
    return this.http.get(`${this.APIUrl}/Products`);
  }
}
