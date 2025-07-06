import { Injectable } from '@angular/core';
 
export interface OrderItemDTO {
  designDetailsId: number;
  productDetailsId: number;
  orderId: number;
  quantity: number;
  orderTotal: number;
}

export interface OrderDTO {
  
}
@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor() { }
}
