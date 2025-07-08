import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
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
  productDetailsId: number = 1; // Default product ID for mockup

  // Font Awesome icons
  faUpload = faUpload;
  faMagic = faMagic;
  faSpinner = faSpinner;
  faCalendarAlt = faCalendarAlt;
  faImages = faImages;
  faRobot = faRobot;
  faExclamationTriangle = faExclamationTriangle;

  constructor(private designService: DesignService, private router: Router, private route: ActivatedRoute) { }

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

  previewMockup(design: Design): void {
    let productDetailsId: number | null = null;
    let productImageUrl: string | null = null;
    this.route.queryParams.subscribe(params => {
      productDetailsId = params['productDetailsId'];
      productImageUrl = params['productImageUrl'];

      if (productDetailsId && productImageUrl) {
        this.router.navigate(['/design/mockup'], {
          relativeTo: this.route,
          queryParams: {
            designId: design.id,
            designImageUrl: design.imageUrl,
          },
          queryParamsHandling: 'merge'
        }
        );
      } else {
        alert('Please select a product first');
      }
    });
  }

  onDesignAdded(design: Design): void {
    this.designs = [design, ...this.designs];
  }
}