import { HttpClient } from '@angular/common/http';
import { Injectable, } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { switchMap, tap, mapTo, map, catchError } from 'rxjs/operators';
import { CartItem } from '../models/cartItem';

export interface OrderItemDTO {
  productDetailsId: number;
  designDetailsId: number;
  quantity: number;
  imgUrl: string;
  designImgUrl: string;
  productName: string;
  unitPrice: number;
  lineTotal: number;
  orderTotal: number;
}

export interface OrderDTO {
  phoneNumber?: string;
  city?: string;
  governorate?: string;
  street?: string;
  zip?: string;
  orderItems: OrderItemDTO[];
}



@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = 'https://localhost:7256/api/ShoppingCart';
  private cartIdKey = 'cartId';

  constructor(private http: HttpClient) {}

  /** Create cart (if needed) + add first item */
 addToCart(
    productDetailsId: number,
    designDetailsId: number,
    quantity: number,
    productName: string,
    unitPrice: number,
    imgUrl: string,
    designImgUrl: string
  ): Observable<void> {
    const isNewCart = !localStorage.getItem(this.cartIdKey);
    
    // Calculate line total and order total
    const lineTotal = unitPrice * quantity;
    const orderTotal = lineTotal; // Will be updated to sum of all items
    
    const orderDto: OrderDTO = {
      orderItems: [{
        productDetailsId,
        designDetailsId,
        quantity,
        imgUrl,
        designImgUrl,
        productName,
        unitPrice,
        lineTotal,
        orderTotal
      }],
      ...(isNewCart && {
        phoneNumber: 'TEMP',
        city: 'TEMP',
        governorate: 'TEMP',
        street: 'TEMP',
        zip: 'TEMP'
      })
    };

    return this.http.post<void>(this.apiUrl, orderDto).pipe(
      catchError(error => {
        console.error('Error adding to cart', error);
        return throwError(() => new Error('Failed to add item to cart'));
      })
    );
  }
  /** Add another item to existing cart */
  // addOrderItem(item: OrderItemDTO): Observable<void> {
  //   return this.http.post<void>(`${this.apiUrl}/OrderItem`, item);
  // }

  /** Get all items in the cart */
  getAllItems(): Observable<CartItem[]> {
    return this.http.get<CartItem[]>(this.apiUrl);
  }

  /** Remove an item */
deleteItem(orderId: number, productId: number, designId: number): Observable<void> {
  return this.http.delete<void>(
    `${this.apiUrl}/${orderId}/${productId}/${designId}`
  );
}

  // getCartItems() {
  //   return this.http.get<any[]>(this.apiUrl).pipe(
  //     // Filter out items with quantity 0 and map to CartItem
  //     map(items => items
  //       .filter(item => item.quantity > 0)
  //       .map(item => ({
  //         imgUrl: item.imgUrl,
  //         productName: item.productName, // Note the typo matches API
  //         unitPrice: item.unitPrice,
  //         quantity: item.quantity,
  //         designImgUrl: item.designImgUrl
  //       } as CartItem))
  //     )
  //   );
  // }
getCartItems(): Observable<CartItem[]> {
  return this.http.get<any[]>(this.apiUrl).pipe(
    map(items => items
      .filter(item => item.quantity >= 0)
      .map(item => ({
        imgUrl: item.imgUrl,
        designImgUrl: item.designImgUrl,
        productName: item.productName,
        unitPrice: item.unitPrice,
        quantity: item.quantity,
        orderId: item.orderId,
        productDetailsId: item.productDetailsId,
        designDetailsId: item.designDetailsId
      } as CartItem))
    )
  );
}


  /**
   * Smart helper: call this to add-to-cart in your product-details.
   * It will create the cart on first call, then add items thereafter.
   */
// addToCart(
//   productDetailsId: number,
//   designDetailsId: number,
//   quantity: number
// ): Observable<void> {
//   const saved = localStorage.getItem('cartId');
//   const orderId = saved ? Number(saved) : 0;

//   if (orderId === 0) {
//     // First item - create cart with minimal shipping info
//     const orderDto: OrderDTO = {
//       phoneNumber: 'TEMP',  // Temporary required value
//       city: 'TEMP',
//       governorate: 'TEMP',
//       street: 'TEMP',
//       zip: 'TEMP',
//       productDetailsId,
//       designDetailsId,
//       cartItem
//     };
//       return this.addOrder(orderDto).pipe(
//         switchMap(() => this.getAllItems()),
//         switchMap(items => {
//           if (items.length > 0) {
//             const newOrderId = items[0].orderId;
//             localStorage.setItem('cartId', String(newOrderId));
            
//             // Now add the first item with proper quantity
//             return this.addOrderItem({
//               orderId: newOrderId,
//               productDetailsId,
//               designDetailsId,
//               quantity
//             });
//           }
//           throw new Error('Cart creation failed');
//         }),
//         mapTo(undefined)
//     );
//   } else {
//     // Existing cart
//     return this.addOrderItem({
//       orderId,
//       productDetailsId,
//       designDetailsId,
//       quantity
//     });
//   }
// }
}
