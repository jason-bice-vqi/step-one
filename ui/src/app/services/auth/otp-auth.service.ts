import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable, take, tap} from "rxjs";
import {environment} from "../../../environments/environment";
import {JwtService} from "./jwt.service";

@Injectable({
  providedIn: 'root'
})
export class OtpAuthService {

  private endpoint = "auth/otp";

  constructor(private httpClient: HttpClient, private jwtService: JwtService) {
  }

  /**
   * Requests a One-Time Passcode (OTP).
   * @param phoneNumber The phone number to which the OTP should be sent.
   */
  requestOtp(phoneNumber: string): Observable<any> {
    const request = {phoneNumber};

    return this.httpClient
      .post(`${environment.apiUrl}/${this.endpoint}`, request, {
        headers: new HttpHeaders({'Content-Type': 'application/json'})
      })
      .pipe(take(1));
  }

  /**
   * Verifies a One-Time Passcode (OTP) and establishes the JWT.
   * @param phoneNumber The phone number associated with the OTP.
   * @param otp The OTP to be verified.
   * @return A JWT if authenticated.
   */
  authenticate(phoneNumber: string, otp: string): Observable<string> {
    const request = {phoneNumber, otp};

    return this.httpClient
      .post<string>(`${environment.apiUrl}/${this.endpoint}/challenge`, request, {
        headers: new HttpHeaders({'Content-Type': 'application/json'})
      })
      .pipe(take(1), tap((x: any) => this.jwtService.storeToken(x.token)));
  }
}
