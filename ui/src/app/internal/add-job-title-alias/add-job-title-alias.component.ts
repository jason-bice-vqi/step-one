import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {Company} from "../../models/org/company";
import {JobTitle} from "../../models/org/job-title";
import {Candidate} from "../../models/candidates/candidate";
import {Workflow} from "../../models/workflows/workflow";
import {CandidateOnboardingRequest} from "../../models/candidates/candidate-onboarding-request";

@Component({
  selector: 'app-add-job-title-alias',
  templateUrl: './add-job-title-alias.component.html',
  styleUrls: ['./add-job-title-alias.component.scss']
})
export class AddJobTitleAliasComponent implements OnInit {

  /**
   * The current candidate context, as passed in from the component that launched this one.
   */
  candidate: Candidate;

  /**
   * All companies, as passed in from the component that launched this one.
   */
  companies: Company[] = [];

  /**
   * All job titles, as passed in from the component that launched this one.
   */
  jobTitles: JobTitle[] = [];

  /**
   * All workflows, as passed in from the component that launched this one.
   */
  workflows: Workflow[] = [];

  /**
   * The currently selected company.
   */
  selectedCompany: Company | null = null;

  /**
   * The currently selected job title.
   */
  selectedJobTitle: JobTitle | null = null;

  /**
   * The currently selected workflow.
   */
  selectedWorkflow: Workflow | null = null;

  /**
   * Job titles filtered based on company selection and/or ATS job title match.
   */
  filteredJobTitles: JobTitle[] = [];

  /**
   * Whether ATS-matched job titles exist for the candidate's ATS job title.
   */
  atsMatchedJobTitlesExist = false;

  /**
   * Whether only ATS-matched job titles should be shown to the user.
   */
  showAtsMatchedJobTitles = false;

  /**
   * The request object used to onboard the candidate.
   */
  candidateOnboardingRequest: CandidateOnboardingRequest = {
    candidateId: 0,
    jobTitleId: 0,
    matchAtsJobTitleToOfficialJobTitle: false,
    matchWorkflowToJobTitle: false,
    sendOnboardingInvite: true,
    workflowId: 0
  };

  /**
   * Whether the job-title-match toggle should be disabled, which occurs when a match is already established between the
   * candidate's ATS job title and the currently selected job title. Such matches can be established via this component,
   * but cannot be removed via this component.
   */
  get disableJobTitleMatch(): boolean {
    return this.isAtsMatchedJobTitleSelected;
  }

  /**
   * Whether the workflow to job title assignment toggle should be disabled, which occurs when the selected workflow is
   * already assigned to a job title. Such matches can be established via this component, but cannot be removed via this
   * component.
   */
  get disableWorkflowMatch() {
    return this.selectedJobTitle?.workflowId !== null || this.selectedWorkflow === null;
  }

  /**
   * Indicates whether the currently selected job title is matched to the current candidate's ATS job title.
   */
  get isAtsMatchedJobTitleSelected(): boolean {
    if (!this.selectedJobTitle) return false;

    return this.getJobTitlesFilteredByAtsJobTitle().some(x => x.id === this.selectedJobTitle?.id);
  }

