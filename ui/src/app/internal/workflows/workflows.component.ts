import { Component } from '@angular/core';

interface WorkflowStep {
  id: number;
  name: string;
}

interface Workflow {
  id: number;
  name: string;
  steps: WorkflowStep[];
}

@Component({
  selector: 'app-workflows',
  templateUrl: './workflows.component.html',
  styleUrls: ['./workflows.component.scss']
})
export class WorkflowsComponent {
  workflows: Workflow[] = [
    { id: 1, name: 'Workflow A', steps: [] },
    { id: 2, name: 'Workflow B', steps: [] }
  ];
  stepPool: WorkflowStep[] = [
    { id: 1, name: 'Job Application' },
    { id: 2, name: 'Upload Drivers License' },
    { id: 3, name: 'Upload Tax Documents' }
  ];

  selectedWorkflow: Workflow | null = null;
  newWorkflowName = '';

  selectWorkflow(workflow: Workflow) {
    this.selectedWorkflow = workflow;
  }

  createWorkflow() {
    if (!this.newWorkflowName.trim()) return;

    const newWorkflow: Workflow = {
      id: this.workflows.length + 1,
      name: this.newWorkflowName,
      steps: []
    };
    this.workflows.push(newWorkflow);
    this.newWorkflowName = '';
    this.selectWorkflow(newWorkflow);
  }

  addStepToWorkflow(step: WorkflowStep) {
    if (!this.selectedWorkflow) return;

    this.selectedWorkflow.steps.push(step);
    this.stepPool = this.stepPool.filter(s => s.id !== step.id);
  }

  removeStepFromWorkflow(step: WorkflowStep) {
    if (!this.selectedWorkflow) return;

    this.stepPool.push(step);
    this.selectedWorkflow.steps = this.selectedWorkflow.steps.filter(s => s.id !== step.id);
  }

  moveStepUp(index: number) {
    if (index === 0 || !this.selectedWorkflow) return;
    const steps = this.selectedWorkflow.steps;
    [steps[index - 1], steps[index]] = [steps[index], steps[index - 1]];
  }

  moveStepDown(index: number) {
    if (!this.selectedWorkflow) return;
    const steps = this.selectedWorkflow.steps;
    if (index >= steps.length - 1) return;
    [steps[index], steps[index + 1]] = [steps[index + 1], steps[index]];
  }
}
