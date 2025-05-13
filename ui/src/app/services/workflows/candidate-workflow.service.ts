import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {CandidateWorkflow} from "../../models/workflows/candidate.workflow";
import {Observable, tap} from "rxjs";
import {environment} from "../../../environments/environment";
import {Candidate} from "../../models/candidates/candidate";

@Injectable({
  providedIn: 'root'
})
export class CandidateWorkflowService {

  constructor(private httpClient: HttpClient) {
  }

  get(candidateId: string): Observable<Candidate> {
    const endpoint = `${environment.apiUrl}/candidates/${candidateId}/workflow`;

    return this.httpClient.get<Candidate>(endpoint)
      .pipe(tap(x => console.info('Candidate w/Workflow Retrieved', x)));
  }
}
