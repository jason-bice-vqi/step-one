export interface CandidateOnboardingRequest {
  createJobTitleAlias: boolean;
  assignWorkflowToJobTitle: boolean;
  jobTitleId: number;
  sendOnboardingInvite: boolean;
  workflowId: number;
}
