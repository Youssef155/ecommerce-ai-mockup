import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
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
  styleUrls: ['./design-list.component.scss']
})
export class DesignListComponent implements OnInit {
  designs: Design[] = [];
  loading = true;
  error: string | null = null;

  // Font Awesome icons
  faUpload = faUpload;
  faMagic = faMagic;
  faSpinner = faSpinner;
  faCalendarAlt = faCalendarAlt;
  faImages = faImages;
  faRobot = faRobot;
  faExclamationTriangle = faExclamationTriangle;

  constructor(private designService: DesignService) { }

  ngOnInit(): void {
    this.loadDesigns();
  }

  loadDesigns(): void {
    this.loading = true;
    this.error = null;
    
    this.designService.getDesigns().subscribe({
      next: (designs) => {
        this.designs = designs;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load designs. Please try again later.';
        this.loading = false;
        console.error(err);
      }
    });
  }

  onDesignAdded(design: Design): void {
    this.designs = [design, ...this.designs];
  }
}