import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface WorkflowStepConfig {
  name: string;
  blockDownstream: boolean;
  requiresAdminApproval: boolean;
}

@Component({
  selector: 'app-workflow-step-config-modal',
  templateUrl: './workflow-step-config-dialog.component.html',
  styleUrls: ['./workflow-step-config-dialog.component.scss']
})
export class WorkflowStepConfigDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<WorkflowStepConfigDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: WorkflowStepConfig
  ) {}

  save() {
    this.dialogRef.close(this.data);
  }
}
