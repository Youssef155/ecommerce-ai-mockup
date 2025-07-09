import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { switchMap, tap, mapTo } from 'rxjs/operators';

export interface OrderDTO {
  phoneNumber:      string;
  city:             string;
  governorate:      string;
  street:           string;
  zip:              string;
  productDetailsId: number;
  designDetailsId:  number;
  quantity:         number;
}

export interface OrderItemDTO {
  orderId:          number;
  productDetailsId: number;
  designDetailsId:  number;
  quantity:         number;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = 'https://localhost:7256/api/ShoppingCart';

  constructor(private http: HttpClient) {}

  /** Create cart (if needed) + add first item */
  addOrder(order: OrderDTO): Observable<void> {
    return this.http.post<void>(this.apiUrl, order);
  }

  /** Add another item to existing cart */
  addOrderItem(item: OrderItemDTO): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/OrderItem`, item);
  }

  /** Get all items in the cart */
  getAllItems(): Observable<OrderItemDTO[]> {
    return this.http.get<OrderItemDTO[]>(this.apiUrl);
  }

  /** Remove an item */
  deleteItem(itemId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${itemId}`);
  }

  /**
   * Smart helper: call this to add-to-cart in your product-details.
   * It will create the cart on first call, then add items thereafter.
   */
  addToCart(
    productDetailsId: number,
    designDetailsId: number,
    quantity: number
  ): Observable<void> {
    const saved = localStorage.getItem('cartId');
    const orderId = saved ? Number(saved) : 0;

    const itemDto: OrderItemDTO = {
      orderId,
      productDetailsId,
      designDetailsId,
      quantity
    };

    if (orderId === 0) {
      // first add → build full OrderDTO
      const orderDto: OrderDTO = {
        phoneNumber:      '',  // fill these out if you collect shipping up front
        city:             '',
        governorate:      '',
        street:           '',
        zip:              '',
        productDetailsId,
        designDetailsId,
        quantity
      };
      return this.addOrder(orderDto).pipe(
        // fetch back to get the new orderId
        switchMap(() => this.getAllItems()),
        tap(items => {
          if (items.length) {
            localStorage.setItem('cartId', String(items[0].orderId));
          }
        }),
        mapTo(undefined)
      );
    } else {
      // just add to existing cart
      return this.addOrderItem(itemDto);
    }
  }
}
