import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { OrderItem } from '../models/order-item';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CartService {
  private baseUrl = '/api/cart';

  constructor(private http: HttpClient) { }

  getCart(): Observable<OrderItem[]> {
    return this.http.get<OrderItem[]>(`${this.baseUrl}`);
  }

  increaseQuantity(productDetailsId: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/increase`, productDetailsId);
  }

  decreaseQuantity(productDetailsId: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/decrease`, productDetailsId);
  }

  checkout(): Observable<{ redirectUrl: string }> {
    return this.http.post<{ redirectUrl: string }>(`${this.baseUrl}/checkout`, {});
  }

  confirmOrder(sessionId: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/confirm`, sessionId);
  }

  addToCart(productDetailsId: number, designDetailsId: number, quantity: number = 1): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/add-item`, {
      productDetailsId,
      designDetailsId,
      quantity
    });
  }
}