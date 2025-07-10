import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/Products/Product';
import { ProductVariationService } from '../../../core/services/product-variation.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CurrencyPipe } from '@angular/common';
import { SizeSelectorComponent } from '../size-selector/size-selector.component';
import { ColorSelectorComponent } from '../color-selector/color-selector.component'; 
import { Router } from '@angular/router';
import { ProductDetails } from '../../../core/models/Products/product-details';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule, CurrencyPipe, SizeSelectorComponent, ColorSelectorComponent, FormsModule],
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  product: Product | null = null;
  productDetails: ProductDetails | null = null;
  selectedSize: string | null = null;
  selectedColor: string | null = null;
  availableColors: string[] = [];
  quantity: number = 1;
  isLoading = true;
  error: string | null = null;

  constructor(
    private productService: ProductService,
    private productVariationService: ProductVariationService,
    private route: ActivatedRoute,
    private router: Router
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
    this.selectedColor = null;
    this.availableColors = [];

    this.productVariationService.getColorsBySize(this.product.id, size)
      .subscribe({
        next: (colors) => {
          this.availableColors = colors;
          if (colors.length === 1) {
            this.selectedColor = colors[0];
          }
        },
        error: () => {
          this.availableColors = [];
        }
      });
  }

  onColorSelect(color: string) {
    this.selectedColor = color;

    if (this.selectedSize && this.selectedColor && this.product) {
      this.productVariationService
        .getProductVariant(this.product.id, this.selectedSize, this.selectedColor)
        .subscribe((details: ProductDetails) => {
          this.productDetails = details;
        });
    }
  }


  goToDesign() {
    if (!this.product || !this.productDetails) return;
    const productDetailsId = this.productDetails.productDetailsId;
    const productImageUrl = 'https://localhost:7256' + this.product.image;
    this.router.navigate(['/design'], {
      queryParams: {
        productDetailsId,
        productImageUrl
      }
    });
  }
}