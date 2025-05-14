import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, tap} from "rxjs";
import {environment} from "../../../environments/environment";
import {Workflow} from "../../models/workflows/workflow";

@Injectable({
  providedIn: 'root'
})
export class WorkflowService {

  constructor(private httpClient: HttpClient) {
  }

  get(): Observable<Workflow[]> {
    const endpoint = `${environment.apiUrl}/workflows`;

    return this.httpClient.get<Workflow[]>(endpoint)
      .pipe(tap(x => console.info('Workflows Retrieved', x)));
  }

  getById(workflowId: number): Observable<Workflow> {
    const endpoint = `${environment.apiUrl}/workflows/${workflowId}`;

    return this.httpClient.get<Workflow>(endpoint)
      .pipe(tap(x => console.info('Workflow Retrieved', x)));
  }

  create(workflow: Workflow): Observable<Workflow> {
    const endpoint = `${environment.apiUrl}/workflows`;

    return this.httpClient.post<Workflow>(endpoint, workflow)
      .pipe(tap(x => console.info('Workflow Created', x)));
  }

  update(workflow: Workflow): Observable<Workflow> {
    const endpoint = `${environment.apiUrl}/workflows/${workflow.id}`;

    return this.httpClient.patch<Workflow>(endpoint, workflow)
      .pipe(tap(x => console.info('Workflow Updated', x)));
  }

  delete(workflow: Workflow): Observable<any> {
    const endpoint = `${environment.apiUrl}/workflows/${workflow.id}`;

    return this.httpClient.delete<Workflow>(endpoint)
      .pipe(tap(x => console.info('Workflow Deleted', x)));
  }
}
