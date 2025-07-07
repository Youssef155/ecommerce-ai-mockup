import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../../interfaces/user';
import { Product } from '../../interfaces/product';
import { Category } from '../../interfaces/category';
import { OrderStatus } from '../../shared/Enums/order-status';
import { PaymentStatus } from '../../shared/Enums/payment-status';
import { Order } from '../../interfaces/order';
import { ApiResponse } from '../../interfaces/api-response';
import { OrderUpdatePaymentDto, OrderUpdateStatusDto } from '../../interfaces/order-item';



@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, FormsModule],
  standalone: true,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})


export class AdminDashboardComponent implements OnInit {
  activeTab = 'dashboard';
  isSidebarOpen = true;
  loading = false;
  searchTerm = '';
  showModal = false;
  modalType: string | null = null;
  selectedItem: any = null;
  newItem: any = {};

  users: User[] = [];
  products: Product[] = [];
  categories: Category[] = [];
  orders: Order[] = [];

  filteredUsers: User[] = [];
  filteredProducts: Product[] = [];
  filteredCategories: Category[] = [];
  filteredOrders: Order[] = [];

  private apiUrl = 'https://localhost:7256/api';

  OrderStatus = OrderStatus;
  PaymentStatus = PaymentStatus;

  menuItems = [
    { id: 'dashboard', label: 'Dashboard', icon: 'fa-solid fa-gauge' },
    { id: 'users', label: 'Users', icon: 'fa-solid fa-users' },
    { id: 'products', label: 'Products', icon: 'fa-solid fa-box' },
    { id: 'categories', label: 'Categories', icon: 'fa-solid fa-tags' },
    { id: 'orders', label: 'Orders', icon: 'fa-solid fa-shopping-cart' }
  ];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadDashboardData();
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  setActiveTab(tab: string) {
    this.activeTab = tab;
    this.searchTerm = '';
    this.loadDataForTab(tab);
  }

  private loadDashboardData() {
    this.fetchUsers();
    this.fetchProducts();
    this.fetchCategories();
    this.fetchOrders();
  }

  private loadDataForTab(tab: string) {
    switch (tab) {
      case 'users':
        this.fetchUsers();
        break;
      case 'products':
        this.fetchProducts();
        break;
      case 'categories':
        this.fetchCategories();
        break;
      case 'orders':
        this.fetchOrders();
        break;
    }
  }

  private getAuthHeaders(isMultipart: boolean = false): HttpHeaders {
    const token = localStorage.getItem('token');
    let headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    if (!isMultipart) {
      headers = headers.set('Content-Type', 'application/json');
    }
    return headers;
  }

  private fetchUsers() {
    this.loading = true;
    this.http.get<User[]>(`${this.apiUrl}/Users/admin`, {
      headers: this.getAuthHeaders()
    }).subscribe({
      next: (data) => {
        this.users = Array.isArray(data) ? data : [];
        this.filteredUsers = this.users;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching users:', error);
        this.loading = false;
        this.handleError(error);
      }
    });
  }

  private fetchProducts() {
    this.loading = true;
    this.http.get<any>(`${this.apiUrl}/Product/products?pageNumber=1&pageSize=10`, {
      headers: this.getAuthHeaders()
    }).subscribe({
      next: (response) => {
        if (response && response.data) {
          this.products = Array.isArray(response.data) ? response.data : [];
        } else if (response && response.result) {
          this.products = Array.isArray(response.result) ? response.result : [];
        } else if (Array.isArray(response)) {
          this.products = response;
        } else {
          this.products = [];
        }
        this.filteredProducts = this.products;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching products:', error);
        this.products = [];
        this.filteredProducts = [];
        this.loading = false;
        this.handleError(error);
      }
    });
  }

  private fetchCategories() {
    this.loading = true;
    this.http.get<Category[]>(`${this.apiUrl}/Categories`, {
      headers: this.getAuthHeaders()
    }).subscribe({
      next: (data) => {
        this.categories = Array.isArray(data) ? data : [];
        this.filteredCategories = this.categories;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching categories:', error);
        this.categories = [];
        this.filteredCategories = [];
        this.loading = false;
        this.handleError(error);
      }
    });
  }

