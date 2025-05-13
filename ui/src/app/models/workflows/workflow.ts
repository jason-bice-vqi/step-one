import {WorkflowStep} from "./workflow.step";

/**
 * Represents a workflow, optionally overriding the default name derived from the in-scope job title.
 */
export interface Workflow {
  id: number;

  /** The name of this workflow, if desired to override the default name derived from the in-scope job title. */
  name: string | null;

  /** The collection of workflow steps associated with this workflow. */
  workflowSteps?: WorkflowStep[] | null;
}
