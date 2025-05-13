import {Workflow} from "./workflow";
import {Step} from "./step";

/**
 * Relates a Step to a Workflow, providing workflow-specific customization and configuration.
 */
export interface WorkflowStep {
  id: number;

  /** The ID of the workflow with which this step is associated. */
  workflowId: number;

  /** The workflow with which this step is associated. */
  workflow?: Workflow | null;

  /** The ID of the step with which this workflow is associated. */
  stepId: number;

  /** The step with which this workflow is associated. */
  step: Step;

  /** Whether an administrator is required to review/approve the completion of this step before it's considered done. */
  isAdminConfirmationRequired: boolean;

  /** An override, if desired, of Step.NameDefault. */
  stepNameOverride?: string | null;

  /** The step name, with stepNameOverride considered. */
  stepName: string;

  /** The display order of this step within the workflow. */
  stepIndex: number;

  /** Whether this step should block downstream steps until it is completed. **/
  blockDownstream: boolean;

  /** Whether this step requires administrator confirmation to be considered completed. **/
  requiresAdminConfirmation: boolean;
}
