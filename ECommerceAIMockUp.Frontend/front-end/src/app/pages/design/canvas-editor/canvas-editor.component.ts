import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import * as fabric from 'fabric';

@Component({
  selector: 'app-canvas-editor',
 standalone: true, 
  imports: [CommonModule],
  templateUrl: './canvas-editor.component.html',
  styleUrl: './canvas-editor.component.css'
})
export class CanvasEditorComponent implements AfterViewInit{
  @ViewChild('canvas') canvasRef!: ElementRef<HTMLCanvasElement>;
  canvas!: fabric.Canvas;

  isImageadded=false;

  ngAfterViewInit(): void {
    this.canvas = new fabric.Canvas(this.canvasRef.nativeElement);
  }

  addImage():void {
    if(!this.canvas || this.isImageadded)
      return;
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
