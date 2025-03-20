import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {CandidateWorkflowInterface} from "../../../models/candidate-workflow.interface";
import {Observable} from "rxjs";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class CandidateWorkflowService {

  constructor(private httpClient: HttpClient) { }

  get(candidateId: string): Observable<CandidateWorkflowInterface> {
    const endpoint = `${environment.apiUrl}/candidates/${candidateId}/workflow`;

    return this.httpClient.get<CandidateWorkflowInterface>(endpoint);
  }
}
