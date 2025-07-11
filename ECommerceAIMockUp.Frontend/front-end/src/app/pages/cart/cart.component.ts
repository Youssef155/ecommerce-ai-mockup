import { Component, OnInit } from '@angular/core';
import { CartService } from '../../core/services/cart.service';
import { CartItem } from '../../core/models/cartItem';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
  imports: [CommonModule]
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = [];
  isLoading = false;
  error: string | null = null;
  orderTotal: number = 0; // Add this property

  constructor(private cartService: CartService) {}
 
  ngOnInit() {
    // this.cartService.getCartItems().subscribe(items => {
    //   this.cartItems = items;
    // });
    // this.isLoading = true;
    // this.error = null;

    // this.cartService.getCartItems().subscribe({
    //   next: items => {
    //     this.cartItems = items;
    //     this.isLoading = false;
    //     console.log('Cart items loaded:', items); // Debugging
    //   },
    //   error: err => {
    //     console.error('Failed to load cart', err);
    //     this.error = 'Failed to load cart. Please try again.';
    //     this.isLoading = false;
    //   }
    // });
    this.loadCart();
  }
   loadCart() {
    this.isLoading = true;
    this.error = null;

    this.cartService.getCartItems().subscribe({
      next: items => {
        this.cartItems = items;
        this.calculateOrderTotal(); // Calculate order total
        this.isLoading = false;
      },
      error: err => {
        console.error('Failed to load cart', err);
        this.error = 'Failed to load cart. Please try again.';
        this.isLoading = false;
      }
    });
  }

  // Add this method to calculate order total
  calculateOrderTotal() {
    this.orderTotal = this.cartItems.reduce(
      (total, item) => total + (item.unitPrice * item.quantity), 
      0
    );
  }
  removeItem(item: CartItem) {
    if (!item.orderId || !item.productDetailsId || !item.designDetailsId) {
      console.error('Missing required IDs for deletion');
      return;
    }

    if (!confirm('Are you sure you want to remove this item from your cart?')) {
      return;
    }

    this.cartService.deleteItem(
      item.orderId, 
      item.productDetailsId, 
      item.designDetailsId
    ).subscribe({
      next: () => {
        console.log('Item removed successfully');
        // Refresh the cart after deletion
        this.loadCart();
      },
      error: err => {
        console.error('Failed to remove item', err);
        this.error = 'Failed to remove item. Please try again.';
      }
    });
  }
  // loadCart() {
  //   this.isLoading = true;
  //   this.error = null;

  //   this.cartService.getAllItems().subscribe({
  //     next: items => {
  //       this.cartItems = items;
  //       this.isLoading = false;
  //     },
  //     error: err => {
  //       console.error('Failed to load cart', err);
  //       this.error = 'Failed to load cart';
  //       this.isLoading = false;
  //     }
  //   });
  // }

// remove(item: CartItem) {
//   // Use composite key for deletion
//   this.cartService.deleteItem(
//     item.orderId, 
//     item.productDetailsId, 
//     item.designDetailsId
//   ).subscribe({
//     next: () => this.loadCart(),
//     error: err => {
//       console.error('Failed to remove item', err);
//       this.error = 'Failed to remove item. Please try again.';
//     }
//   });
// }
}
