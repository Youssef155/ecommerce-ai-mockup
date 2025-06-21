import { Routes } from '@angular/router';
import { AuthorizationGuard } from './core/guards/authorization.guard';
import { ProductComponent } from './pages/products/product-list/product-list.component';

export const routes: Routes = [
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
  {
    path:' ', runGuardsAndResolvers:'always',
    canActivate:[AuthorizationGuard],
    children:[
      {path: 'product', component: ProductComponent}
    ]
  },
{ path: 'products', loadComponent: () => import('./pages/products/product-list/product-list.component').then(c => c.ProductComponent), title: 'Products' },
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

  { path: '**', redirectTo: 'auth/login' }
];
