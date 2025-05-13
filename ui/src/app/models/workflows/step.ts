import {StepTypes} from "./step.types";

/** The fundamental definition of a task. Provides a default template for a WorkflowStep. */
export interface Step {
  id: number;

  /** The type of step this entity represents. */
  stepType: StepTypes;

  /** The default name of this step. Can be overridden by any WorkflowStep to which this step belongs. */
  nameDefault: string;

  /** Whether an administrator is required to review/approve the completion of this step before it's considered done. */
  isAdminConfirmationRequiredDefault: boolean;

  /** The link to an external site if stepType is StepTypes.ExternalHttpTask; null otherwise. */
  externalHttpTaskLink?: string | null;

  /** The file descriptor, if applicable; null otherwise. */
  descriptor?: string | null;
}
