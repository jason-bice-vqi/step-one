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

  private endpoint = "auth/ad/exchange-token";

  constructor(private httpClient: HttpClient, private jwtService: JwtService, private msalService: MsalService) {
  }

  authenticate(): Observable<string> {
    const initMsalService$ = () => from(this.msalService.instance.initialize()).pipe(take(1));

    const loginPopup$ = () => from(this.msalService.loginPopup()).pipe(
      take(1),
      tap(response => this.msalService.instance.setActiveAccount(response.account))
    );

    const acquireToken$ = (response: AuthenticationResult) =>
      from(this.msalService.instance.acquireTokenSilent({
        scopes: [`api://${environment.clientId}/access_as_user`],
        account: this.msalService.instance.getAllAccounts()[0],
        authority: environment.authority,
      })).pipe(take(1));

    const apiAuthRequest$ = (tokenResponse: AuthenticationResult) =>
      this.httpClient.post(`${environment.apiUrl}/${this.endpoint}`, {}, {
        headers: {Authorization: `Bearer ${tokenResponse.idToken}`}
      }).pipe(
        take(1),
        tap((x: any) => {
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
