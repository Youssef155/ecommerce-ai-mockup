import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class MockupStateService {
  private mockupData: {
    productDetailsId?: number;
    designId?: number;
    productImageUrl?: string;
    designImageUrl?: string;
  } = {};

  setState(data: {
    productDetailsId: number;
    designId: number;
    productImageUrl: string;
    designImageUrl: string;
  }) {
    this.mockupData = data;
  }

  getState() {
    return this.mockupData;
  }

  clear() {
    this.mockupData = {};
  }
}