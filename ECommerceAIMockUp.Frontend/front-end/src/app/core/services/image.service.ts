import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  private readonly APIUrl = 'https://localhost:7256'; 

  getFullUrl(path: string): string {
    // 1. Handle empty paths
    if (!path) return 'assets/default-product.png';
    
    // 2. Return full URLs as-is
    if (path.startsWith('http')) return path;
    
    // 3. Construct full URL from base API URL
    // Remove leading slashes to prevent double slashes
    const cleanPath = path.replace(/^\/+/, '');
    return `${this.APIUrl}/${cleanPath}`;
  }

  // Additional useful methods could include:
  getThumbnailUrl(path: string): string {
    return this.getFullUrl(path) + '?width=300'; // Example for thumbnail generation
  }

  handleErrorImage(event: Event): void {
    const imgElement = event.target as HTMLImageElement;
    imgElement.src = 'assets/default-product.png';
    imgElement.style.objectFit = 'contain';
  }
}