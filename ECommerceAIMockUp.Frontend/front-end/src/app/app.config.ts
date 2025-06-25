import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { productInterceptorInterceptor } from './core/interceptors/product-interceptor.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [provideHttpClient(withInterceptors([productInterceptorInterceptor])),provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes)]
};