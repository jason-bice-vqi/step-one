import {Component, Input} from '@angular/core';
import {CandidateWorkflowStepStatuses} from "../models/workflows/candidate-workflow-step.statuses";
import {CandidateWorkflowStep} from "../models/workflows/candidate-workflow.step";
import {StepTypes} from "../models/workflows/step.types";
import {animate, keyframes, style, transition, trigger} from "@angular/animations";

@Component({
  selector: 'app-candidate-workflow-step',
  templateUrl: './candidate-workflow-step.component.html',
  styleUrls: ['./candidate-workflow-step.component.scss'],
  animations: [
    trigger('revealAnimation', [
      transition(':enter', [
        animate('1500ms ease-out', keyframes([
          style({
            opacity: 0,
            transform: 'scale(0.9)',
            boxShadow: '0 0 0px rgba(255, 255, 255, 0)',
            offset: 0
          }),
          style({
            opacity: 1,
            transform: 'scale(1.05)',
            boxShadow: '0 0 40px rgba(255, 255, 255, 1)',
            offset: 0.15
          }),
          style({
            transform: 'scale(1.03)',
            boxShadow: '0 0 30px rgba(255, 255, 255, 0.8)',
            offset: 0.35
          }),
          style({
            transform: 'scale(1)',
            boxShadow: '0 0 0px rgba(255, 255, 255, 0)',
            offset: 0.5
          })
        ]))
      ])
    ])
  ]
})
export class CandidateWorkflowStepComponent {
  @Input() cws!: CandidateWorkflowStep;

  protected readonly CandidateWorkflowStepStatusesEnum = CandidateWorkflowStepStatuses;
  protected readonly StepTypesEnum = StepTypes;

  taskStarted: boolean = false;
  isConfirmed: boolean = false;

  onCompleteTask(cws: CandidateWorkflowStep) {

  }

  handleFile($event: File) {

  }

  onStartTask() {
    this.taskStarted = !this.taskStarted;
  }
}
