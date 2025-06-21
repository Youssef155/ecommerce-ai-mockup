import { HttpInterceptorFn } from '@angular/common/http';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const userData = localStorage.getItem('userkey');

  if (userData) {
    try {
      const parsed = JSON.parse(userData);
      const token = parsed?.token;

      if (token) {
        const cloned = req.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`
          }
        });

        console.log('[Interceptor] Token attached:', token);
        return next(cloned);
      }
    } catch (e) {
      console.error('Invalid token format in localStorage', e);
    }
  }

  return next(req);
};
