import { Gender } from "../shared/Enums/gender";
import { Season } from "../shared/Enums/season";

export interface Product {
  id: number;
  name: string;
  price: number;
  description: string;
  categoryName: string; 
  stock: number;
  imageUrl?: string;
  color?: string;
  size?: string;
  gender: Gender; 
  season: Season; 
  categoryId?: number;
}

