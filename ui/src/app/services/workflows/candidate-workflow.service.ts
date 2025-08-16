import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable, tap} from "rxjs";
import {environment} from "../../../environments/environment";
import {Candidate} from "../../models/candidates/candidate";
import {CandidateOnboardingRequest} from "../../models/candidates/candidate-onboarding-request";

@Injectable({
  providedIn: 'root'
})
export class CandidateWorkflowService {

  constructor(private httpClient: HttpClient) {
  }

  create(candidateId: number | string, candidateOnboardingRequest: CandidateOnboardingRequest): Observable<Candidate> {
    const endpoint = `${environment.apiUrl}/candidates/${candidateId}/workflow`;

    return this.httpClient.post<Candidate>(endpoint, candidateOnboardingRequest)
      .pipe(tap(x => console.info('Candidate Workflow Created', x)));
  }

  get(candidateId: number | string): Observable<Candidate> {
    const endpoint = `${environment.apiUrl}/candidates/${candidateId}/workflow`;

    return this.httpClient.get<Candidate>(endpoint)
      .pipe(tap(x => console.info('Candidate w/Workflow Retrieved', x)));
  }
}
