import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable, tap} from "rxjs";
import {environment} from "../../../environments/environment";
import {CandidateSearchRequest} from "../../models/candidates/candidate-search.request";
import {CandidateSearchResponse} from "../../models/candidates/candidate-search.response";
import {cleanRequestParams} from "../../functions/http.functions";

@Injectable({
  providedIn: 'root'
})
export class CandidateService {
  private endpoint = `${environment.apiUrl}/candidates`;

  constructor(private httpClient: HttpClient) {
  }

  search(searchRequest: CandidateSearchRequest): Observable<CandidateSearchResponse> {
    const httpParams = new HttpParams({fromObject: cleanRequestParams(searchRequest)});

    return this.httpClient.get<CandidateSearchResponse>(this.endpoint, {params: httpParams})
      .pipe(tap(x => console.info('Candidates Retrieved', x)));
  }
}
