import { OrderStatus } from "../shared/Enums/order-status";
import { PaymentStatus } from "../shared/Enums/payment-status";

export interface OrderItem {
   id: number;
  productId: number;
  productName: string;
  quantity: number;
  price: number;
  size?: string;
  color?: string;
}

export interface OrderUpdateStatusDto {
  id: number;
  status: OrderStatus;
}

export interface OrderUpdatePaymentDto {
  id: number;
  paymentStatus: PaymentStatus;
}
