import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { DesignService } from '../../../core/services/design.service';
import { Design } from '../../../core/models/design.model';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { 
  faUpload, 
  faMagic, 
  faSpinner, 
  faCalendarAlt, 
  faImages, 
  faRobot,
  faExclamationTriangle
} from '@fortawesome/free-solid-svg-icons';
import { MockupStateService } from '../../../core/services/mockup-state-service.service';

@Component({
  selector: 'app-design-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FontAwesomeModule],
  templateUrl: './design-list.component.html',
  styleUrls: ['./design-list.component.css']
})
export class DesignListComponent implements OnInit {
  designs: Design[] = [];
  loading = true;
  error: string | null = null;
  productImageUrl: string = 'https://localhost:7256/images/products/white-tshirt-n0j.png'; // Default product image URL
  productDetailsId: number = 1; // Default product ID for mockup

  // Font Awesome icons
  faUpload = faUpload;
  faMagic = faMagic;
  faSpinner = faSpinner;
  faCalendarAlt = faCalendarAlt;
  faImages = faImages;
  faRobot = faRobot;
  faExclamationTriangle = faExclamationTriangle;

  constructor(private designService: DesignService, private router: Router, private mockupStateService: MockupStateService) { }

  ngOnInit(): void {
    this.loadDesigns();
  }

  loadDesigns(): void {
    this.loading = true;
    this.error = null;
    
    this.designService.getDesigns().subscribe({
      next: (res) => {
        this.designs = res.result || [];
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load designs, ' + (err.error || 'Please try again.');
        this.loading = false;
      }
    });
  }

  setMockupData(
  productDetailsId: number,
  designId: number,
  productImageUrl: string,
  designImageUrl: string
  ): void {
  this.mockupStateService.setState({
    productDetailsId,
    designId,
    productImageUrl,
    designImageUrl,
  });
}

  previewMockup(design: Design): void {
  // const productId = this.route.snapshot.queryParams['productId'];
  const productId : number = 1; 

  
  if (productId) {
    this.router.navigate(['/designs/mockup', productId, design.id], {
      state: {
        designImageUrl: design.imageUrl,
        productImageUrl: this.productImageUrl,
      }
    });
  } else {
    alert('Please select a product first');
  }
}

  onDesignAdded(design: Design): void {
    this.designs = [design, ...this.designs];
  }
}