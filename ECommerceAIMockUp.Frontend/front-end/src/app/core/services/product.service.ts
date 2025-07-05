import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/Products/Product';

@Injectable({
  providedIn: 'root'
})

export class ProductService {

  private readonly APIUrl = 'https://localhost:7256/api/Product'; 
  private readonly pageSize = 10;
  constructor(private http : HttpClient) { }

  getProducts(page: number =1): Observable<ApiResponse>{
    return this.http.get<ApiResponse>(`${this.APIUrl}/Products?pageNumber=${page}&&pageSize=${this.pageSize}`);
  }

  getFilteredProducts(page: number = 1,selectedGenders: string[] = [],selectedSeasons: string[] = [],categoryId?: number)
  :Observable<ApiResponse> {
    const params: any = {pageNumber: page, pageSize: this.pageSize,};

    if (selectedGenders.length) {
      params.gender = selectedGenders.join(',');
    }

    if (selectedSeasons.length) {
      params.seasons = selectedSeasons.join(',');
    }

    if (categoryId != null) {
      params.categoryId = categoryId;
    }
    return this.http.get<ApiResponse>(`${this.APIUrl}/Filter/products`, { params });
  }

  getProduct(id: number): Observable<any> {
    return this.http.get(`${this.APIUrl}/${id}`);
  }

  getColorsBySize(id: number, size: string): Observable<any> {
    return this.http.get(`${this.APIUrl}/${id}/sizes/${size}/colors`);
  }

  getVariant(id: number, size: string, color: string): Observable<any> {
    const params = new HttpParams()
      .set('size', size)
      .set('color', color);

    return this.http.get(`${this.APIUrl}/${id}/variant`, { params });
  }
}
