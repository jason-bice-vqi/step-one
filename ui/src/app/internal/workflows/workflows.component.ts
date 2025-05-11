import {Component} from '@angular/core';
import {
  WorkflowStepConfig,
  WorkflowStepConfigDialogComponent,
} from "../workflow-step-config-dialog/workflow-step-config-dialog.component";
import {MatDialog} from "@angular/material/dialog";
import {ConfirmDeleteDialogComponent} from "../../confirm-delete-dialog/confirm-delete-dialog.component";
import {take} from "rxjs";

interface WorkflowStep {
  id: number;
  name: string;
  blockDownstream: boolean;
  requiresAdminApproval: boolean;
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
    {id: 1, name: 'Workflow A', steps: []},
    {id: 2, name: 'Workflow B', steps: []}
  ];
  stepPool: WorkflowStep[] = [
    {id: 1, name: 'Job Application', blockDownstream: false, requiresAdminApproval: false},
    {id: 2, name: 'Upload Drivers License', blockDownstream: false, requiresAdminApproval: false},
    {id: 3, name: 'Upload Tax Documents', blockDownstream: false, requiresAdminApproval: false}
  ];

  selectedWorkflow: Workflow | null = null;
  newWorkflowName = '';

  constructor(private dialog: MatDialog) {

  }


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

    const dialogRef = this.dialog.open(ConfirmDeleteDialogComponent, {
      data: {
        message: `Are you sure you want to delete the <strong>${step.name}</strong> step from the <strong>${this.selectedWorkflow.name}</strong> workflow?`
      }
    });

    dialogRef.afterClosed().pipe(take(1)).subscribe(result => {
      if (result) {
        this.stepPool.push(step);
        this.selectedWorkflow!.steps = this.selectedWorkflow!.steps.filter(s => s.id !== step.id);
      }
    });
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

  configureStep(step: WorkflowStep) {
    const dialogRef = this.dialog.open(WorkflowStepConfigDialogComponent, {
      width: '400px',
      data: {
        name: step.name,
        blockDownstream: step.blockDownstream || false,
        requiresAdminApproval: step.requiresAdminApproval || false
      }
    });

    dialogRef.afterClosed().pipe(take(1)).subscribe((result: WorkflowStepConfig | undefined) => {
      if (result) {
        step.name = result.name;
        step.blockDownstream = result.blockDownstream;
        step.requiresAdminApproval = result.requiresAdminApproval;
      }
    });
  }

  deleteWorkflow(selectedWorkflow: Workflow) {
    const dialogRef = this.dialog.open(ConfirmDeleteDialogComponent, {
      data: {
        message: `Are you sure you want to delete the <strong>${selectedWorkflow.name}</strong> workflow?`
      }
    });

    dialogRef.afterClosed().pipe(take(1)).subscribe(result => {
      if (result) {
        // Proceed with delete
      }
    });
  }
}
