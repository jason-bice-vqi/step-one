import {SearchRequest} from "../search/search.request";
import {CandidateStatus} from "./candidate.status";
import {CandidateWorkflowStepStatuses} from "../workflows/candidate-workflow-step.statuses";

export interface CandidateSearchRequest extends SearchRequest {
  name?: string;

  candidateStatus?: CandidateStatus;

  workflowStatus?: CandidateWorkflowStepStatuses;

  company?: any;

  jobTitle?: any;
}