  constructor(
    public dialogRef: MatDialogRef<AddJobTitleAliasComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      candidate: Candidate,
      companies: Company[],
      jobTitles: JobTitle[],
      workflows: Workflow[]
    }
  ) {
    this.candidate = data.candidate;
    this.companies = data.companies;
    this.jobTitles = data.jobTitles;
    this.workflows = data.workflows;
    this.candidateOnboardingRequest.candidateId = this.candidate.id;
  }

  /**
   * If the candidate already has an assigned job title, we need to assign the company, filter the job titles by
   * company, select said job title, and select the job titles assigned workflow (if such an assignment exists).
   *
   * If not, we attempt to select a default based on an ATS match, or if more than one match exists, present only those
   * matches to the user.
   */
  ngOnInit(): void {
    if (this.candidate.jobTitle) {
      this.selectedCompany = this.companies.find(x => x.id === this.candidate.jobTitle!.company.id)!;

      this.updateFilteredJobTitles('company');

      this.selectedJobTitle = this.jobTitles.find(x => x.id === this.candidate.jobTitleId)!;

      if (this.candidate.candidateWorkflow?.workflow) {
        this.selectedWorkflow = this.workflows.find(x => x.id === this.candidate.candidateWorkflowId)!;
      }
    } else {
      this.updateFilteredJobTitles('ats-job-title');
    }
  }

  cancel(): void {
    this.dialogRef.close(null);
  }

  /**
   * Gets a filtered list of job titles based on matches between said job titles and the candidate's ATS job title.
   */
  getJobTitlesFilteredByAtsJobTitle(): JobTitle[] {
    return this.jobTitles
      .filter(x => x.jobTitleAliases.some(j => j.alias === this.candidate.atsJobTitle.toUpperCase()))
      .sort((a, b) => a.displayTitle.localeCompare(b.displayTitle));
  }

  /**
   * Updates the filtered job titles and triggers downstream logic, such as setting toggle defaults, workflow
   * assignment, etc.
   *
   * @param filterBy The nature of the filter to be applied. Job titles can be filtered by company, or they can be
   * filtered for ATS-matched titles only.
   */
  updateFilteredJobTitles(filterBy: 'company' | 'ats-job-title' | null = null): void {
    if (filterBy === null) {
      filterBy = this.showAtsMatchedJobTitles ? 'ats-job-title' : 'company';
    }

    if (filterBy === 'company') {
      this.showAtsMatchedJobTitles = false;

      this.filteredJobTitles = this.jobTitles
        .filter(x => x.company.id === this.selectedCompany!.id)
        .sort((a, b) => a.displayTitle.localeCompare(b.displayTitle));
    } else {
      this.showAtsMatchedJobTitles = true;
      this.selectedCompany = null;

      this.filteredJobTitles = this.getJobTitlesFilteredByAtsJobTitle();

      if (this.filteredJobTitles.length === 1) {
        this.selectedJobTitle = this.filteredJobTitles[0];
      }

      this.atsMatchedJobTitlesExist = this.filteredJobTitles.length > 0;

      if (this.atsMatchedJobTitlesExist) {
        this.candidateOnboardingRequest.matchAtsJobTitleToOfficialJobTitle = true;
      } else {
        this.updateFilteredJobTitles('company');
      }
    }

    this.applyJobTitleSelection();
  }

  /**
   * Applies logic based on the currently selected job title:
   *
   *   - Toggles the ATS job title match based on whether the currently selected title is already aliased.
   *   - Selects the job title's company.
   *   - Selects the workflow assigned to the job title, if such an assignment exists.
   */
  applyJobTitleSelection() {
    this.candidateOnboardingRequest.matchAtsJobTitleToOfficialJobTitle = this.isAtsMatchedJobTitleSelected;
    this.candidateOnboardingRequest.matchWorkflowToJobTitle = false;

    if (this.selectedJobTitle === null) {
      this.selectedWorkflow = null;

      return;
    }

    this.selectedCompany = this.companies.find(x => x.id === this.selectedJobTitle!.company!.id)!;

    if (this.selectedJobTitle.workflowId === null) {
      this.selectedWorkflow = null;
    } else {
      this.selectedWorkflow = this.workflows.find(x => x.id === this.selectedJobTitle?.workflowId!)!;

      this.applyWorkflowSelection();
    }
  }

  applyWorkflowSelection() {
    if (this.selectedJobTitle === null || this.selectedWorkflow === null) {
      this.candidateOnboardingRequest.matchWorkflowToJobTitle = false;

      return;
    }

    this.candidateOnboardingRequest.matchWorkflowToJobTitle =
      this.selectedJobTitle.workflowId === this.selectedWorkflow.id;
  }
}
