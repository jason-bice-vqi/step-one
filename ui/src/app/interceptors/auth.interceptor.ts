import {Injectable} from '@angular/core';
import {HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {JwtService} from "../services/auth/jwt.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private jwtService: JwtService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const token = this.jwtService.getToken();

    if (token) {
      req = req.clone({
        setHeaders: {Authorization: `Bearer ${token}`}
      });
    }

    return next.handle(req);
  }
}
