export interface CandidateOnboardingRequest {
  candidateId: number;
  matchAtsJobTitleToOfficialJobTitle: boolean;
  matchWorkflowToJobTitle: boolean;
  jobTitleId: number;
  sendOnboardingInvite: boolean;
  workflowId: number;
}
