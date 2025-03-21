import {WorkflowStepInterface} from "./workflow-step.interface";

/**
 * Represents a workflow, optionally overriding the default name derived from the in-scope job title.
 */
export interface WorkflowInterface {
  id: number;

  /** The name of this workflow, if desired to override the default name derived from the in-scope job title. */
  name: string | null;

  /** The collection of workflow steps associated with this workflow. */
  workflowSteps?: WorkflowStepInterface[] | null;
}
