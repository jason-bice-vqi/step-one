import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable, take} from "rxjs";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class OtpService {

  private endpoint = "otp";

  constructor(private httpClient: HttpClient) {
  }

  /**
   * Requests a One-Time Passcode (OTP).
   * @param phoneNumber The phone number to which the OTP should be sent.
   */
  requestOtp(phoneNumber: string): Observable<any> {
    const request = {
      phoneNumber: phoneNumber
    };

    return this.httpClient
      .post(`${environment.apiUrl}/${this.endpoint}`, request, {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
      })
      .pipe(take(1));
  }

  /**
   * Verifies a One-Time Passcode (OTP).
   * @param phoneNumber The phone number associated with the OTP.
   * @param otp The OTP to be verified.
   */
  verifyOtp(phoneNumber: string, otp: string): Observable<boolean> {
    const request = {
      phoneNumber: phoneNumber,
      otp: otp
    };

    return this.httpClient
      .put<boolean>(`${environment.apiUrl}/${this.endpoint}`, request, {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
      })
      .pipe(take(1));
  }
}
