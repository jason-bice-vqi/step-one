import {Component, OnInit} from '@angular/core';
import {CandidateWorkflowService} from "../services/workflows/candidate-workflow.service";
import {CandidateWorkflowInterface} from "../../models/candidate-workflow.interface";
import {take} from "rxjs";
import {JwtService} from "../services/auth/jwt.service";

@Component({
  selector: 'app-admin-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.scss']
})
export class UserDashboardComponent implements OnInit {
  steps = [
    {
      title: 'Step 1: Offer Letter',
      description: 'Sign offer letter.',
      buttonText: 'Completed',
      status: 'complete'
    },
    {
      title: 'Step 2: Complete Employment Application',
      description: 'Complete and sign Employment Application.',
      buttonText: 'Get Started',
      status: 'pending'
    },
    {
      title: 'Step 3: Verification of Compliance',
      description: 'Complete and sign Verification of Compliance form.',
      buttonText: 'Verify',
      status: 'blocked'
    },
    {
      title: 'Step 4: Pre-Employment Form',
      description: 'Complete and sign Pre-Employment form.',
      buttonText: 'Get Started',
      status: 'blocked'
    }
  ];

  onAction(step: any) {
    // Handle action (e.g., route to form, call API, etc.)
    console.log(`Action for: ${step.title}`);
  }

  candidateWorkflow?: CandidateWorkflowInterface;

  constructor(private jwtService: JwtService, private candidateWorkflowService: CandidateWorkflowService) {
  }

  ngOnInit(): void {
    const candidateId = this.jwtService.getCandidateId()!;

    this.candidateWorkflowService.get(candidateId).pipe(take(1)).subscribe(x => this.candidateWorkflow = x);
  }
}
