import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {JwtService} from "./jwt.service";
import {from, Observable, switchMap, take, tap} from "rxjs";
import {environment} from "../../../environments/environment";
import {AuthenticationResult} from "@azure/msal-browser";
import {MsalService} from "@azure/msal-angular";

@Injectable({
  providedIn: 'root'
})
export class AdAuthService {

  private endpoint = "auth/ad";

  constructor(private httpClient: HttpClient, private jwtService: JwtService, private msalService: MsalService) {
  }

  authenticate(): Observable<string> {
    const initMsalService$ = () => from(this.msalService.instance.initialize())
      .pipe(take(1), tap(() => console.log('MSAL initialized.')));

    const loginPopup$ = () => from(this.msalService.loginPopup()).pipe(
      take(1),
      tap(response => {
        console.log('MSAL Response', response);

        this.msalService.instance.setActiveAccount(response.account);
      })
    );

    const acquireToken$ = (response: AuthenticationResult) =>
      from(this.msalService.instance.acquireTokenSilent({scopes: []}))
        .pipe(take(1), tap(() => console.log('MSAL Auth Response', response)));

    const apiAuthRequest$ = (tokenResponse: AuthenticationResult) =>
      this.httpClient.post(`${environment.apiUrl}/${this.endpoint}/v`, {}, {
        headers: {Authorization: `Bearer ${tokenResponse.accessToken}`}
      }).pipe(
        take(1),
        tap((x: any) => {
          console.log('API Auth Response', x);

          this.jwtService.storeToken(x.token);
        })
      );

    return initMsalService$().pipe(
      switchMap(() => loginPopup$()),
      switchMap(response => acquireToken$(response)),
      switchMap(tokenResponse => apiAuthRequest$(tokenResponse))
    );
  }
}
