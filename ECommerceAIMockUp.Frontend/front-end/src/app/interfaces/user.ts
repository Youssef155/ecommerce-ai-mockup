export interface User {
 id: string; 
  name: string;
  email: string;
  phone: string;
  createdAt: string;
  isActive: boolean;
  roles?: string[]; 
}
