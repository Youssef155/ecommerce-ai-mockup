import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-canvas-editor',
 standalone: true, 
  imports: [CommonModule],
  templateUrl: './canvas-editor.component.html',
  styleUrl: './canvas-editor.component.css'
})
export class CanvasEditorComponent {
    @ViewChild('canvas') canvasRef!: ElementRef<HTMLCanvasElement>;
  canvas!: fabric.Canvas;
  isImageadded=false;

  ngAfterViewInit(): void {
    requestAnimationFrame(() => {
      this.canvas = new fabric.Canvas(this.canvasRef.nativeElement);
    });
  }

 addImage(): void {
  if (!this.canvas||this.isImageadded) return; //if canvas is null or the image is already added then return so the image can be added just once

  fabric.util.loadImage('/loogo.png', { crossOrigin: 'anonymous' })
    .then((imgEl) => {
      const img = new fabric.Image(imgEl, {
        scaleX: 0.4,
        scaleY: 0.4,
        originX: 'center',
        originY: 'center',
        left: this.canvas.getWidth() / 2,
        top: this.canvas.getHeight() / 2,
      });

      this.canvas.add(img);
      this.canvas.renderAll();
      this.isImageadded = true;
    })
    .catch((err) => console.error('Image load error', err));
}
}
