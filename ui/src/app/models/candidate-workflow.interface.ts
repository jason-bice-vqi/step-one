import {WorkflowInterface} from "./workflow.interface";
import {CandidateInterface} from "./candidate.interface";
import {CandidateWorkflowStepInterface} from "./candidate-workflow-step.interface";
import {EntityStatusesEnum} from "./entity-statuses.enum";

/**
 * Represents a candidate's progress through a workflow.
 */
export interface CandidateWorkflowInterface {
  id: number;

  /** The unique identifier of the candidate. */
  candidateId: string;

  /** The candidate associated with this workflow. */
  candidate?: CandidateInterface | null;

  /** The ID of the associated workflow. */
  workflowId: number;

  /** The workflow associated with this candidate workflow. */
  workflow?: WorkflowInterface | null;

  /** The date and time when this candidate workflow was created. */
  createdAt: Date;

  /** The date and time when this candidate workflow was completed, if applicable. */
  completedAt?: Date | null;

  /** The current status of this candidate workflow. */
  entityStatus: EntityStatusesEnum;

  /** The collection of candidate workflow steps associated with this workflow. */
  candidateWorkflowSteps: CandidateWorkflowStepInterface[];

  /** The number of steps completed. */
  completedSteps: number;
}