  private fetchOrders() {
    this.loading = true;
    this.http.get<Order[]>(`${this.apiUrl}/Orders/admin`, {
      headers: this.getAuthHeaders()
    }).subscribe({
      next: (data) => {
        this.orders = Array.isArray(data) ? data : [];
        this.filteredOrders = this.orders;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching orders:', error);
        this.orders = [];
        this.filteredOrders = [];
        this.loading = false;
        this.handleError(error);
      }
    });
  }

  private handleError(error: any) {
    if (error.status === 401) {
      alert('Unauthorized. Please login again.');
      localStorage.removeItem('token');
      window.location.href = '/login';
    } else if (error.status === 403) {
      alert('Access denied. Admin privileges required.');
    } else {
      console.error('API Error:', error);
      alert('An error occurred while fetching data. Please try again.');
    }
  }

  refreshData() {
    this.loadDataForTab(this.activeTab);
  }

  addNew(type: string) {
    this.modalType = type;
    this.selectedItem = null;
    this.newItem = {};
    this.showModal = true;
  }

  editCategory(category: Category) {
    this.modalType = 'categories';
    this.selectedItem = category;
    this.newItem = { ...category };
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.modalType = null;
    this.selectedItem = null;
    this.newItem = {};
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.newItem.image = file;
    }
  }

  saveItem() {
    if (!this.modalType) return;

    const isEdit = !!this.selectedItem;
    let endpoint = '';
    let headers = this.getAuthHeaders(this.modalType === 'products');

    let body = this.newItem;
    if (this.modalType === 'products') {
      const formData = new FormData();
      for (const key in this.newItem) {
        if (this.newItem.hasOwnProperty(key) && this.newItem[key] !== undefined) {
          formData.append(key, this.newItem[key]);
        }
      }
      body = formData;
      endpoint = `${this.apiUrl}/Product/Product`;
    } else if (this.modalType === 'categories') {
      endpoint = isEdit ? `${this.apiUrl}/Categories/${this.selectedItem.id}` : `${this.apiUrl}/Categories`;
    } else {
      console.log('Unknown modal type:', this.modalType);
      return;
    }

    const request = isEdit
      ? this.http.put(endpoint, body, { headers })
      : this.http.post(endpoint, body, { headers });

    request.subscribe({
      next: () => {
        alert(isEdit ? 'Category updated successfully' : 'Item added successfully');
        this.refreshData();
        this.closeModal();
      },
      error: (error) => {
        console.error('Error saving item:', error);
        this.handleError(error);
      }
    });
  }

  deleteItem(id: number, type: string) {
    if (confirm('Are you sure you want to delete this item?')) {
      this.loading = true;

      let endpoint = '';
      const headers = this.getAuthHeaders();

      switch (type) {
        case 'category':
          endpoint = `${this.apiUrl}/Categories/${id}`;
          break;
        case 'order':
          endpoint = `${this.apiUrl}/Orders/admin/${id}`;
          break;
        default:
          console.log(`Delete functionality not available for ${type}`);
          this.loading = false;
          return;
      }

      this.http.delete(endpoint, { headers }).subscribe({
        next: () => {
          alert('Item deleted successfully.');
          this.refreshData();
        },
        error: (error) => {
          console.error('Error while deleting:', error);
          this.loading = false;
          this.handleError(error);
        }
      });
    }
  }

  promoteUser(userId: string) {
    if (confirm('Are you sure you want to promote this user to admin?')) {
      this.loading = true;
      this.http.put(`${this.apiUrl}/Users/admin/promote/${userId}`, {}, {
        headers: this.getAuthHeaders()
      }).subscribe({
        next: () => {
          alert('User promoted to admin successfully.');
          this.refreshData();
        },
        error: (error) => {
          console.error('Error promoting user:', error);
          this.loading = false;
          this.handleError(error);
        }
      });
    }
  }

  updateOrderStatus(orderId: number, newStatus: OrderStatus) {
    const updateDto: OrderUpdateStatusDto = {
      id: orderId,
      status: newStatus
    };

    this.loading = true;
    this.http.put<Order>(`${this.apiUrl}/Orders/admin/${orderId}/status`, updateDto, {
      headers: this.getAuthHeaders()
    }).subscribe({
      next: (updatedOrder) => {
        const index = this.orders.findIndex(o => o.id === orderId);
        if (index !== -1) {
          this.orders[index] = updatedOrder;
          this.filteredOrders = [...this.orders];
        }
        this.loading = false;
        alert('Order status updated successfully.');
      },
      error: (error) => {
        console.error('Error updating order status:', error);
        this.loading = false;
        this.handleError(error);
      }
    });
  }

  updatePaymentStatus(orderId: number, newPaymentStatus: PaymentStatus) {
    const updateDto: OrderUpdatePaymentDto = {
      id: orderId,
      paymentStatus: newPaymentStatus
    };

    this.loading = true;
    this.http.put<Order>(`${this.apiUrl}/Orders/admin/${orderId}/payment-status`, updateDto, {
      headers: this.getAuthHeaders()
    }).subscribe({
      next: (updatedOrder) => {
        const index = this.orders.findIndex(o => o.id === orderId);
        if (index !== -1) {
          this.orders[index] = updatedOrder;
          this.filteredOrders = [...this.orders];
        }
        this.loading = false;
        alert('Payment status updated successfully.');
      },
      error: (error) => {
        console.error('Error updating payment status:', error);
        this.loading = false;
        this.handleError(error);
      }
    });
  }

  search() {
    const term = this.searchTerm.toLowerCase();

    if (this.activeTab === 'users') {
      this.filteredUsers = this.users.filter(u =>
        u.name?.toLowerCase().includes(term) ||
        u.email?.toLowerCase().includes(term) ||
        u.phone?.toLowerCase().includes(term)
      );
    } else if (this.activeTab === 'products') {
      this.filteredProducts = this.products.filter(p =>
        p.name?.toLowerCase().includes(term) ||
        p.category?.toLowerCase().includes(term) ||
        p.description?.toLowerCase().includes(term)
      );
    } else if (this.activeTab === 'categories') {
      this.filteredCategories = this.categories.filter(c =>
        c.name?.toLowerCase().includes(term) ||
        c.description?.toLowerCase().includes(term)
      );
    } else if (this.activeTab === 'orders') {
      this.filteredOrders = this.orders.filter(o =>
        o.id.toString().includes(term) ||
        o.userName?.toLowerCase().includes(term) ||
        o.userEmail?.toLowerCase().includes(term) ||
        o.status?.toString().toLowerCase().includes(term) ||
        o.paymentStatus?.toString().toLowerCase().includes(term)
      );
    }
  }

  get dashboardStats() {
    return {
      totalUsers: this.users.length,
      totalProducts: this.products.length,
      totalCategories: this.categories.length,
      totalOrders: this.orders.length,
      activeUsers: this.users.filter(u => u.isActive === true).length || 0,
      pendingOrders: this.orders.filter(o => o.status === OrderStatus.Pending).length || 0,
      paidOrders: this.orders.filter(o => o.paymentStatus === PaymentStatus.Paid).length || 0
    };
  }

  getStatusClass(status: OrderStatus): string {
    switch (status) {
      case OrderStatus.Pending: return 'status-pending';
      case OrderStatus.Processing: return 'status-processing';
      case OrderStatus.Shipped: return 'status-shipped';
      case OrderStatus.Delivered: return 'status-delivered';
      case OrderStatus.Cancelled: return 'status-cancelled';
      default: return '';
    }
  }

  getPaymentStatusClass(status: PaymentStatus): string {
    switch (status) {
      case PaymentStatus.Pending: return 'payment-pending';
      case PaymentStatus.Paid: return 'payment-paid';
      case PaymentStatus.Failed: return 'payment-failed';
      case PaymentStatus.Refunded: return 'payment-refunded';
      default: return '';
    }
  }

  getOrderStatusOptions(): OrderStatus[] {
    return Object.values(OrderStatus);
  }

  getPaymentStatusOptions(): PaymentStatus[] {
    return Object.values(PaymentStatus);
  }

  onOrderStatusChange(orderId: number, event: any) {
    const newStatus = event.target.value as OrderStatus;
    this.updateOrderStatus(orderId, newStatus);
  }

  onPaymentStatusChange(orderId: number, event: any) {
    const newStatus = event.target.value as PaymentStatus;
    this.updatePaymentStatus(orderId, newStatus);
  }
}