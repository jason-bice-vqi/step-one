import {inject, Injectable} from '@angular/core';
import {CanActivateFn, Router} from '@angular/router';
import {JwtService} from "../services/auth/jwt.service";

export const authGuard: CanActivateFn = () => {

  const jwtService = inject(JwtService);
  const router = inject(Router);

  if (jwtService.isAuthenticated()) {
    return true;
  }

  console.log('User is not authenticated. Redirecting to login.');

  router.navigate(['/login']);

  return false;
}
