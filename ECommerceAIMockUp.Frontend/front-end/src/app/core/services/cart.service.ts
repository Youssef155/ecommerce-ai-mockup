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
deleteItem(orderId: number, productId: number, designId: number): Observable<void> {
  return this.http.delete<void>(
    `${this.apiUrl}/${orderId}/${productId}/${designId}`
  );
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

  if (orderId === 0) {
    // First item - create cart with minimal shipping info
    const orderDto: OrderDTO = {
      phoneNumber: 'TEMP',  // Temporary required value
      city: 'TEMP',
      governorate: 'TEMP',
      street: 'TEMP',
      zip: 'TEMP',
      productDetailsId,
      designDetailsId,
    };
      return this.addOrder(orderDto).pipe(
        switchMap(() => this.getAllItems()),
        switchMap(items => {
          if (items.length > 0) {
            const newOrderId = items[0].orderId;
            localStorage.setItem('cartId', String(newOrderId));
            
            // Now add the first item with proper quantity
            return this.addOrderItem({
              orderId: newOrderId,
              productDetailsId,
              designDetailsId,
              quantity
            });
          }
          throw new Error('Cart creation failed');
        }),
        mapTo(undefined)
    );
  } else {
    // Existing cart
    return this.addOrderItem({
      orderId,
      productDetailsId,
      designDetailsId,
      quantity
    });
  }
}
}
