import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductVariation } from '../models/Products/product-variation';

@Injectable({
  providedIn: 'root'
})
export class ProductVariationService {
  private apiUrl = 'https://localhost:7256/api/Product'; // Replace with your API URL

  constructor(private http: HttpClient) { }

  getColorsBySize(productId: number, size: string): Observable<ProductVariation> {
    return this.http.get<ProductVariation>(`${this.apiUrl}/${productId}?size=${size}`);
  }
}