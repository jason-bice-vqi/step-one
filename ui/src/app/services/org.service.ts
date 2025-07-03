import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, tap} from "rxjs";
import {environment} from "../../environments/environment";
import {Company} from "../models/org/company";
import {JobTitle} from "../models/org/job-title";
import {JobTitleAlias} from "../models/org/job-title-alias";

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

  getJobTitleAliases(jobTitleId: number): Observable<JobTitleAlias[]> {
    const endpoint = `${environment.apiUrl}/job-titles/${jobTitleId}/aliases`;

    return this.httpClient.get<JobTitleAlias[]>(endpoint)
      .pipe(tap(x => console.info('Job Title Aliases Retrieved', x)));
  }

  createJobTitleAlias(jobTitleId: number, alias: string): Observable<JobTitleAlias> {
    const jobTitleAliasReq = {alias};
    const endpoint = `${environment.apiUrl}/job-titles/${jobTitleId}/aliases`;

    return this.httpClient.post<JobTitleAlias>(endpoint, jobTitleAliasReq)
      .pipe(tap(x => console.info('Job Title Alias Created', x)));
  }

  deleteJobTitleAlias(jobTitleAliasId: number): Observable<any> {
    const endpoint = `${environment.apiUrl}/job-titles/aliases/${jobTitleAliasId}`;

    return this.httpClient.delete<JobTitleAlias>(endpoint)
      .pipe(tap(x => console.info('Job Title Alias Deleted', x)));
  }
}
