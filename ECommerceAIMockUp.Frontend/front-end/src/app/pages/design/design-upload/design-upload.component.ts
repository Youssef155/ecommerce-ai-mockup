import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
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
  styleUrls: ['./design-upload.component.scss']
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
      next: () => {
        this.isUploading = false;
        window.history.back();
      },
      error: (err) => {
        this.error = 'Failed to upload design. Please try again.';
        this.isUploading = false;
        console.error(err);
      }
    });
  }
}