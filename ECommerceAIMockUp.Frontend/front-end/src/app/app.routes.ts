import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  {path: 'products', loadComponent: () => import('./pages/products/product-list/product-list.component').then(c => c.ProductComponent), title: 'Product list' },
  { path: 'products/:id', loadComponent: () => import('./pages/products/product-details/product-details.component').then(c => c.ProductDetailsComponent), title: 'Product Details' },
  { path: 'orders', loadComponent: () => import('./pages/orders/order-list/order-list.component').then(c => c.OrderListComponent), title: 'Orders' },
  { path: 'orders/:id', loadComponent: () => import('./pages/orders/order-details/order-details.component').then(c => c.OrderDetailsComponent), title: 'Order Details' },
  {
    path: 'auth',
    loadComponent: () => import('./layouts/auth-layout/auth-layout.component').then(m => m.AuthLayoutComponent),
    children: [
      { path: 'login', loadComponent: () => import('./pages/auth/login/login.component')
        .then(c => c.LoginComponent), title: 'Login', data: { hideFooter: true, hideNavbar: true } },
      { path: 'register', loadComponent: () => import('./pages/auth/register/register.component')
        .then(c => c.RegisterComponent), title: 'Register', data: { hideFooter: true, hideNavbar: true } }
    ]
  },
  { 
    path: 'design',
    loadChildren: () => import('./pages/design/route').then(m => m.DESIGN_ROUTES),
    title: 'Design'
  },


  {
    path: 'admin',
    loadComponent: () => import('./pages/admin/dashboard.component').then(c => c.AdminDashboardComponent),
    title: 'Admin Dashboard'
  },
  { path: '**', redirectTo: 'auth/login' }
];
