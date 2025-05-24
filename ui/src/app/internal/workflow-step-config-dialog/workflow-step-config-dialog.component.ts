import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {WorkflowStep} from "../../models/workflows/workflow.step";

@Component({
  selector: 'app-workflow-step-config-modal',
  templateUrl: './workflow-step-config-dialog.component.html',
  styleUrls: ['./workflow-step-config-dialog.component.scss']
})
export class WorkflowStepConfigDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<WorkflowStepConfigDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: WorkflowStep
  ) {}

  save() {
    this.dialogRef.close(this.data);
  }

  clearStepNameOverride() {
    this.data.stepNameOverride = null;
  }
}
