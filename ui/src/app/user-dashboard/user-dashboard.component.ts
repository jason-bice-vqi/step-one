import {Component, OnInit} from '@angular/core';
import {CandidateWorkflowService} from "../services/workflows/candidate-workflow.service";
import {CandidateWorkflowInterface} from "../models/candidate-workflow.interface";
import {take} from "rxjs";
import {JwtService} from "../services/auth/jwt.service";
import {CandidateWorkflowStepStatusesEnum} from "../models/candidate-workflow-step-statuses.enum";
import {CandidateWorkflowStepInterface} from "../models/candidate-workflow-step.interface";
import {StepTypesEnum} from "../models/step-types.enum";

@Component({
  selector: 'app-admin-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.scss']
})
export class UserDashboardComponent implements OnInit {

  candidateWorkflow?: CandidateWorkflowInterface | null = null;

  constructor(private jwtService: JwtService, private candidateWorkflowService: CandidateWorkflowService) {
  }

  ngOnInit(): void {
    const candidateId = this.jwtService.getCandidateId()!;

    this.candidateWorkflowService.get(candidateId)
      .pipe(take(1)).subscribe(x => this.candidateWorkflow = x.candidateWorkflow);
  }

  protected readonly CandidateWorkflowStepStatusesEnum = CandidateWorkflowStepStatusesEnum;

  onCompleteTask(cws: CandidateWorkflowStepInterface) {
    switch(cws.workflowStep?.step.stepType) {
      case StepTypesEnum.DataEntry:
        alert('Data Entry');
        return;
      case StepTypesEnum.ExternalHttpTask:
        alert('External HTTP Task');
        return;
      case StepTypesEnum.FileSubmission:
        alert('File Submission');
        return;
    }
  }

  handleUpload($event: File) {

  }

  handleFile($event: File) {

  }
}
