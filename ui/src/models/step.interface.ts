import {WorkflowStepTypesEnum} from "./workflow-step-types.enum";
import {StepTypesEnum} from "./step-types.enum";

/**
 * The fundamental definition of a task. Provides a default template for a WorkflowStep.
 */
export interface StepInterface {
  id: number;

  /** The type of step this entity represents. */
  stepType: StepTypesEnum;

  /** The specific nature of this workflow step. */
  workflowStepType: WorkflowStepTypesEnum;

  /** The default name of this step. Can be overridden by any WorkflowStep to which this step belongs. */
  nameDefault: string;

  /** Whether an administrator is required to review/approve the completion of this step before it's considered done. */
  isAdminConfirmationRequiredDefault: boolean;

  /** The link to an external site if stepType is StepTypes.ExternalHttpTask; null otherwise. */
  externalHttpTaskLink?: string | null;
}
