import { Injectable } from '@angular/core';
import * as fabric from 'fabric';
import { FabricImage } from 'fabric';


@Injectable({ providedIn: 'root' })
export class MockupService {
  canvas!: fabric.Canvas;
  tshirtObject!: fabric.Image;
  designObject!: fabric.Image;
  tshirtBounds: { left: number; top: number; width: number; height: number } | null = null;
  tshirtOriginalWidth: number = 0;
  tshirtOriginalHeight: number = 0;
  scaleX: number = 1;
  scaleY: number = 1;
  tshirtScaleFactor: number = 1;

  initCanvas(canvasEl: HTMLCanvasElement, width: number, height: number): fabric.Canvas {
    this.canvas = new fabric.Canvas(canvasEl, {
      width,
      height,
      backgroundColor: '#f9f9f9'
    });
    return this.canvas;
  }

  async renderTshirt(productImageUrl: string, canvasWidth: number, canvasHeight: number): Promise<void> {
  const img = await FabricImage.fromURL(productImageUrl);
  img.selectable = false;

    this.tshirtOriginalWidth = img.width!;
    this.tshirtOriginalHeight = img.height!;

    const canvasRatio = canvasWidth / canvasHeight;
    const tshirtRatio = img.width! / img.height!;

    let scaleFactor: number;

    if (tshirtRatio > canvasRatio) {
      scaleFactor = (canvasWidth * 0.9) / img.width!;
      img.scaleToWidth(canvasWidth * 0.9);
    } else {
      scaleFactor = (canvasHeight * 0.9) / img.height!;
      img.scaleToHeight(canvasHeight * 0.9);
    }

    img.left = (canvasWidth - img.getScaledWidth()) / 2;
    img.top = (canvasHeight - img.getScaledHeight()) / 2;

    this.canvas.add(img);
    this.canvas.moveObjectTo(img as fabric.Object, 0);
    img.setCoords();

    this.tshirtObject = img;
    this.tshirtScaleFactor = scaleFactor;
    this.tshirtBounds = img.getBoundingRect();
}

async renderDesign(
  designImageUrl: string, logicalScaleX: number, logicalScaleY: number, rotation: number
): Promise<void> {
  const img = await FabricImage.fromURL(designImageUrl);
  img.set({
      originX: 'center',
      originY: 'center',
      lockUniScaling: true,
      hasControls: true,
    });

    const factor = this.tshirtScaleFactor;
    const width = this.tshirtOriginalWidth;
    const height = this.tshirtOriginalHeight;

    img.scaleX = (width * logicalScaleX * factor) / img.width!;
    img.scaleY = (height * logicalScaleY * factor) / img.height!;

    if (this.tshirtBounds) {
      img.left = this.tshirtBounds.left + this.tshirtBounds.width / 2;
      img.top = this.tshirtBounds.top + this.tshirtBounds.height / 2;
    }

    img.rotate(rotation);

    this.designObject = img;
    this.canvas.add(img);
    this.canvas.setActiveObject(img);
    this.addBoundingLogic();
}

  enforceBounds(obj: fabric.Object) {
    if (!this.tshirtBounds || !obj) return;

    const bounds = this.tshirtBounds;

    obj.setCoords();
    const objBounds = obj.getBoundingRect();

    const deltaX =
      objBounds.left < bounds.left
        ? bounds.left - objBounds.left
        : objBounds.left + objBounds.width > bounds.left + bounds.width
        ? (bounds.left + bounds.width) - (objBounds.left + objBounds.width)
        : 0;

    const deltaY =
      objBounds.top < bounds.top
        ? bounds.top - objBounds.top
        : objBounds.top + objBounds.height > bounds.top + bounds.height
        ? (bounds.top + bounds.height) - (objBounds.top + objBounds.height)
        : 0;

    obj.left = (obj.left ?? 0) + deltaX;
    obj.top = (obj.top ?? 0) + deltaY;
    obj.setCoords();
  }

  addBoundingLogic() {
    if (!this.canvas || !this.tshirtBounds || !this.designObject) return;

    const obj = this.designObject;

    this.canvas.on('object:moving', () => this.enforceBounds(obj));
    this.canvas.on('object:scaling', () => this.enforceBounds(obj));
    this.canvas.on('object:rotating', () => this.enforceBounds(obj));
  }

}
