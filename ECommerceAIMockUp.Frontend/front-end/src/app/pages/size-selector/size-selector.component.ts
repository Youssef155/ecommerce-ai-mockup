import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-size-selector',
  standalone: true,
  template: `
    <div class="size-selector">
      <h3>Size</h3>
      <div class="size-options">
        @for (size of sizes(); track size) {
          <button 
            [class.selected]="selectedSize === size"
            (click)="selectSize(size)">
            {{ size }}
          </button>
        }
      </div>
    </div>
  `,
  styles: [`
    .size-options {
      display: flex;
      gap: 10px;
      margin-top: 10px;
    }
    .size-options button {
      padding: 8px 15px;
      border: 1px solid #ddd;
      background: white;
      cursor: pointer;
    }
    .size-options button.selected {
      background: #333;
      color: white;
    }
  `]
})
export class SizeSelectorComponent {
  sizes = input<string[]>([]);
  sizeSelected = output<string>();
  selectedSize: string | null = null;

  selectSize(size: string) {
    this.selectedSize = size;
    this.sizeSelected.emit(size);
  }
}