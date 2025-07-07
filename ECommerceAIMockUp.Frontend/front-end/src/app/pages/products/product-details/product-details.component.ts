import { Component, Input, signal } from '@angular/core';
import { Product } from '../../../core/models/Products/Product';
import { ProductVariationService } from '../../../core/services/product-variation.service';
import { ProductVariation } from '../../../core/models/Products/product-variation';
import { CommonModule } from '@angular/common';
import { CurrencyPipe } from '@angular/common';
import { SizeSelectorComponent } from '../size-selector/size-selector.component';
import { ColorSelectorComponent } from '../color-selector/color-selector.component';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [SizeSelectorComponent, ColorSelectorComponent, CurrencyPipe],
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent {
  @Input() product: Product | null = null;
  
  selectedSize: string | null = null;
  selectedColor: string | null = null;
  availableColors: string[] = [];
  isLoadingColors = false;

  constructor(private productVariationService: ProductVariationService) {}

  onSizeSelect(size: string) {
    if (!this.product) return;
    
    this.isLoadingColors = true;
    this.selectedSize = size;
    this.selectedColor = null;
    
    this.productVariationService.getColorsBySize(this.product.id, size)
      .subscribe({
        next: (variation) => {
          this.availableColors = variation.availableColors;
          this.isLoadingColors = false;
        },
        error: () => this.isLoadingColors = false
      });
  }

  onColorSelect(color: string) {
    this.selectedColor = color;
  }
}