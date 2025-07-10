import { Component, OnInit } from '@angular/core';
import { CartService, OrderItemDTO } from '../../core/services/cart.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  items: OrderItemDTO[] = [];
  isLoading = false;
  error: string | null = null;

  constructor(private cartService: CartService) {}
 
  ngOnInit() {
    this.loadCart();
  }

  loadCart() {
    this.isLoading = true;
    this.error = null;

    this.cartService.getAllItems().subscribe({
      next: items => {
        this.items = items;
        this.isLoading = false;
      },
      error: err => {
        console.error('Failed to load cart', err);
        this.error = 'Failed to load cart';
        this.isLoading = false;
      }
    });
  }

remove(item: OrderItemDTO) {
  // Use composite key for deletion
  this.cartService.deleteItem(
    item.orderId, 
    item.productDetailsId, 
    item.designDetailsId
  ).subscribe({
    next: () => this.loadCart(),
    error: err => {
      console.error('Failed to remove item', err);
      this.error = 'Failed to remove item. Please try again.';
    }
  });
}
}
