import {SearchRequestInterface} from "../search/search-request.interface";
import {CandidateStatusEnum} from "./candidate-status.enum";
import {CandidateWorkflowStepStatusesEnum} from "../workflows/candidate-workflow-step-statuses.enum";

export interface CandidateSearchRequestInterface extends SearchRequestInterface {
  name?: string;

  candidateStatus?: CandidateStatusEnum;

  workflowStatus?: CandidateWorkflowStepStatusesEnum;

  company?: any;

  jobTitle?: any;
}
