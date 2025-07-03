import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {catchError, Observable, of, tap, throwError} from "rxjs";
import {JobTitleWorkflow} from "../../models/workflows/job-title-workflow";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class JobTitleWorkflowService {

  constructor(private httpClient: HttpClient) {
  }

  showByJobTitle(jobTitleId: number): Observable<JobTitleWorkflow | null> {
    const endpoint = `${environment.apiUrl}/job-titles/${jobTitleId}/workflow`;

    return this.httpClient.get<JobTitleWorkflow>(endpoint).pipe(
      tap(x => console.info('Job Title Workflow Retrieved', x)),
      catchError(err => {
        if (err.status === 404) {
          return of(null);
        }

        return throwError(() => err);
      })
    );
  }

  create(jobTitleId: number, workflowId: number): Observable<JobTitleWorkflow> {
    const jobTitleWorkflowReq = {workflowId};
    const endpoint = `${environment.apiUrl}/job-titles/${jobTitleId}/workflow`;

    return this.httpClient.post<JobTitleWorkflow>(endpoint, jobTitleWorkflowReq)
      .pipe(tap(x => console.info('Job Title Workflow Created', x)));
  }
}
