import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { DesignService } from '../../../core/services/design.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { 
  faCloudUploadAlt,
  faImage,
  faTimes,
  faSpinner
} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-design-upload',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, FontAwesomeModule],
  templateUrl: './design-upload.component.html',
  styleUrls: ['./design-upload.component.css']
})
export class DesignUploadComponent {
  selectedFile: File | null = null;
  isUploading = false;
  error: string | null = null;

  // Font Awesome icons
  faCloudUploadAlt = faCloudUploadAlt;
  faImage = faImage;
  faTimes = faTimes;
  faSpinner = faSpinner;

  constructor(
    private designService: DesignService,
    private router: Router,
  ) { }

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
      next: (res) => {
        this.isUploading = false;
        this.router.navigate(['/design']);
      },
      error: (err) => {
        this.error = `Failed to upload design, ` + (err.error || 'Please try again.');
        this.isUploading = false;
      }
    });
  }
}