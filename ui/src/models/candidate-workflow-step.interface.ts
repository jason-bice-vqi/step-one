import {CandidateWorkflowInterface} from "./candidate-workflow.interface";
import {WorkflowStepInterface} from "./workflow-step.interface";
import {CandidateWorkflowStepStatusesEnum} from "./candidate-workflow-step-statuses.enum";

/**
 * Represents a step within a candidate's workflow.
 */
export interface CandidateWorkflowStepInterface {
  id: number;

  /** The ID of the associated candidate workflow. */
  candidateWorkflowId: number;

  /** The candidate workflow associated with this step. */
  candidateWorkflow?: CandidateWorkflowInterface | null;

  /** The ID of the associated workflow step. */
  workflowStepId: number;

  /** The workflow step associated with this candidate workflow step. */
  workflowStep?: WorkflowStepInterface | null;

  /** The timestamp when this step was completed, if applicable. */
  completedAt?: Date | null;

  /** Indicates whether an administrator has confirmed this step. */
  isConfirmedByAdmin: boolean;

  /** The current status of this candidate workflow step. */
  candidateWorkflowStepStatus: CandidateWorkflowStepStatusesEnum;
}
