import { OrderStatus } from "../shared/Enums/order-status";
import { PaymentStatus } from "../shared/Enums/payment-status";
import { OrderItem } from "./order-item";

export interface Order {
   id: number;
  userId: string;
  userName?: string;
  userEmail?: string;
  orderDate: string;
  totalAmount: number;
  status: OrderStatus;
  paymentStatus: PaymentStatus;
  items?: OrderItem[];
  shippingAddress?: string;
}
