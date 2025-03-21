import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {CandidateWorkflowInterface} from "../../models/candidate-workflow.interface";
import {Observable, tap} from "rxjs";
import {environment} from "../../../environments/environment";
import {CandidateInterface} from "../../models/candidate.interface";

@Injectable({
  providedIn: 'root'
})
export class CandidateWorkflowService {

  constructor(private httpClient: HttpClient) {
  }

  get(candidateId: string): Observable<CandidateInterface> {
    const endpoint = `${environment.apiUrl}/candidates/${candidateId}/workflow`;

    return this.httpClient.get<CandidateInterface>(endpoint)
      .pipe(tap(x => console.info('Candidate w/Workflow Retrieved', x)));
  }
}
