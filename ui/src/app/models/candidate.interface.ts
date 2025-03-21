import {CandidateWorkflowInterface} from "./candidate-workflow.interface";
import {EntityStatusesEnum} from "./entity-statuses.enum";

/**
 * Represents a candidate in the system.
 */
export interface CandidateInterface {
  /** The candidate's unique identifier. */
  id: number;

  /** The candidate's first name. */
  firstName: string;

  /** The candidate's last name. */
  lastName: string;

  /** The candidate's full name (computed in C#, omitted in TypeScript). */
  fullName?: string;

  /** The candidate's phone number, expected to be exactly 10 characters. */
  phoneNumber: string;

  /** The timestamp when the last OTP was requested. */
  otpLastRequestedAt?: Date | null;

  /** The timestamp when the candidate was last authenticated (last logon). */
  lastAuthenticatedAt?: Date | null;

  /** The ID of the associated candidate workflow, if any. */
  candidateWorkflowId?: number | null;

  /** The workflow associated with this candidate. */
  candidateWorkflow?: CandidateWorkflowInterface | null;

  /** The current status of this candidate. */
  entityStatus: EntityStatusesEnum;
}
