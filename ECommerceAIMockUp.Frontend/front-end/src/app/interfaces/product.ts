export interface Product {
  id: number;
  name: string;
  price: number;
  description: string;
  categoryName: string; 
  stock: number;
  imageUrl?: string;
  colors?: string[];
  sizes?: string[];
  season?: string; 
  gender?: string;
  categoryId?: number;
}
export interface GetAllProductsDto {
  Id: number;
  Name: string;
  Price: number;
  Description: string;
  CategoryName: string;
  Image?: string;
  Gender?: number;
  Season?: number;
}
