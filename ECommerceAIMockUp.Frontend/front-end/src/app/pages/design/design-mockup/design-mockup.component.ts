import {
  Component, AfterViewInit, ViewChild, ElementRef, HostListener,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { MockupService } from '../../../core/services/mockup.service.service';
import { MockupStateService } from '../../../core/services/mockup-state-service.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import * as fabric from 'fabric';
// 
@Component({
    selector: 'app-design-mockup',
  templateUrl: './design-mockup.component.html',
  styleUrls: ['./design-mockup.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule]
})
export class DesignMockupComponent implements OnInit, AfterViewInit {
  @ViewChild('canvasEl', { static: true }) canvasEl!: ElementRef<HTMLCanvasElement>;
  @ViewChild('wrapperRef', { static: true }) wrapperRef!: ElementRef<HTMLDivElement>;

  private mockupStateService = inject(MockupStateService);
  private cdr = inject(ChangeDetectorRef);
  private mockupService = inject(MockupService);

  canvas!: fabric.Canvas;
  designImageUrl: string | null = null;
  productImageUrl: string | null = null;
  productDetailsId: number | null = null;
  designId: number | null = null;

  canvasWidth = 0;
  canvasHeight = 0;

  x = 0;
  y = 0;
  rotation = 0;
  scaleX = .3;
  scaleY = .3;
  maxX: number = .5;
  maxY: number = .5;

  get designObject() {
    return this.mockupService.designObject;
  }

  
  ngOnInit(): void {
    this.initializeFromRoute();
  }
  
  private initializeFromRoute(): void {
    const state = this.mockupStateService.getState();
    console.log('Initializing from state:', state);
    this.productImageUrl = state.productImageUrl ?? null;
    this.designImageUrl = state.designImageUrl ?? null;
    this.designId = state.designId ?? null;
    this.productDetailsId = state.productDetailsId ?? null;
    console.log(this.productImageUrl, this.designImageUrl, this.designId, this.productDetailsId);
  }
  ngAfterViewInit(): void {
    this.setCanvasSize();
    this.initializeCanvas();
    this.setupCanvasListeners();
  }

  @HostListener('window:resize')
  onResize() {
    this.setCanvasSize();
    this.resizeCanvas();
  }

  private setCanvasSize() {
    const wrapper = this.wrapperRef.nativeElement;
    this.canvasWidth = wrapper.clientWidth;
    this.canvasHeight = wrapper.clientHeight;
  }

  private initializeCanvas() {
    this.canvas = new fabric.Canvas(this.canvasEl.nativeElement, {
      backgroundColor: '#fff',
      preserveObjectStacking: true,
    });
    this.mockupService.canvas = this.canvas;
    this.resizeCanvas();
  }

  private async resizeCanvas() {
    const currentX = this.x;
    const currentY = this.y;
    this.canvas.setDimensions({
      width: this.canvasWidth,
      height: this.canvasHeight,
    });

    this.canvas.clear();
    await this.mockupService.renderTshirt(this.productImageUrl!, this.canvasWidth, this.canvasHeight);
    await this.mockupService.renderDesign(this.designImageUrl!, this.scaleX, this.scaleY, this.rotation);
    this.x = currentX;
    this.y = currentY;
    this.updatePosition();
  }

   private setupCanvasListeners() {
    const syncObjectState = () => {
  const obj = this.designObject;
  const tshirt = this.mockupService.tshirtObject;
  if (!obj || !tshirt) return;

  // T-shirt bounds in canvas coordinates
  const bounds = tshirt.getBoundingRect();
  const objBounds = obj.getBoundingRect();

  // Calculate relative position (center-based)
  const xRaw = ((obj.left ?? 0) - (bounds.left + bounds.width / 2)) / bounds.width;
  const yRaw = ((obj.top ?? 0) - (bounds.top + bounds.height / 2)) / bounds.height;

  this.x = parseFloat(xRaw.toFixed(3));
  this.y = parseFloat(yRaw.toFixed(3));

  // Calculate relative scale
  const visualScaleX = obj.scaleX ?? 1;
  const visualScaleY = obj.scaleY ?? 1;

  const logicalScaleX = (visualScaleX * obj.width!) / (this.mockupService.tshirtOriginalWidth * this.mockupService.tshirtScaleFactor);
  const logicalScaleY = (visualScaleY * obj.height!) / (this.mockupService.tshirtOriginalHeight * this.mockupService.tshirtScaleFactor);

  this.scaleX = parseFloat(logicalScaleX.toFixed(3));
  this.scaleY = parseFloat(logicalScaleY.toFixed(3));
  this.rotation = Math.round(obj.angle ?? 0);

  // 🆕 Calculate allowed X/Y range based on design size relative to T-shirt
  const objWidthRatio = objBounds.width / bounds.width;
  const objHeightRatio = objBounds.height / bounds.height;

  this.maxX = parseFloat((0.5 - objWidthRatio / 2).toFixed(3));
  this.maxY = parseFloat((0.5 - objHeightRatio / 2).toFixed(3));

  this.cdr.detectChanges();
};

    this.canvas.on('object:moving', syncObjectState);
    this.canvas.on('object:scaling', syncObjectState);
    this.canvas.on('object:rotating', syncObjectState);
    this.canvas.on('object:modified', syncObjectState);
    this.canvas.on('selection:created', syncObjectState);
    this.canvas.on('selection:updated', syncObjectState);
  }

  updateScale() {
    const design = this.designObject;
    if (!design) return;

    const factor = this.mockupService.tshirtScaleFactor;
    const width = this.mockupService.tshirtOriginalWidth;
    const height = this.mockupService.tshirtOriginalHeight;

    design.set({
      scaleX: (width * this.scaleX * factor) / design.width!,
      scaleY: (height * this.scaleY * factor) / design.height!,
    });
    design.setCoords();
    this.canvas.renderAll();
  }

  updateRotation() {
    if (!this.designObject) return;
    this.designObject.rotate(this.rotation);
    this.designObject.setCoords();
    this.canvas.renderAll();
  }

  updatePosition() {
    const obj = this.designObject;
    const tshirt = this.mockupService.tshirtObject;
    if (!obj || !tshirt) return;

    const bounds = tshirt.getBoundingRect();

    const left = bounds.left + bounds.width / 2 + this.x * bounds.width;
    const top = bounds.top + bounds.height / 2 + this.y * bounds.height;

    obj.set({ left, top });
    this.mockupService.enforceBounds(obj);
    obj.setCoords();
    this.canvas.renderAll();
  }

  
}
