import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable, tap} from "rxjs";
import {environment} from "../../../environments/environment";
import {Step} from "../../models/workflows/step";

@Injectable({
  providedIn: 'root'
})
export class StepService {

  constructor(private httpClient: HttpClient) {
  }

  create(step: Step): Observable<Step> {
    const endpoint = `${environment.apiUrl}/steps`;

    return this.httpClient.post<Step>(endpoint, step)
      .pipe(tap(x => console.info('Step Created', x)));
  }

  delete(step: Step): Observable<any> {
    const endpoint = `${environment.apiUrl}/steps/${step.id}`;

    return this.httpClient.delete<Step>(endpoint)
      .pipe(tap(_ => console.info('Step Deleted')));
  }

  get(): Observable<Step[]> {
    const endpoint = `${environment.apiUrl}/steps`;

    return this.httpClient.get<Step[]>(endpoint)
      .pipe(tap(x => console.info('Steps Retrieved', x)));
  }
}
