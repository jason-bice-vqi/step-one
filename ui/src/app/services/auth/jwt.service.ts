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

  getClaimValue(claimName: string): string {
    const decodedToken = this.decodeToken();

    return decodedToken[`http://schemas.xmlsoap.org/ws/2005/05/identity/claims/${claimName}`];
  }

  getFullName() : string | null {
    return this.getClaimValue('name');
  }

  storeToken(token: string) {
    // Don't use decodeToken(); we want to log this as a proper JwtPayload.
    const decodedToken = jwtDecode(token);

    console.log('Storing JWT', decodedToken);

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
