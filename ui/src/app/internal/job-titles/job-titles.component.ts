import {Component, OnInit} from '@angular/core';
import {OrgService} from "../../services/org.service";
import {filter, forkJoin, map, switchMap, take, tap} from "rxjs";
import {Company} from "../../models/org/company";
import {JobTitle} from "../../models/org/job-title";
import {JobTitleAlias} from "../../models/org/job-title-alias";
import {WorkflowService} from "../../services/workflows/workflow.service";
import {Workflow} from "../../models/workflows/workflow";
import {NotificationService} from "../../services/notification.service";
import {JobTitleService} from "../../services/job-title.service";
import {ActivatedRoute} from "@angular/router";

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
  allJobTitles: JobTitle[] = [];
  jobTitlesFilteredByCompany: JobTitle[] = [];
  jobTitleAliases: JobTitleAlias[] = [];
  workflows: Workflow[] = [];

  newAlias: string = '';

  isWorkflowAssignmentDirty = false;

  constructor(private activatedRoute: ActivatedRoute,
              private jobTitleService: JobTitleService,
              private notificationService: NotificationService,
              private orgService: OrgService,
              private workflowService: WorkflowService) {
  }

  ngOnInit(): void {
    forkJoin({
      jobTitles: this.orgService.getJobTitles().pipe(take(1)),
      companies: this.orgService.getCompanies().pipe(take(1)),
      workflows: this.workflowService.get().pipe(take(1)),
    }).pipe(
      tap(({jobTitles, companies, workflows}) => {
        this.allJobTitles = jobTitles;
        this.companies = companies;
        this.workflows = workflows;
      }),
      switchMap(() => this.activatedRoute.paramMap),
      map(params => params.get('id')),
      filter((id): id is string => !!id),
      tap((jobTitleIdStr) => {
        const jobTitleId = parseInt(jobTitleIdStr);
        const jobTitle = this.allJobTitles.filter(x => x.id === jobTitleId)[0];
        const company = this.companies.filter(x => x.id === jobTitle.company.id)[0];

        this.selectedCompany = company;
        this.jobTitlesFilteredByCompany = this.allJobTitles.filter(x => x.company.id === company.id);
        this.selectedJobTitle = jobTitle;
      })
    ).subscribe();
  }

  onCompanyChange(): void {
    if (this.selectedCompany) {
      this.jobTitlesFilteredByCompany = this.allJobTitles.filter(x => x.company.id === this.selectedCompany?.id);
    }
  }

  onJobTitleChange(): void {
    if (!this.selectedJobTitle) return;

    this.orgService.getJobTitleAliases(this.selectedJobTitle.id)
      .pipe(take(1))
      .subscribe((x) => this.jobTitleAliases = x);

    this.selectedWorkflow = this.selectedJobTitle.workflow ?? null;
    this.selectedWorkflowId = this.selectedJobTitle.workflowId ?? null;
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
    this.jobTitleService.updateWorkflow(this.selectedJobTitle!.id, this.selectedWorkflowId!)
      .pipe(take(1), tap(_ => {
        this.isWorkflowAssignmentDirty = false;

        this.notificationService.success(`The workflow assignment for <strong>${this.selectedJobTitle?.displayTitle}</strong> at <strong>${this.selectedCompany?.name}</strong> has been assigned to the <strong>${this.selectedWorkflow?.name}</strong> workflow.`, 10000);
      }))
      .subscribe();
  }
}
