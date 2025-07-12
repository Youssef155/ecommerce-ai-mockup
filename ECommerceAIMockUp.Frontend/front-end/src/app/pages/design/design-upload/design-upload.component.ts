import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DesignService } from '../../../core/services/design.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { 
  faCloudUploadAlt,
  faImage,
  faTimes,
  faSpinner
} from '@fortawesome/free-solid-svg-icons';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-design-upload',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, FontAwesomeModule],
  templateUrl: './design-upload.component.html',
  styleUrls: ['./design-upload.component.css']
})
export class DesignUploadComponent implements OnInit{
  selectedFile: File | null = null;
  isUploading = false;
  error: string | null = null;

  productDetailsId!: number;
  productImageUrl!: string;
  quantity: number = 1;

  // after upload
  designDetailsId?: number;


  // Font Awesome icons
  faCloudUploadAlt = faCloudUploadAlt;
  faImage = faImage;
  faTimes = faTimes;
  faSpinner = faSpinner;

  constructor(
    private designService: DesignService,
    private router: Router,
    private route: ActivatedRoute,
    private cartService: CartService
  ) { }
  ngOnInit(): void {
    const qp = this.route.snapshot.queryParamMap;
    this.productDetailsId = Number(qp.get('productDetailsId'));
    this.productImageUrl  = qp.get('productImageUrl') || ''; 

    const qtyParam = qp.get('quantity');
    this.quantity = qtyParam ? Number(qtyParam) : 1;
   }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  uploadDesign(): void {
    if (!this.selectedFile) {
      this.error = 'Please select a file first';
      return;
    }

    this.isUploading = true;
    this.error = null;

    this.designService.uploadDesign(this.selectedFile).subscribe({
      next: (res: { designDetailsId: number }) => {
        this.isUploading = false;
        this.designDetailsId = res.designDetailsId; // store the ID for adding to cart
              this.router.navigate(['/design/mockup'], {
        queryParams: {
          designId: this.designDetailsId,
          productDetailsId: this.productDetailsId,
          quantity: this.quantity,
          designImageUrl: URL.createObjectURL(this.selectedFile!),
          productImageUrl: this.productImageUrl
        }
      });
      },
      

      error: (err) => {
        this.error = `Failed to upload design, ` + (err.error || 'Please try again.');
        this.isUploading = false;
      }
    });
  }
//     addDesignToCart(): void {
//       if (!this.designDetailsId) {
//         this.error = 'Please upload your design first.';
//         return;
//       }

//       this.cartService.addToCart(
//         this.productDetailsId,
//         this.designDetailsId,
//         this.quantity
//       ).subscribe({
//         next: () => this.router.navigate(['/cart']),
//         error: (err) => {
//           console.error('Add to cart failed', err);
//           this.error = 'Failed to add to cart. Please try again.';
//         }
//       });
// }
}