import {CandidateWorkflow} from "./candidate.workflow";
import {WorkflowStep} from "./workflow.step";
import {CandidateWorkflowStepStatuses} from "./candidate-workflow-step.statuses";

/**
 * Represents a step within a candidate's workflow.
 */
export interface CandidateWorkflowStep {
  id: number;

  /** The ID of the associated candidate workflow. */
  candidateWorkflowId: number;

  /** The candidate workflow associated with this step. */
  candidateWorkflow?: CandidateWorkflow | null;

  /** The ID of the associated workflow step. */
  workflowStepId: number;

  /** The workflow step associated with this candidate workflow step. */
  workflowStep?: WorkflowStep | null;

  /** The timestamp when this step was completed, if applicable. */
  completedAt?: Date | null;

  /** Whether this step has been completed. */
  isCompleted: boolean;

  /** Indicates whether an administrator has confirmed this step. */
  isConfirmedByAdmin: boolean;

  /** The current status of this candidate workflow step. */
  candidateWorkflowStepStatus: CandidateWorkflowStepStatuses;
}
