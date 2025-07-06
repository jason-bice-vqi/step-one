import {Component, OnInit} from '@angular/core';
import {OrgService} from "../../services/org.service";
import {take, tap} from "rxjs";
import {Company} from "../../models/org/company";
import {JobTitle} from "../../models/org/job-title";
import {JobTitleAlias} from "../../models/org/job-title-alias";
import {WorkflowService} from "../../services/workflows/workflow.service";
import {Workflow} from "../../models/workflows/workflow";
import {JobTitleWorkflowService} from "../../services/workflows/job-title-workflow.service";
import {NotificationService} from "../../services/notification.service";

@Component({
  selector: 'app-job-titles',
  templateUrl: './job-titles.component.html',
  styleUrls: ['./job-titles.component.scss']
})
export class JobTitlesComponent implements OnInit {

  selectedCompany: Company | null = null;
  selectedJobTitle: JobTitle | null = null;
  selectedWorkflow: Workflow | null = null;
  selectedWorkflowId: number | null = null;

  companies: Company[] = [];
  jobTitles: JobTitle[] = [];
  jobTitleAliases: JobTitleAlias[] = [];
  workflows: Workflow[] = [];

  newAlias: string = '';

  isWorkflowAssignmentDirty = false;

  constructor(private jobTitleWorkflowService: JobTitleWorkflowService,
              private notificationService: NotificationService,
              private orgService: OrgService,
              private workflowService: WorkflowService) {
  }

  ngOnInit(): void {
    this.orgService.getCompanies().pipe(take(1)).subscribe((x) => this.companies = x);
    this.workflowService.get().pipe(take(1)).subscribe((x) => this.workflows = x);

    this.notificationService.intro('Here you can manage aliases for job titles. Select a <strong>Company</strong>, then select a <strong>Job Title</strong> within that Company. Once a Job Title has been selected, you can manage its <strong>Aliases</strong> as well as assign its assigned <strong>Workflow</strong>.');
  }

  onCompanyChange(): void {
    if (this.selectedCompany) {
      this.orgService.getJobTitles(this.selectedCompany.id).pipe(take(1)).subscribe((x) => this.jobTitles = x);
    }
  }

  onJobTitleChange(): void {
    if (!this.selectedJobTitle) return;

    this.orgService.getJobTitleAliases(this.selectedJobTitle.id)
      .pipe(take(1))
      .subscribe((x) => this.jobTitleAliases = x);

    this.jobTitleWorkflowService.showByJobTitle(this.selectedJobTitle.id)
      .pipe(take(1))
      .subscribe(x => {
        this.selectedWorkflow = x?.workflow ?? null;
        this.selectedWorkflowId = x?.workflowId ?? null;
      });

    this.isWorkflowAssignmentDirty = false;
  }

  addAlias(): void {
    if (!this.selectedJobTitle?.id || !this.newAlias.trim()) return;

    this.orgService.createJobTitleAlias(this.selectedJobTitle.id, this.newAlias)
      .pipe(take(1), tap((x) => {
        this.jobTitleAliases.push(x);
        this.newAlias = '';

        this.notificationService.success(`The alias <strong>${x.alias}</strong> has been added.`);
      }))
      .subscribe();
  }

  deleteAlias(jobTitleAlias: JobTitleAlias): void {
    const index = this.jobTitleAliases.indexOf(jobTitleAlias);

    if (index === -1) return;

    this.orgService.deleteJobTitleAlias(jobTitleAlias.id).pipe(take(1), tap((_) => {
      this.jobTitleAliases.splice(index, 1);

      this.notificationService.success(`The alias <strong>${jobTitleAlias.alias}</strong> has been removed.`);
    })).subscribe();
  }

  onWorkflowChange() {
    this.isWorkflowAssignmentDirty = true;
  }

  saveWorkflowAssignment() {
    this.jobTitleWorkflowService.create(this.selectedJobTitle!.id, this.selectedWorkflowId!)
      .pipe(take(1), tap(_ => {
        this.isWorkflowAssignmentDirty = false;

        this.notificationService.success(`The workflow assignment for <strong>${this.selectedJobTitle?.displayTitle}</strong> at <strong>${this.selectedCompany?.name}</strong> has been assigned to the <strong>${this.selectedWorkflow?.name}</strong> workflow.`, 10000);
      }))
      .subscribe();
  }
}
