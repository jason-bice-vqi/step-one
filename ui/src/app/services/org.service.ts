import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, tap} from "rxjs";
import {environment} from "../../environments/environment";
import {Company} from "../models/org/company";
import {JobTitle} from "../models/org/job-title";

@Injectable({
  providedIn: 'root'
})
export class OrgService {

  constructor(private httpClient: HttpClient) {
  }

  getCompanies(): Observable<Company[]> {
    const endpoint = `${environment.apiUrl}/companies`;

    return this.httpClient.get<Company[]>(endpoint)
      .pipe(tap(x => console.info('Companies Retrieved', x)));
  }

  getJobTitles(companyId: number): Observable<JobTitle[]> {
    const endpoint = `${environment.apiUrl}/companies/${companyId}/job-titles`;

    return this.httpClient.get<JobTitle[]>(endpoint)
      .pipe(tap(x => console.info('Job Titles Retrieved', x)));
  }
}
