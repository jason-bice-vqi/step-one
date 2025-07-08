import {Injectable} from '@angular/core';
import {Observable, tap} from "rxjs";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {JobTitle} from "../models/org/job-title";

@Injectable({
  providedIn: 'root'
})
export class JobTitleService {

  constructor(private httpClient: HttpClient) { }

  updateWorkflow(jobTitleId: number, workflowId?: number | null): Observable<JobTitle> {
    const endpoint = `${environment.apiUrl}/job-titles/${jobTitleId}`;

    const updateJobTitleRequest = {
      workflowId
    };

    return this.httpClient.patch<JobTitle>(endpoint, updateJobTitleRequest)
      .pipe(tap(x => console.info('Job Title Workflow Updated', x)));
  }
}
