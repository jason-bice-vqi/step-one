import {Injectable} from '@angular/core';
import {jwtDecode, JwtPayload} from "jwt-decode";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  private tokenKey = 'jwt';

  constructor(private router: Router) {

  }

  decodeToken(): any | null {
    const token = this.getToken();

    if (!token) {
      return null;
    }

    try {
      return jwtDecode(token);
    } catch (error) {
      console.error('Invalid JWT Token:', error);

      return null;
    }
  }

  hasClaim(claimName: string): boolean {
    const decodedToken = this.decodeToken();

    return decodedToken[claimName] !== null;
  }

  getRole(): string {
    const decodedToken = this.decodeToken();

    return decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  }

  hasRole(roleName: string): boolean {
    const decodedToken = this.decodeToken();

    const roles = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    return roles.some((x:any) => x.value === roleName);
  }

  getFullName() : string | null {
    const decodedToken = this.decodeToken();

    return decodedToken['name'];
  }

  storeToken(token: string) {
    console.info('Storing JWT', jwtDecode(token));

    sessionStorage.setItem(this.tokenKey, token);
  }

  getToken() {
    return sessionStorage.getItem(this.tokenKey);
  }

  isTokenExpired(): boolean {
    const decodedToken = this.decodeToken();

    if (!decodedToken || !decodedToken.exp) {
      return true;
    }

    const expiryTime = decodedToken.exp * 1000; // Convert seconds to milliseconds

    return Date.now() >= expiryTime;
  }

  isAuthenticated(): boolean {
    return !!this.getToken() && !this.isTokenExpired();
  }

  logout(): void {
    sessionStorage.removeItem(this.tokenKey);

    this.router.navigate(['/login']);
  }
}
