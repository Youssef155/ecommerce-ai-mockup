import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartService } from '../../core/services/cart.service';
import { OrderItem } from '../../core/models/order-item';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart.component.html'
})
export class CartComponent implements OnInit {
  private cartService = inject(CartService);
  items: OrderItem[] = [];
  isLoading = false;

  ngOnInit() {
    this.loadCart();
  }

  loadCart() {
    this.isLoading = true;
    this.cartService.getCart().subscribe(items => {
      this.items = items;
      this.isLoading = false;
    });
  }

  increase(productId: number) {
    this.cartService.increaseQuantity(productId).subscribe(() => this.loadCart());
  }

  decrease(productId: number) {
    this.cartService.decreaseQuantity(productId).subscribe(() => this.loadCart());
  }

  checkout() {
    this.cartService.checkout().subscribe(res => {
      window.location.href = res.redirectUrl;
    });
  }
}