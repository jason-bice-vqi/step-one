import {Workflow} from "./workflow";
import {Candidate} from "../candidates/candidate";
import {CandidateWorkflowStep} from "./candidate-workflow.step";
import {EntityStatuses} from "../entity.statuses";

/**
 * Represents a candidate's progress through a workflow.
 */
export interface CandidateWorkflow {
  id: number;

  /** The unique identifier of the candidate. */
  candidateId: string;

  /** The candidate associated with this workflow. */
  candidate?: Candidate | null;

  /** The ID of the associated workflow. */
  workflowId: number;

  /** The workflow associated with this candidate workflow. */
  workflow?: Workflow | null;

  /** The date and time when this candidate workflow was created. */
  createdAt: Date;

  /** The date and time when this candidate workflow was completed, if applicable. */
  completedAt?: Date | null;

  /** The current status of this candidate workflow. */
  entityStatus: EntityStatuses;

  /** The collection of candidate workflow steps associated with this workflow. */
  candidateWorkflowSteps: CandidateWorkflowStep[];

  /** The number of steps completed. */
  completedSteps: number;

  percentComplete?: number | null
}
