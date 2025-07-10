import {
  Component, AfterViewInit, ViewChild, ElementRef, HostListener,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';
import { ActivatedRoute, Route, Router, RouterModule } from '@angular/router';
import { MockupService } from '../../../core/services/mockup.service.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import * as fabric from 'fabric';
import { DesignDetails } from '../../../core/models/design-details.model';
import { DesignService } from '../../../core/services/design.service';
import { firstValueFrom } from 'rxjs';
import { CartService } from '../../../core/services/cart.service';
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

  private cdr = inject(ChangeDetectorRef);
  private mockupService = inject(MockupService);
  private router: Router = inject(Router);
  private designService = inject(DesignService);
  private route = inject(ActivatedRoute);

  canvas!: fabric.Canvas;
  designImageUrl: string | null = null;
  productImageUrl: string | null = null;
  productDetailsId: number | null = null;
  quantity: number = 1;
constructor(
  // ... existing injections
  private cartService: CartService,
) {}
  designDetails: DesignDetails = {
    designId: 0,
    scaleX: .3,
    scaleY: .3,
    xAxis: 0,
    yAxis: 0,
    rotation: 0,
    opacity: 1,
    position: 'front'
  }

  canvasWidth = 0;
  canvasHeight = 0;
  maxX: number = .5;
  maxY: number = .5;

  get designObject() {
    return this.mockupService.designObject;
  }


  ngOnInit(): void {
    this.initializeFromRoute();
  }

  private initializeFromRoute(): void {
    this.route.queryParams.subscribe(params => {
      this.designImageUrl = params['designImageUrl'] || null;
      this.productImageUrl = params['productImageUrl'] || "https://localhost:7256/images/products/white-tshirt-n0j.png";
      this.designDetails.designId = params['designId'] ? +params['designId'] : 0;
      this.productDetailsId = params['productDetailsId'] ? +params['productDetailsId'] : 0;
      this.quantity = params['quantity'] ? +params['quantity'] : 1;
    });
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
    const currentX = this.designDetails.xAxis;
    const currentY = this.designDetails.yAxis;
    this.canvas.setDimensions({
      width: this.canvasWidth,
      height: this.canvasHeight,
    });

    this.canvas.clear();
    await this.mockupService.renderTshirt(this.productImageUrl!, this.canvasWidth, this.canvasHeight);
    await this.mockupService.renderDesign(this.designImageUrl!, this.designDetails.scaleX, this.designDetails.scaleY, this.designDetails.rotation);
    this.designDetails.xAxis = currentX;
    this.designDetails.yAxis = currentY;
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

      this.designDetails.xAxis = parseFloat(xRaw.toFixed(3));
      this.designDetails.yAxis = parseFloat(yRaw.toFixed(3));

      // Calculate relative scale
      const visualScaleX = obj.scaleX ?? 1;
      const visualScaleY = obj.scaleY ?? 1;

      const logicalScaleX = (visualScaleX * obj.width!) / (this.mockupService.tshirtOriginalWidth * this.mockupService.tshirtScaleFactor);
      const logicalScaleY = (visualScaleY * obj.height!) / (this.mockupService.tshirtOriginalHeight * this.mockupService.tshirtScaleFactor);

      this.designDetails.scaleX = parseFloat(logicalScaleX.toFixed(3));
      this.designDetails.scaleY = parseFloat(logicalScaleY.toFixed(3));
      this.designDetails.rotation = Math.round(obj.angle ?? 0);
      this.designDetails.opacity = parseFloat((obj.opacity ?? 1).toFixed(2));

      // Calculate allowed X/Y range based on design size relative to T-shirt
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
      scaleX: (width * this.designDetails.scaleX * factor) / design.width!,
      scaleY: (height * this.designDetails.scaleY * factor) / design.height!,
    });
    design.setCoords();
    this.canvas.renderAll();
  }

  updateRotation() {
    if (!this.designObject) return;
    this.designObject.rotate(this.designDetails.rotation);
    this.designObject.setCoords();
    this.canvas.renderAll();
  }

  updateOpacity() {
    const obj = this.designObject;
    if (!obj) return;

    obj.set('opacity', this.designDetails.opacity);
    this.canvas.renderAll();
  }

  updatePosition() {
    const obj = this.designObject;
    const tshirt = this.mockupService.tshirtObject;
    if (!obj || !tshirt) return;

    const bounds = tshirt.getBoundingRect();

    const left = bounds.left + bounds.width / 2 + this.designDetails.xAxis * bounds.width;
    const top = bounds.top + bounds.height / 2 + this.designDetails.yAxis * bounds.height;

    obj.set({ left, top });
    this.mockupService.enforceBounds(obj);
    obj.setCoords();
    this.canvas.renderAll();
  }

  onCancel() {
    const confirmCancel = confirm("Are you sure you want to cancel? Your changes won't be saved.");
    if (confirmCancel) {
      this.router.navigate(['/design']);
    }
  }

  async onAddToCart() {
       const response = await firstValueFrom(
      this.designService.addDesignDetails(this.designDetails)
    );
    console.log(response);
    
    // Get designDetailsId from response
    const designDetailsId = response.result;
  try {
    // Save design details first
 
    
    if (!designDetailsId) {
      throw new Error('Design details ID not found in response');
    }    
    // Add to cart
    if (this.productDetailsId) {
      this.cartService.addToCart(
        this.productDetailsId,
        designDetailsId,
        this.quantity
      ).subscribe({
        next: () => this.router.navigate(['/cart']),
        error: (err) => {
          console.error('Add to cart failed', err);
          alert('Failed to add to cart. Please try again.');
        }
      });
    } else {
      throw new Error('Product details ID is missing');
    }
  } catch (err) {
     console.log(this.productDetailsId,designDetailsId),
    console.error(err);
    alert('Something went wrong. Please try again.');
  }
}


}
