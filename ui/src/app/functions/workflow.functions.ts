import {Workflow} from "../models/workflows/workflow";

export function sortWorkflows(workflows: Workflow[]) {
  workflows.sort((a, b) => a.name! < b.name! ? 0 : 1);
}

export function sortWorkflowSteps(workflow: Workflow) {
  workflow.workflowSteps!.sort((a, b) => a.stepIndex < b.stepIndex ? 0 : 1);
}
