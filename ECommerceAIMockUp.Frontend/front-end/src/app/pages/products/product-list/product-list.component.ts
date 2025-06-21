import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { ProductService } from '../../../core/services/product.service';
import { DecodedToken } from '../../../core/models/TokenData';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product',
  templateUrl: './product-list.component.html',
  imports: [CommonModule],
  styleUrls: ['./product-list.component.css']
})
export class ProductComponent implements OnInit {
  user: DecodedToken | null = null;
  message: string = '';
  products: any[] = [];

  constructor(
    private authService: AuthService,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.authService.user$.subscribe(user => {
      this.user = user;
    });

    this.authService.token$.subscribe(token => {
      if (token) {
        this.productService.getProducts().subscribe({
          next: (res: any) => {
            this.products = res;
            this.message = '';
          },
          error: (err) => {
            this.message = err.status === 401 ? 'Unauthorized' : 'Error loading products';
          }
        });
      }
    });
  }
}
