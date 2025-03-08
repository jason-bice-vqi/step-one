import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {JwtService} from "./jwt.service";
import {Observable, take, tap} from "rxjs";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AdAuthService {

  private endpoint = "auth/ad";

  constructor(private httpClient: HttpClient, private jwtService: JwtService) {
  }

  authenticate(): Observable<string> {
    return this.httpClient
      .post<string>(`${environment.apiUrl}/${this.endpoint}`, {
        headers: new HttpHeaders({'Content-Type': 'application/json'})
      })
      .pipe(take(1), tap((x: any) => this.jwtService.storeToken(x.token)));
  }
}
