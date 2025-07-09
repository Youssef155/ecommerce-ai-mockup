import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { tap, map, catchError } from 'rxjs/operators';
import { ProductVariation } from '../models/Products/product-variation';

@Injectable({
  providedIn: 'root'
})
export class ProductVariationService {
  private apiUrl = 'https://localhost:7256/api/Product';

  constructor(private http: HttpClient) { }

  getColorsBySize(productId: number, size: string): Observable<string[]> {
    return this.http.get<any>(`${this.apiUrl}/${productId}/sizes/${size}/colors`).pipe(
      tap(response => console.log('Raw API Response:', response)),
      map(variation => variation.availableColors || [])
    );
  }
}