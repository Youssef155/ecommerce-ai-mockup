// product-normalizer.service.ts
import { Injectable } from '@angular/core';
import { Product } from '../models/Products/Product';

@Injectable({
  providedIn: 'root'
})
export class ProductNormalizerService {
  normalizeProduct(product: any): Product {
    return {
      id: product.id,
      name: product.name,
      descriptions: product.description,
      price: product.price,
      image: product.imgUrl || product.image,
      availableSizes: product.availableSizes,
      gender: product.gender,
      season: product.season,
      categoryName: product.categoryName
    };
  }
}