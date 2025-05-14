import {WorkflowStep} from "./workflow.step";

/**
 * Represents a workflow, optionally overriding the default name derived from the in-scope job title.
 */
export interface Workflow {
  id: number;

  /** The name of this workflow, if desired to override the default name derived from the in-scope job title. */
  name: string | null;

  /** The workflow from which this workflow was copied, if this was a copied workflow. */
  copiedFromWorkflowId?: number | null;

  /** Whether the job assignments should be copied if this workflow is being generated from another workflow. */
  copyJobAssignments?: boolean | false;

  /** Whether the steps should be copied if this workflow is being generated from another workflow. */
  copySteps?: boolean | false;

  /** The collection of workflow steps associated with this workflow. */
  workflowSteps?: WorkflowStep[] | null;
}
