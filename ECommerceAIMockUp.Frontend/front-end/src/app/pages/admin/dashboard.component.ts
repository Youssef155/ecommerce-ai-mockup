import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../../interfaces/user';
import { Product } from '../../interfaces/product';
import { Category } from '../../interfaces/category';
import { OrderStatus } from '../../shared/Enums/order-status';
import { PaymentStatus } from '../../shared/Enums/payment-status';
import { Gender } from '../../shared/Enums/gender';
import { Season } from '../../shared/Enums/season';
import { Order } from '../../interfaces/order';
import { ApiResponse } from '../../interfaces/api-response';
import { OrderUpdatePaymentDto, OrderUpdateStatusDto } from '../../interfaces/order-item';
import { Router } from '@angular/router';
import { Subject, takeUntil, catchError, finalize } from 'rxjs';
import { of } from 'rxjs';

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
  private destroy$ = new Subject<void>();


  OrderStatus = OrderStatus;
  PaymentStatus = PaymentStatus;
  Gender = Gender;
  Season = Season;

  menuItems = [
    { id: 'dashboard', label: 'Dashboard', icon: 'fa-solid fa-gauge' },
    { id: 'users', label: 'Users', icon: 'fa-solid fa-users' },
    { id: 'products', label: 'Products', icon: 'fa-solid fa-box' },
    { id: 'categories', label: 'Categories', icon: 'fa-solid fa-tags' },
    { id: 'orders', label: 'Orders', icon: 'fa-solid fa-shopping-cart' }
  ];

  constructor(
    private http: HttpClient,
    private router: Router 
  ) {}

  ngOnInit() {
    const token = localStorage.getItem('angular19Token');
    if (!token) {
      this.showNotification('Please login first.', 'error');
      this.logout();
      return;
    }

    const payload = this.getTokenPayload(token);
    if (!payload || payload.role !== 'Admin') {
      this.showNotification('Access denied. Admin privileges required.', 'error');
      this.logout(); 
      return;
    }

    this.loadDashboardData();
  }

  private getTokenPayload(token: string): any {
    try {
      const payloadBase64 = token.split('.')[1];
      const decodedPayload = atob(payloadBase64);
      return JSON.parse(decodedPayload);
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
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
    const token = localStorage.getItem('angular19Token');
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
    this.http.get<User[]>(`${this.apiUrl}/Users/admin`, { headers: this.getAuthHeaders() })
      .pipe(takeUntil(this.destroy$), catchError(this.handleHttpError), finalize(() => this.loading = false))
      .subscribe({
        next: (data) => {
          this.users = Array.isArray(data) ? data : [];
          this.filteredUsers = [...this.users];
        }
      });
  }

  private fetchProducts() {
    this.loading = true;
    this.http.get<ApiResponse<any>>(`${this.apiUrl}/Product/products?pageNumber=1&pageSize=10`, { headers: this.getAuthHeaders() })
      .pipe(takeUntil(this.destroy$), catchError(this.handleHttpError), finalize(() => this.loading = false))
      .subscribe({
        next: (response) => {
          console.log('API Response:', response);
          if (response && response.data) {
            if (response.data.data && Array.isArray(response.data.data)) {
              this.products = response.data.data.map((dto: any) => ({
                id: dto.Id || dto.id || 0,
                name: dto.Name || dto.name || '',
                price: dto.Price || dto.price || 0,
                description: dto.Description || dto.description || '',
                categoryName: dto.CategoryName || dto.categoryName || '',
                stock: dto.stock || 0,
                imageUrl: dto.Image || dto.imageUrl || '',
                gender: dto.Gender,
                season: dto.Season,
                colors: dto.colors || [],
                sizes: dto.sizes || [],
                categoryId: dto.categoryId
              }));
            } else if (Array.isArray(response.data)) {
              this.products = response.data.map((dto: any) => ({
                id: dto.Id || dto.id || 0,
                name: dto.Name || dto.name || '',
                price: dto.Price || dto.price || 0,
                description: dto.Description || dto.description || '',
                categoryName: dto.CategoryName || dto.categoryName || '',
                stock: dto.stock || 0,
                imageUrl: dto.Image || dto.imageUrl || '',
                gender: dto.Gender,
                season: dto.Season,
                colors: dto.colors || [],
                sizes: dto.sizes || [],
                categoryId: dto.categoryId
              }));
            }
          } else if (response && response.result && Array.isArray(response.result)) {
            this.products = response.result.map((dto: any) => ({
              id: dto.Id || dto.id || 0,
              name: dto.Name || dto.name || '',
              price: dto.Price || dto.price || 0,
              description: dto.Description || dto.description || '',
              categoryName: dto.CategoryName || dto.categoryName || '',
              stock: dto.stock || 0,
              imageUrl: dto.Image || dto.imageUrl || '',
              gender: dto.Gender,
              season: dto.Season,
              colors: dto.colors || [],
              sizes: dto.sizes || [],
              categoryId: dto.categoryId
            }));
          } else {
            console.warn('Unexpected response structure:', response);
            this.products = [];
          }
          this.filteredProducts = [...this.products];
        }
      });
  }

  private fetchCategories() {
    this.loading = true;
    this.http.get<Category[]>(`${this.apiUrl}/Categories`, { headers: this.getAuthHeaders() })
      .pipe(takeUntil(this.destroy$), catchError(this.handleHttpError), finalize(() => this.loading = false))
      .subscribe({
        next: (data) => {
          this.categories = Array.isArray(data) ? data : [];
          this.filteredCategories = [...this.categories];
        }
      });
  }

  private fetchOrders() {
    this.loading = true;
    this.http.get<Order[]>(`${this.apiUrl}/Orders/admin`, { headers: this.getAuthHeaders() })
      .pipe(takeUntil(this.destroy$), catchError(this.handleHttpError), finalize(() => this.loading = false))
      .subscribe({
        next: (data) => {
          this.orders = Array.isArray(data) ? data : [];
          this.filteredOrders = [...this.orders];
        }
      });
  }

  private handleHttpError = (error: any) => {
    console.error('HTTP Error:', error);
    
    if (error.status === 401) {
      this.showNotification('Session expired. Please login again.', 'error');
      this.logout();
      return of(null);
    } else if (error.status === 403) {
      this.showNotification('Access denied. Admin privileges required.', 'error');
      this.logout();
      return of(null);
    } else if (error.status === 404) {
      this.showNotification('Resource not found.', 'error');
    } else if (error.status === 500) {
      this.showNotification('Server error. Please try again later.', 'error');
    } else {
      this.showNotification('An unexpected error occurred. Please try again.', 'error');
    }
    
    return of(null);
  };

  private logout(): void {
    localStorage.removeItem('angular19Token');
    this.router.navigate(['/login']);
  }

  private redirectToUnauthorized(): void {
    this.clearAllData();
    this.router.navigate(['/unauthorized']);
  }

  private clearAllData(): void {
    this.users = [];
    this.products = [];
    this.categories = [];
    this.orders = [];
    this.filteredUsers = [];
    this.filteredProducts = [];
    this.filteredCategories = [];
    this.filteredOrders = [];
  }

  private showNotification(message: string, type: 'success' | 'error' | 'warning'): void {
    // Replace with proper notification service later
    alert(message);
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

    let body: any;
    if (this.modalType === 'products') {
      const formData = new FormData();
      
      formData.append('Name', this.newItem.name || '');
      formData.append('Description', this.newItem.description || '');
      
      if (this.newItem.gender !== undefined) {
        formData.append('Gender', this.newItem.gender.toString());
      }
      if (this.newItem.season !== undefined) {
        formData.append('Season', this.newItem.season.toString());
      }
      if (this.newItem.price) {
        formData.append('Price', this.newItem.price.toString());
      }
      if (this.newItem.categoryName) {
        formData.append('CategoryName', this.newItem.categoryName);
      }
      
      if (this.newItem.image) {
        formData.append('ImgUrl', this.newItem.image);
      }

      console.log('Sending FormData:', Array.from(formData.entries()));
      body = formData;
      endpoint = `${this.apiUrl}/Product/Product`;
    } else if (this.modalType === 'categories') {
      body = this.newItem;
      endpoint = isEdit ? `${this.apiUrl}/Categories/${this.selectedItem.id}` : `${this.apiUrl}/Categories`;
    } else {
      console.log('Unknown modal type:', this.modalType);
      return;
    }

    this.loading = true;
    const request = isEdit
      ? this.http.put(endpoint, body, { headers })
      : this.http.post(endpoint, body, { headers });

    request.pipe(
      takeUntil(this.destroy$), 
      catchError((error) => {
        console.error('Error saving item:', error);
        if (error.status === 400) {
          this.showNotification('Error: Check if all required fields are filled correctly.', 'error');
        } else if (error.status === 401) {
          this.showNotification('Session expired. Please login again.', 'error');
          this.logout();
        } else if (error.status === 403) {
          this.showNotification('Access denied. Admin privileges required.', 'error');
          this.logout();
        } else {
          this.showNotification('An error occurred while saving. Please try again.', 'error');
        }
        return of(null);
      }),
      finalize(() => this.loading = false)
    ).subscribe({
      next: (response) => {
        if (response !== null) {
          this.showNotification(isEdit ? 'Item updated successfully' : 'Item added successfully', 'success');
          this.refreshData();
          this.closeModal();
        }
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

      this.http.delete(endpoint, { headers }).pipe(
        takeUntil(this.destroy$),
        catchError(this.handleHttpError),
        finalize(() => this.loading = false)
      ).subscribe({
        next: () => {
          this.showNotification('Item deleted successfully.', 'success');
          this.refreshData();
        }
      });
    }
  }

  promoteUser(userId: string) {
    if (confirm('Are you sure you want to promote this user to admin?')) {
      this.loading = true;
      this.http.put(`${this.apiUrl}/Users/admin/promote/${userId}`, {}, {
        headers: this.getAuthHeaders()
      }).pipe(
        takeUntil(this.destroy$),
        catchError(this.handleHttpError),
        finalize(() => this.loading = false)
      ).subscribe({
        next: () => {
          this.showNotification('User promoted to admin successfully.', 'success');
          this.refreshData();
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
    }).pipe(
      takeUntil(this.destroy$),
      catchError(this.handleHttpError),
      finalize(() => this.loading = false)
    ).subscribe({
      next: (updatedOrder) => {
        if (updatedOrder) {
          const index = this.orders.findIndex(o => o.id === orderId);
          if (index !== -1) {
            this.orders[index] = updatedOrder;
          }
          const filteredIndex = this.filteredOrders.findIndex(o => o.id === orderId);
          if (filteredIndex !== -1) {
            this.filteredOrders[filteredIndex] = updatedOrder;
          }
        }
        this.showNotification('Order status updated successfully.', 'success');
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
    }).pipe(
      takeUntil(this.destroy$),
      catchError(this.handleHttpError),
      finalize(() => this.loading = false)
    ).subscribe({
      next: (updatedOrder) => {
        if (updatedOrder) {
          const index = this.orders.findIndex(o => o.id === orderId);
          if (index !== -1) {
            this.orders[index] = updatedOrder;
          }
          const filteredIndex = this.filteredOrders.findIndex(o => o.id === orderId);
          if (filteredIndex !== -1) {
            this.filteredOrders[filteredIndex] = updatedOrder;
          }
        }
        this.showNotification('Payment status updated successfully.', 'success');
      }
    });
  }

  search() {
    const term = this.searchTerm.toLowerCase();

    if (this.activeTab === 'users') {
      this.filteredUsers = this.users.filter(u =>
        (u.name?.toLowerCase().includes(term) || false) ||
        (u.email?.toLowerCase().includes(term) || false) ||
        (u.phone?.toLowerCase().includes(term) || false)
      );
    } else if (this.activeTab === 'products') {
      this.filteredProducts = this.products.filter(p =>
        (p.name?.toLowerCase().includes(term) || false) ||
        (p.categoryName?.toLowerCase().includes(term) || false) ||
        (p.description?.toLowerCase().includes(term) || false) ||
        (this.getGenderName(p.gender)?.toLowerCase().includes(term) || false) ||
        (this.getSeasonName(p.season)?.toLowerCase().includes(term) || false)
      );
    } else if (this.activeTab === 'categories') {
      this.filteredCategories = this.categories.filter(c =>
        (c.name?.toLowerCase().includes(term) || false) ||
        (c.description?.toLowerCase().includes(term) || false)
      );
    } else if (this.activeTab === 'orders') {
      this.filteredOrders = this.orders.filter(o =>
        o.id.toString().includes(term) ||
        (o.userName?.toLowerCase().includes(term) || false) ||
        (o.userEmail?.toLowerCase().includes(term) || false) ||
        (this.getOrderStatusName(o.status)?.toLowerCase().includes(term) || false) ||
        (this.getPaymentStatusName(o.paymentStatus)?.toLowerCase().includes(term) || false)
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
      paidOrders: this.orders.filter(o => o.paymentStatus === PaymentStatus.Approved).length || 0
    };
  }

  getStatusClass(status: OrderStatus): string {
    switch (status) {
      case OrderStatus.Pending: return 'status-pending';
      case OrderStatus.Approved: return 'status-approved';
      case OrderStatus.Cancelled: return 'status-cancelled';
      case OrderStatus.CartItem: return 'status-cart';
      case OrderStatus.Completed: return 'status-completed';
      default: return '';
    }
  }

  getPaymentStatusClass(status: PaymentStatus): string {
    switch (status) {
      case PaymentStatus.Pending: return 'payment-pending';
      case PaymentStatus.Approved: return 'payment-approved';
      default: return '';
    }
  }

  getOrderStatusOptions(): OrderStatus[] {
    return Object.values(OrderStatus).filter(value => typeof value === 'number') as OrderStatus[];
  }

  getPaymentStatusOptions(): PaymentStatus[] {
    return Object.values(PaymentStatus).filter(value => typeof value === 'number') as PaymentStatus[];
  }

  getGenderOptions(): Gender[] {
    return Object.values(Gender).filter(value => typeof value === 'number') as Gender[];
  }

  getSeasonOptions(): Season[] {
    return Object.values(Season).filter(value => typeof value === 'number') as Season[];
  }

  getOrderStatusName(status: OrderStatus): string {
    return OrderStatus[status] || 'Unknown';
  }

  getPaymentStatusName(status: PaymentStatus): string {
    return PaymentStatus[status] || 'Unknown';
  }

  getGenderName(gender: Gender): string {
    return Gender[gender] || 'Unknown';
  }

  getSeasonName(season: Season): string {
    return Season[season] || 'Unknown';
  }

  onOrderStatusChange(orderId: number, event: any) {
    const newStatus = parseInt(event.target.value) as OrderStatus;
    this.updateOrderStatus(orderId, newStatus);
  }

  onPaymentStatusChange(orderId: number, event: any) {
    const newStatus = parseInt(event.target.value) as PaymentStatus;
    this.updatePaymentStatus(orderId, newStatus);
  }
}