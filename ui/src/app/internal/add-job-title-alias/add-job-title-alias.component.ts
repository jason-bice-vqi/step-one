import {ChangeDetectorRef, Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {Company} from "../../models/org/company";
import {JobTitle} from "../../models/org/job-title";
import {OrgService} from "../../services/org.service";
import {take} from "rxjs";
import {Candidate} from "../../models/candidates/candidate";
import {Workflow} from "../../models/workflows/workflow";

@Component({
  selector: 'app-add-job-title-alias',
  templateUrl: './add-job-title-alias.component.html',
  styleUrls: ['./add-job-title-alias.component.scss']
})
export class AddJobTitleAliasComponent implements OnInit {
  candidate: Candidate;
  companies: Company[] = [];
  jobTitles: JobTitle[] = [];
  workflows: Workflow[] = [];
  selectedCompany: Company | null = null;
  selectedJobTitle: JobTitle | null = null;
  selectedWorkflow: Workflow | null = null;

  get filteredJobTitles(): JobTitle[] {
    if (this.selectedCompany === null) return [];

    return this.jobTitles
      .filter(x => x.company.id === this.selectedCompany!.id)
      .sort((a, b) => a.displayTitle.localeCompare(b.displayTitle));
  }

  constructor(
    public changeDet: ChangeDetectorRef,
    public dialogRef: MatDialogRef<AddJobTitleAliasComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      candidate: Candidate,
      companies: Company[],
      jobTitles: JobTitle[],
      workflows: Workflow[]
    },
    private orgService: OrgService
  ) {
    this.candidate = data.candidate;
    this.companies = data.companies;
    this.jobTitles = data.jobTitles;
    this.workflows = data.workflows;
  }

  ngOnInit(): void {
    console.info('Onboarding Candidate', this.candidate);

    if (this.candidate.jobTitle) {
      this.selectedCompany = this.companies.find(x => x.id === this.candidate.jobTitle!.company.id)!;
      this.selectedJobTitle = this.jobTitles.find(x => x.id === this.candidate.jobTitleId)!;
    }

    if (this.candidate.candidateWorkflow?.workflow) {
      this.selectedWorkflow = this.workflows.find(x => x.id === this.candidate.candidateWorkflowId)!;
    }

    // TODO: Needed?
    this.changeDet.markForCheck();
    this.changeDet.detectChanges();
  }

  createAlias(): void {
    this.orgService.createJobTitleAlias(this.selectedJobTitle!.id, this.candidate.atsJobTitle)
      .pipe((take(1))).subscribe(x => {
      this.dialogRef.close(x);
    });
  }

  cancel(): void {
    this.dialogRef.close(null);
  }
}
