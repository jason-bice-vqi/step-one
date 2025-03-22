import {inject} from '@angular/core';
import {CanActivateFn} from '@angular/router';
import {JwtService} from "../../services/auth/jwt.service";

export const internalUserAuthGuard: CanActivateFn = () => {

  const jwtService = inject(JwtService);

  if (jwtService.isAuthenticated() && jwtService.isInternal()) {
    return true;
  }

  console.warn('User is not authenticated or not an internal user. Logging out.');

  jwtService.logout();

  return false;
}
