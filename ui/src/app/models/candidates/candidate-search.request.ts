import {SearchRequest} from "../search/search.request";
import {CandidateStatus} from "./candidate.status";
import {CandidateWorkflowStatuses} from "./candidate-workflow.statuses";

export interface CandidateSearchRequest extends SearchRequest {
  name?: string | null;

  candidateWorkflowStatus?: CandidateWorkflowStatuses | null;

  companyId?: number | null;

  jobTitleId?: number | null;

  atsJobTitle?: string | null;
}
