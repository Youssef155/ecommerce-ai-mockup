export interface Category {
    id: number;
  name: string;
  description: string;
  productsCount?: number;
}
export interface CategoryCreateDto {
  name: string;
  description: string;
}

export interface CategoryUpdateDto {
  id: number;
  name: string;
  description: string;
}
