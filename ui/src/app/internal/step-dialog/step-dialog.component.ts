import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {Step} from "../../models/workflows/step";
import {StepTypes} from "../../models/workflows/step.types";
import {humanizeEnumKey} from "../../functions/string.functions";

@Component({
  selector: 'app-step-dialog',
  templateUrl: './step-dialog.component.html',
  styleUrls: ['step-dialog.component.scss']
})
export class StepDialogComponent {
  step: Step;

  stepTypeOptions = Object.entries(StepTypes)
    .filter(([, value]) => typeof value === 'number')
    .map(([key, value]) => ({label: humanizeEnumKey(key), value: value as number}));

  constructor(
    public dialogRef: MatDialogRef<StepDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Step
  ) {
    this.step = {...data};
  }

  onSave(): void {
    this.dialogRef.close(this.step);
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  protected readonly StepTypes = StepTypes;
}
