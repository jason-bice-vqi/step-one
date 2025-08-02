import {SearchRequest} from "../search/search.request";
import {CandidateStatus} from "./candidate.status";
import {CandidateWorkflowStatuses} from "./candidate-workflow.statuses";

export interface CandidateSearchRequest extends SearchRequest {
  name?: string;

  candidateStatus?: CandidateStatus;

  candidateWorkflowStatus?: CandidateWorkflowStatuses;

  company?: any;

  jobTitle?: any;
}
