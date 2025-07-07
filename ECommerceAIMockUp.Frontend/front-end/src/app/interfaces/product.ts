export interface Product {
 id: number;
  name: string;
  price: number;
  description: string;
  category: string;
  stock: number;
  imageUrl?: string;
  colors?: string[];
  sizes?: string[];
  season?: string;
  gender?: string;
  categoryId?: number;

}
