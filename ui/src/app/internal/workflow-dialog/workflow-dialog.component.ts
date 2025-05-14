import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {Workflow} from "../../models/workflows/workflow";

export interface WorkflowDialogData {
  mode: 'edit' | 'copy' | 'create';
  workflow?: Workflow;
}

@Component({
  selector: 'app-workflow-dialog',
  templateUrl: './workflow-dialog.component.html',
  styleUrls: ['./workflow-dialog.component.scss']
})
export class WorkflowDialogComponent {
  workflow: Workflow;
  mode: 'edit' | 'copy' | 'create';

  constructor(
    public dialogRef: MatDialogRef<WorkflowDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public workflowDialogData: WorkflowDialogData
  ) {
    this.mode = workflowDialogData.mode;

    // Create a copy of the inbound workflow or initialize a new one.
    this.workflow = workflowDialogData.workflow ? {...workflowDialogData.workflow} : {id: 0, name: ''};

    if (this.mode === 'copy') {
      this.workflow.id = 0;
      this.workflow.copiedFromWorkflowId = workflowDialogData.workflow!.id;
      this.workflow.copySteps = true;
      this.workflow.name += ' (Copy)';
    }
  }

  save(): void {
    if (this.workflow.name?.trim()) {
      this.dialogRef.close(this.workflow);
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
