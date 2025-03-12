import {inject} from '@angular/core';
import {CanActivateFn, Router} from '@angular/router';
import {JwtService} from "../../services/auth/jwt.service";

export const adminAuthGuard: CanActivateFn = () => {

  const jwtService = inject(JwtService);

  if (jwtService.isAuthenticated() && jwtService.isAdministrator()) {
    return true;
  }

  console.log('User is not authenticated or not an administrator. Logging out.');

  jwtService.logout();

  return false;
}
