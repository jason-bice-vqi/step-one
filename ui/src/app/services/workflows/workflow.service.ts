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

  update(workflow: Workflow): Observable<Workflow> {
    const endpoint = `${environment.apiUrl}/workflows/${workflow.id}`;

    return this.httpClient.patch<Workflow>(endpoint, workflow)
      .pipe(tap(x => console.info('Workflow Updated', x)));
  }
}
