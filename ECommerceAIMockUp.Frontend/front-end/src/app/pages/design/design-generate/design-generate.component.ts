import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
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
import { GeneratedDesign } from '../../../core/models/generated-design.model';

@Component({
  selector: 'app-design-generate',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, FontAwesomeModule],
  templateUrl: './design-generate.component.html',
  styleUrls: ['./design-generate.component.css']
})
export class DesignGenerateComponent {
  generatedDesign: GeneratedDesign = { imageUrl: '', promptId: 0 };
  prompt = '';
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
    private router: Router,
    private route: ActivatedRoute
  ) { }

  generateDesign(): void {
    if (!this.prompt.trim()) {
      this.error = 'Please enter a prompt';
      return;
    }

    this.isGenerating = true;
    this.error = null;

    this.designService.generateDesign(this.prompt).subscribe({
      next: (response) => {
        console.log('Design generated successfully:', response);
        this.generatedDesign.promptId = response.promptId;
        this.generatedDesign.imageUrl = response.imageURL;
        this.isGenerating = false;
      },
      error: (err) => {
        this.error = 'Failed to generate design, ' + (err.error || 'Please try again.');
        this.isGenerating = false;
      }
    });
  }

  saveDesign(): void {
    if (!this.generatedDesign.imageUrl) return;

    this.isSaving = true;
    this.error = null;

    this.designService.saveGeneratedDesign(this.generatedDesign).subscribe({
      next: () => {
        this.isSaving = false;
        this.router.navigate(['/design'], {
          relativeTo: this.route,
          queryParamsHandling: 'preserve'
        });
      },
      error: (err) => {
        this.error = 'Failed to save design, ' + (err.error || 'Please try again.');
        this.isSaving = false;
      }
    });
  }

   discardDesign(): void {
    if (!this.generatedDesign.imageUrl) {
      this.router.navigate(['/design'], {
          relativeTo: this.route,
          queryParamsHandling: 'preserve'
        });
      return;
    }

    this.designService.discardDesign(this.generatedDesign).subscribe({
      next: () => {
        this.router.navigate(['/design'], {
          relativeTo: this.route,
          queryParamsHandling: 'preserve'
        });
      },
      error: (err) => {
        console.error('Failed to discard design:', err);
        this.router.navigate(['/design']);
      }
    });
  }

  back(): void{
    this.router.navigate(['/design'], {
          relativeTo: this.route,
          queryParamsHandling: 'preserve'
        }
  )}
}