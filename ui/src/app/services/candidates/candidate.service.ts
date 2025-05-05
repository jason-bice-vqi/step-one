import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable, tap} from "rxjs";
import {environment} from "../../../environments/environment";
import {CandidateSearchRequestInterface} from "../../models/candidates/candidate-search-request.interface";
import {CandidateSearchResponseInterface} from "../../models/candidates/candidate-search-response.interface";
import {cleanRequestParams} from "../../functions/http.functions";

@Injectable({
  providedIn: 'root'
})
export class CandidateService {
  private endpoint = `${environment.apiUrl}/candidates`;

  constructor(private httpClient: HttpClient) {
  }

  search(searchRequest: CandidateSearchRequestInterface): Observable<CandidateSearchResponseInterface> {
    const httpParams = new HttpParams({fromObject: cleanRequestParams(searchRequest)});

    return this.httpClient.get<CandidateSearchResponseInterface>(this.endpoint, {params: httpParams})
      .pipe(tap(x => console.info('Candidates Retrieved', x)));
  }
}
