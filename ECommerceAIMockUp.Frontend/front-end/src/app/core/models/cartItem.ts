export interface CartItem {
  imgUrl: string;
  productName: string; // Note: Matches the typo "proudctName" in API response
  unitPrice: number;
  quantity: number;
  designImgUrl: string;
  orderId: number;
    productDetailsId: number; // Add this
  designDetailsId: number; 
}