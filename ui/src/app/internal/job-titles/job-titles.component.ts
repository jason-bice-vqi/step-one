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

  selectedCompanyId: number | null = null;
  selectedJobTitleId: number | null = null;
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
  }

  onCompanyChange(): void {
    if (this.selectedCompanyId) {
      this.orgService.getJobTitles(this.selectedCompanyId).pipe(take(1)).subscribe((x) => this.jobTitles = x);
    }
  }

  onJobTitleChange(): void {
    if (!this.selectedJobTitleId) return;

    this.orgService.getJobTitleAliases(this.selectedJobTitleId)
      .pipe(take(1))
      .subscribe((x) => this.jobTitleAliases = x);

    this.jobTitleWorkflowService.showByJobTitle(this.selectedJobTitleId)
      .pipe(take(1))
      .subscribe(x => {
        this.selectedWorkflowId = x?.workflowId ?? null;
      });

    this.isWorkflowAssignmentDirty = false;
  }

  addAlias(): void {
    if (!this.selectedJobTitleId || !this.newAlias.trim()) {
      return;
    }

    this.orgService.createJobTitleAlias(this.selectedJobTitleId, this.newAlias)
      .pipe(take(1), tap((x) => {
        this.jobTitleAliases.push(x);
        this.newAlias = '';

        this.notificationService.success(`The alias '${x.alias}' has been added.`);
      }))
      .subscribe();
  }

  deleteAlias(jobTitleAlias: JobTitleAlias): void {
    const index = this.jobTitleAliases.indexOf(jobTitleAlias);

    if (index === -1) return;

    this.orgService.deleteJobTitleAlias(jobTitleAlias.id).pipe(take(1), tap((_) => {
      this.jobTitleAliases.splice(index, 1);

      this.notificationService.success(`The alias '${jobTitleAlias.alias}' has been deleted.`);
    })).subscribe();
  }

  onWorkflowChange() {
    this.isWorkflowAssignmentDirty = true;
  }

  saveWorkflowAssignment() {
    this.jobTitleWorkflowService.create(this.selectedJobTitleId!, this.selectedWorkflowId!)
      .pipe(take(1), tap(_ => {
        this.isWorkflowAssignmentDirty = false;

        this.notificationService.success("The workflow assignment has been updated.");
      }))
      .subscribe();
  }
}
