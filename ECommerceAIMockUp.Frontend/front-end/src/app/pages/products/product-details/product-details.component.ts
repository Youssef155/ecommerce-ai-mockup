import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../core/services/product.service';


@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html'
})
export class ProductDetailsComponent implements OnInit {
  productId = 1; // Example; use dynamic route param in real case
  product: any;
  selectedSize: string = '';
  colors: string[] = [];
  selectedColor: string = '';
  variant: any;

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(): void {
    this.productService.getProduct(this.productId).subscribe({
      next: (data) => this.product = data,
      error: () => console.error('Product not found')
    });
  }

  onSizeSelect(size: string): void {
    this.selectedSize = size;
    this.productService.getColorsBySize(this.productId, size).subscribe({
      next: (colors) => this.colors = colors,
      error: () => this.colors = []
    });
  }

  onColorSelect(color: string): void {
    this.selectedColor = color;
    this.productService.getVariant(this.productId, this.selectedSize, color).subscribe({
      next: (variant) => this.variant = variant,
      error: () => this.variant = null
    });
  }
}
