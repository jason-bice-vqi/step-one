import {Component, OnInit} from '@angular/core';
import {CandidateWorkflowService} from "../services/workflows/candidate-workflow.service";
import {CandidateWorkflow} from "../models/workflows/candidate.workflow";
import {take} from "rxjs";
import {JwtService} from "../services/auth/jwt.service";
import {CandidateWorkflowStep} from "../models/workflows/candidate-workflow.step";
import {StepTypes} from "../models/workflows/step.types";
import {CandidateWorkflowStatuses} from "../models/candidates/candidate-workflow.statuses";

@Component({
  selector: 'app-internal-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.scss']
})
export class UserDashboardComponent implements OnInit {

  candidateWorkflow?: CandidateWorkflow | null = null;

  constructor(private jwtService: JwtService, private candidateWorkflowService: CandidateWorkflowService) {
  }

  ngOnInit(): void {
    const candidateId = this.jwtService.getCandidateId()!;

    this.candidateWorkflowService.get(candidateId)
      .pipe(take(1)).subscribe(x => this.candidateWorkflow = x.candidateWorkflow);
  }

  protected readonly CandidateWorkflowStepStatusesEnum = CandidateWorkflowStatuses;

  onCompleteTask(cws: CandidateWorkflowStep) {
    switch(cws.workflowStep?.step.stepType) {
      case StepTypes.DataEntry:
        alert('Data Entry');
        return;
      case StepTypes.ExternalHttpTask:
        alert('External HTTP Task');
        return;
      case StepTypes.FileSubmission:
        alert('File Submission');
        return;
    }
  }

  handleUpload($event: File) {

  }

  handleFile($event: File) {

  }
}
