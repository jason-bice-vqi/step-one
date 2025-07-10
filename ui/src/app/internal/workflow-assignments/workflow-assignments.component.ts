import {Component, OnInit} from '@angular/core';
import {Workflow} from "../../models/workflows/workflow";
import {OrgService} from "../../services/org.service";
import {filter, switchMap, take, tap} from "rxjs";
import {WorkflowService} from "../../services/workflows/workflow.service";
import {CompanyJobTitle} from "../../models/org/company-job-title";
import {ActivatedRoute, Router} from "@angular/router";
import {JobTitle} from "../../models/org/job-title";
import {JobTitleService} from "../../services/job-title.service";
import {ConfirmDeleteDialogComponent} from "../../shared/confirm-delete-dialog/confirm-delete-dialog.component";
import {MatDialog} from "@angular/material/dialog";
import {MatCheckboxChange} from "@angular/material/checkbox";
import {NotificationService} from "../../services/notification.service";

@Component({
  selector: 'app-workflow-assignments',
  templateUrl: './workflow-assignments.component.html',
  styleUrls: ['./workflow-assignments.component.scss']
})
export class WorkflowAssignmentsComponent implements OnInit {
  activeWorkflow: Workflow | null = null;
  activeWorkflowId: number | null = null;
  selectedWorkflows: Workflow[] | null = null;
  companyJobTitles: CompanyJobTitle[] = [];
  workflows: Workflow[] = [];
  expandedCompanies = new Set<number>();

  constructor(private activatedRoute: ActivatedRoute,
              private dialog: MatDialog,
              private orgService: OrgService,
              private jobTitleService: JobTitleService,
              private notificationService: NotificationService,
              private router: Router,
              private workflowService: WorkflowService) {
  }

  ngOnInit(): void {
    this.loadDataFromRoute();
  }

  loadDataFromRoute() {
    this.activatedRoute.paramMap.subscribe(params => {
      this.orgService.getJobTitlesGroupedByCompany().pipe(take(1)).subscribe(x => this.companyJobTitles = x);

      this.workflowService.get()
        .pipe(
          take(1),
          tap((x) => {
            this.workflows = x;

            const paramWorkflowId = params.get('id');

            if (!paramWorkflowId) {
              this.notificationService.info('Please select a workflow to manage its job title assignments.');

              return;
            }

            this.activeWorkflowId = +paramWorkflowId;
          }),
          filter(() => this.activeWorkflowId !== null))
        .subscribe(x => {
          this.activeWorkflow = x.filter(w => w.id == this.activeWorkflowId)[0];
          this.selectedWorkflows = [this.activeWorkflow];
        });
    });
  }

  onWorkflowSelected(): void {
    this.router.navigate(['/internal', 'workflows', this.selectedWorkflows![0].id, 'assignments']);
  }

  toggleCompany(companyId: number): void {
    if (this.expandedCompanies.has(companyId)) {
      this.expandedCompanies.delete(companyId);
    } else {
      this.expandedCompanies.add(companyId);
    }
  }

  isExpanded(companyId: number): boolean {
    return this.expandedCompanies.has(companyId);
  }

  setJobTitleWorkflow(event: MatCheckboxChange, jobTitle: JobTitle) {
    const workflowId = event.checked ? this.activeWorkflowId! : null;

    if (jobTitle.workflowId !== null && jobTitle.workflowId !== this.activeWorkflowId) {
      this.dialog.open(ConfirmDeleteDialogComponent, {
        data: {
          message: `<p>The job title <strong>${jobTitle.displayTitle}</strong> is currently assigned to the <strong>${jobTitle.workflow?.name}</strong> workflow.</p><p>Are you sure you want to reassign this job title to the <strong>${this.activeWorkflow?.name}</strong> workflow?</p>`,
          noText: 'Cancel',
          yesText: 'Reassign',
          title: 'Job Title Already Assigned To Workflow'
        }
      }).afterClosed().pipe(
        take(1),
        tap(x => {
          if (!x) {
            // User cancelled, so reload state. This is the easiest solution to cancelling the check. Alternatively, can
            // use (click) with EventArgs.preventDefault, but then the checked state for each item needs to be tracked.
            // This is simpler.
            this.loadDataFromRoute();
          }
        }),
        filter(x => !!x),
        switchMap(() => this.jobTitleService.updateWorkflow(jobTitle.id, workflowId)),
        tap(x => {
          jobTitle.workflowId = x.workflowId;
          this.notifyOfAssignmentChange(jobTitle);
        })
      ).subscribe();
    } else {
      this.jobTitleService.updateWorkflow(jobTitle.id, workflowId)
        .pipe(take(1), tap(x => {
          jobTitle.workflowId = x.workflowId;
          this.notifyOfAssignmentChange(jobTitle);
        }))
        .subscribe();
    }
  }

  private notifyOfAssignmentChange(jobTitle: JobTitle) {
    if (jobTitle.workflowId) {
      this.notificationService.success(`<strong>${jobTitle.displayTitle}</strong> has been assigned to the <strong>${this.activeWorkflow!.name}</strong> workflow.`);
    } else {
      this.notificationService.warn(`<strong>${jobTitle.displayTitle}</strong> is no longer assigned to the <strong>${this.activeWorkflow!.name}</strong> workflow.`);
    }
  }

  getAssignmentCount(companyJobTitle: CompanyJobTitle | null = null): number {
    if (companyJobTitle === null) {
      return this.companyJobTitles.flatMap(x => x.jobTitles).filter(x => x.workflowId == this.activeWorkflowId).length;
    }

    return companyJobTitle.jobTitles.filter(x => x.workflowId === this.activeWorkflowId).length;
  }
}
