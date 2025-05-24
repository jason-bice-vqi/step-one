import {Pipe, PipeTransform} from '@angular/core';
import {WorkflowStep} from "../../models/workflows/workflow.step";

@Pipe({name: 'workflowStepName'})
export class WorkflowStepNamePipe implements PipeTransform {
  transform(item: WorkflowStep): string {
    return (item.stepNameOverride === '' ? null : item.stepNameOverride) ?? item.step.nameDefault;
  }
}
