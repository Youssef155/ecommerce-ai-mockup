import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DesignService } from '../../../core/services/design.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { 
  faMagic,
  faWandMagicSparkles,
  faImage,
  faExclamationTriangle,
  faSave,
  faTrashAlt
} from '@fortawesome/free-solid-svg-icons';
import { Design } from '../../../core/models/design.model';

@Component({
  selector: 'app-design-generate',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, FontAwesomeModule],
  templateUrl: './design-generate.component.html',
  styleUrls: ['./design-generate.component.scss']
})
export class DesignGenerateComponent {
  design: Design = { id: 0, imageUrl: '' };
  prompt = '';
  generatedImageUrl: string | null = null;
  isGenerating = false;
  isSaving = false;
  error: string | null = null;

  // Font Awesome icons
  faMagic = faMagic;
  faWandMagicSparkles = faWandMagicSparkles;
  faImage = faImage;
  faExclamationTriangle = faExclamationTriangle;
  faSave = faSave;
  faTrashAlt = faTrashAlt;

  constructor(
    private designService: DesignService,
  ) { }

  generateDesign(): void {
    if (!this.prompt.trim()) {
      this.error = 'Please enter a prompt';
      return;
    }

    this.isGenerating = true;
    this.error = null;
    this.generatedImageUrl = null;

    this.designService.generateDesign(this.prompt).subscribe({
      next: (response) => {
        this.design = response.design;
        this.isGenerating = false;
      },
      error: (err) => {
        this.error = 'Failed to generate design. Please try again.';
        this.isGenerating = false;
        console.error(err);
      }
    });
  }

  saveDesign(): void {
    if (!this.generatedImageUrl) return;

    this.isSaving = true;
    this.error = null;

    this.designService.saveGeneratedDesign(this.design).subscribe({
      next: () => {
        this.isSaving = false;
        window.history.back();
      },
      error: (err) => {
        this.error = 'Failed to save design. Please try again.';
        this.isSaving = false;
        console.error(err);
      }
    });
  }
}