import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  {path: 'products', loadComponent: () => import('./pages/products/product-list/product-list.component').then(c => c.ProductComponent), title: 'Product list' },
  { path: 'products/:id', loadComponent: () => import('./pages/products/product-details/product-details.component').then(c => c.ProductDetailsComponent), title: 'Product Details' },
  { path: 'design', loadComponent: () => import('./pages/design/design-preview/design-preview.component').then(c => c.DesignPreviewComponent), title: 'Design Preview' },
  { path: 'canvas', loadComponent: () => import('./pages/design/canvas-editor/canvas-editor.component').then(c => c.CanvasEditorComponent), title: 'Design Canvas' },
  { path: 'orders', loadComponent: () => import('./pages/orders/order-list/order-list.component').then(c => c.OrderListComponent), title: 'Orders' },
  { path: 'orders/:id', loadComponent: () => import('./pages/orders/order-details/order-details.component').then(c => c.OrderDetailsComponent), title: 'Order Details' },
  {
    path: 'auth',
    loadComponent: () => import('./layouts/auth-layout/auth-layout.component').then(m => m.AuthLayoutComponent),
    children: [
      { path: 'login', loadComponent: () => import('./pages/auth/login/login.component').then(c => c.LoginComponent), title: 'Login' },
      { path: 'register', loadComponent: () => import('./pages/auth/register/register.component').then(c => c.RegisterComponent), title: 'Register' }
    ]
  },


  {
    path: 'admin',
    loadComponent: () => import('./pages/admin/dashboard.component').then(c => c.DashboardComponent),
    title: 'Admin Dashboard'
  },
  { path: '**', redirectTo: 'auth/login' }
];
