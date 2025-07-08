import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/Products/Product';
import { ImageService } from '../../../core/services/image.service';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductComponent implements OnInit {
  products : Product[] = [];
  selectedProduct = signal<Product | undefined>(undefined);
  currentPage = 1;
  totalpages = 1;
  loading = false;
  error = '';

  constructor(private productService: ProductService, private router: Router, public imageService: ImageService) {}

categories = [
  { id: 1, name: 'Tops' },
  { id: 2, name: 'Jeans' },
  { id: 3, name: 'Joggers' },
  // Add all your categories here
];


selectedGenders: string[] = [];
selectedSeasons: string[] = [];
selectedCategoryId: number | null = null;



  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
   this.loading = true;
   this.error = '';
   this.productService.getProducts(this.currentPage).subscribe({
    next:(res) =>{
      this.products = res.data.data;
      this.currentPage = res.data.currentPage;
      this.totalpages = res.data.totalPages;
      this.loading = false;
      console.log(res);
    },
      error: (err) => {
        this.loading = false;
        this.error = 'Failed to load products';
        console.error(err);
      }
   })
  }

  toggleGender(gender: string, event: any) {
    if (event.target.checked) {
      this.selectedGenders.push(gender);
    } else {
      this.selectedGenders = this.selectedGenders.filter(g => g !== gender);
    }
    this.loadFilteredProducts();
  }

  toggleSeason(season: string, event: any) {
    if (event.target.checked) {
      this.selectedSeasons.push(season);
    } else {
      this.selectedSeasons = this.selectedSeasons.filter(s => s !== season);
    }
    this.loadFilteredProducts();
  }

  selectCategory(categoryId: number | null) {
    this.selectedCategoryId = categoryId;
    this.loadFilteredProducts();
  }


  loadFilteredProducts(page: number = 1) {
    this.loading = true;
    this.error = '';

    this.productService
      .getFilteredProducts(this.currentPage, this.selectedGenders, this.selectedSeasons)
      .subscribe({
        next: (response) => {
          this.products = response.data.data;
          this.totalpages = response.data.totalPages;
          this.loading = false;
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Failed to load filtered products';
          console.error(err);
        }
      });
  }


  get pageNumbers(): number[] {
    return Array.from({ length: this.totalpages }, (_, i) => i + 1);
  }



  goToPage(page: number): void {
    if (page !== this.currentPage) {
      this.currentPage = page;

      const isFilterActive =
        this.selectedGenders.length > 0 ||
        this.selectedSeasons.length > 0 ||
        this.selectedCategoryId !== null;

      if (isFilterActive) {
        this.loadFilteredProducts();
      } else {
        this.loadProducts();
      }
    }
  }
}