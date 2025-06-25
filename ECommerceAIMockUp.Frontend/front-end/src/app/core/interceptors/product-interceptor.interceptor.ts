import { HttpInterceptorFn } from '@angular/common/http';

export const productInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const token =  localStorage.getItem("angular19Token");
  if (token) {
    const clonedReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(clonedReq); // ✅ Use the cloned request
  }

  return next(req);

};
