import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable, tap} from "rxjs";
import {CandidateInterface} from "../../models/candidates/candidate.interface";
import {environment} from "../../../environments/environment";
import {SearchResponseInterface} from "../../models/search/search-response.interface";
import {SearchRequestInterface} from "../../models/search/search-request.interface";

@Injectable({
  providedIn: 'root'
})
export class CandidateService {
  private endpoint = `${environment.apiUrl}/candidates`;

  constructor(private httpClient: HttpClient) {
  }

  search(searchRequest: SearchRequestInterface<CandidateInterface>): Observable<SearchResponseInterface<CandidateInterface>> {
    const httpParams = new HttpParams().append('page', searchRequest.page).append('limit', searchRequest.limit);

    return this.httpClient.get<SearchResponseInterface<CandidateInterface>>(this.endpoint, {params: httpParams})
      .pipe(tap(x => console.info('Candidates Retrieved', x)));
  }
}
