import {WorkflowInterface} from "./workflow.interface";
import {StepInterface} from "./step.interface";

/**
 * Relates a Step to a Workflow, providing workflow-specific customization and configuration.
 */
export interface WorkflowStepInterface {
  id: number;

  /** The ID of the workflow with which this step is associated. */
  workflowId: number;

  /** The workflow with which this step is associated. */
  workflow?: WorkflowInterface | null;

  /** The ID of the step with which this workflow is associated. */
  stepId: number;

  /** The step with which this workflow is associated. */
  step: StepInterface;

  /** Whether an administrator is required to review/approve the completion of this step before it's considered done. */
  isAdminConfirmationRequired: boolean;

  /** An override, if desired, of Step.NameDefault. */
  stepNameOverride?: string | null;

  /** The step name, with stepNameOverride considered. */
  stepName: string;

  /** The display order of this step within the workflow. */
  stepIndex: number;
}
