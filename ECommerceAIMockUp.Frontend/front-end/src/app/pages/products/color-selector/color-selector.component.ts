import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-color-selector',
  standalone: true,
  template: `
    <div class="color-selector">
      <h3>Color</h3>
      <div class="color-options">
        @for (color of colors(); track color) {
          <div 
            [class.selected]="selectedColor === color"
            [style.backgroundColor]="color.toLowerCase()"
            (click)="selectColor(color)"
            [title]="color">
          </div>
        }
      </div>
    </div>
  `,
  styles: [`
    .color-options {
      display: flex;
      gap: 10px;
      margin-top: 10px;
    }
    .color-options div {
      width: 30px;
      height: 30px;
      border-radius: 50%;
      cursor: pointer;
      border: 2px solid transparent;
    }
    .color-options div.selected {
      border-color: #333;
    }
  `]
})
export class ColorSelectorComponent {
  colors = input<string[]>([]);
  colorSelected = output<string>();
  selectedColor: string | null = null;

  selectColor(color: string) {
    this.selectedColor = color;
    this.colorSelected.emit(color);
  }
}