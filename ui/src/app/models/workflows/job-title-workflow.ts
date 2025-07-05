import {Workflow} from "./workflow";

export interface JobTitleWorkflow {
  id: number;
  jobTitleId: number;
  workflowId: number;
  workflow: Workflow;
}
