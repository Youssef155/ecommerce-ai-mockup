import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/Products/Product';
import { ProductVariationService } from '../../../core/services/product-variation.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CurrencyPipe } from '@angular/common';
import { ColorSelectorComponent } from '../../color-selector/color-selector.component';
import { SizeSelectorComponent } from '../../size-selector/size-selector.component';
import { Router } from '@angular/router';
import { CartService } from '../../../core/services/cart.service';

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
  selectedColor: string | null = null;
  availableColors: string[] = [];
  isLoading = true;
  error: string | null = null;

   quantityOptions = Array.from({ length: 10 }, (_, i) => i + 1);
  selectedQuantity = 1;

  constructor(
    private productService: ProductService,
    private productVariationService: ProductVariationService,
    private route: ActivatedRoute,
    private router: Router,
    private cartService: CartService
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
  }

  addToCart() { //flag
    if (!this.product || !this.selectedSize || !this.selectedColor) return;
    this.cartService
  .addToCart(
    this.product.id,
    this.product., //this should be the design Id, it will be handled with the design service
    this.selectedQuantity
  )
  .subscribe(() => alert('Added to cart!'));
    alert(`Added to cart: ${this.product.name} - Size: ${this.selectedSize}, Color: ${this.selectedColor}`);
  }

  goToDesign() {
    if (!this.product) return;
    const productDetailsId = this.product.id;
    const imgUrl = this.product.image;
    this.router.navigate(['/design'], {
      queryParams: {
        productDetailsId,
        imgUrl
      }
    });
  }
}