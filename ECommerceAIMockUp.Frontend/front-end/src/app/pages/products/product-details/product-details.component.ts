import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/Products/Product';
import { ProductVariationService } from '../../../core/services/product-variation.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CurrencyPipe } from '@angular/common';
import { SizeSelectorComponent } from '../size-selector/size-selector.component';
import { ColorSelectorComponent } from '../color-selector/color-selector.component'; 

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule, CurrencyPipe, SizeSelectorComponent, ColorSelectorComponent],
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  product: Product | null = null;
  selectedSize: string | null = null;
  availableColors: string[] = [];
  isLoading = true;
  error: string | null = null;

  constructor(
    private productService: ProductService,
    private productVariationService: ProductVariationService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    const productId = this.route.snapshot.paramMap.get('id');
    if (productId) {
      this.loadProductDetails(+productId);
    }
  }

  loadProductDetails(id: number) {
    this.isLoading = true;
    this.productService.getProduct(id).subscribe({
      next: (product) => {
        this.product = product;
        this.isLoading = false;
        console.log(product)
      },
      error: (err) => {
        this.error = 'Failed to load product details';
        this.isLoading = false;
        console.error('Error loading product:', err);
      }
    });
  }

  onSizeSelect(size: string) {
      if (!this.product) return;
      
      this.selectedSize = size;
      this.availableColors = [];
      
      this.productVariationService.getColorsBySize(this.product.id, size)
        .subscribe({
          next: (colors) => {
            this.availableColors = colors;
            console.log('Size:', size, 'Colors:', colors);
          },
          error: () => {
            this.availableColors = [];
            console.warn('Failed to load colors for size:', size);
          }
        });
  }

}