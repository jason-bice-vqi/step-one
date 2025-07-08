import {Component, OnInit} from '@angular/core';
import {WorkflowStepConfigDialogComponent,} from "../workflow-step-config-dialog/workflow-step-config-dialog.component";
import {MatDialog} from "@angular/material/dialog";
import {ConfirmDeleteDialogComponent} from "../../shared/confirm-delete-dialog/confirm-delete-dialog.component";
import {filter, switchMap, take, tap} from "rxjs";
import {Workflow} from "../../models/workflows/workflow";
import {WorkflowStep} from "../../models/workflows/workflow.step";
import {WorkflowService} from "../../services/workflows/workflow.service";
import {StepService} from "../../services/workflows/step.service";
import {Step} from "../../models/workflows/step";
import {StepDialogComponent} from "../step-dialog/step-dialog.component";
import {WorkflowDialogComponent} from "../workflow-dialog/workflow-dialog.component";
import {sortWorkflows, sortWorkflowSteps} from "../../functions/workflow.functions";
import {Router} from "@angular/router";

@Component({
  selector: 'app-workflows',
  templateUrl: './workflows.component.html',
  styleUrls: ['./workflows.component.scss']
})
export class WorkflowsComponent implements OnInit {
  /** All workflows. **/
  workflows: Workflow[] | null = null;

  /** All potential steps. **/
  steps: Step[] | null = null;

  /** Filtered/sorted steps available for the currently selected workflow. **/
  stepPool: Step[] | null = null;

  /** The currently selected workflow. **/
  selectedWorkflow: Workflow | null = null;

  /** Whether the configuration of the currently selected workflow's steps contains unsaved changes. **/
  workflowStepsDirty = false;

  constructor(private dialog: MatDialog,
              private router: Router,
              private stepService: StepService,
              private workflowService: WorkflowService) {
  }

  ngOnInit(): void {
    this.loadSteps();
    this.loadWorkflows();
  }

  loadSteps() {
    this.steps = null;
    this.stepPool = null;

    this.stepService.get()
      .pipe(take(1))
      .subscribe((x) => {
        this.steps = x;
        this.refreshLocalStepPool();
      });
  }

  loadWorkflows(selectWorkflow?: Workflow | null): void {
    this.workflows = null;
    this.selectedWorkflow = null;

    this.workflowService.get()
      .pipe(take(1))
      .subscribe(workflows => {
        sortWorkflows(workflows);

        this.workflows = workflows;
        this.selectedWorkflow = null;

        if (selectWorkflow) {
          selectWorkflow = workflows.filter(w => w.id === selectWorkflow!.id)[0];

          this.selectWorkflow(selectWorkflow!);
        }

        this.refreshLocalStepPool();
      });
  }

  refreshLocalStepPool() {
    const workflowSteps: WorkflowStep[] = this.selectedWorkflow?.workflowSteps ?? [];
    const steps: Step[] = workflowSteps.map(x => x.step);
    const stepIds: number[] = steps.map(x => x.id);

    this.stepPool = this.steps!
      .filter(x => stepIds.indexOf(x.id) === -1)
      .sort((a, b) => a.nameDefault.localeCompare(b.nameDefault));
  }

  confirmSelectWorkflow(workflow: Workflow) {
    if (this.workflowStepsDirty) {
      this.dialog.open(ConfirmDeleteDialogComponent, {
        data: {
          message: `You have unsaved changes on the workflow <strong>${this.selectedWorkflow!.name}</strong>, and if you select a new workflow those changes will be lost.`,
          title: 'Unsaved Changes',
          yesText: 'Discard Changes',
          noText: 'Cancel'
        }
      }).afterClosed().pipe(
        take(1),
        filter(result => !!result),
        tap(() => {
          this.selectWorkflow(workflow);
        })
      ).subscribe();
    } else {
      this.selectWorkflow(workflow);
    }
  }

  private selectWorkflow(workflow: Workflow) {
    sortWorkflowSteps(workflow);

    this.workflowStepsDirty = false;

    // Clone the workflow so there is consistent behavior if the selected workflow changes after changes were made and
    // then discarded.
    this.selectedWorkflow = structuredClone(workflow);

    this.refreshLocalStepPool();
  }

  createWorkflow() {
    this.manageWorkflow('create');
  }

  copyWorkflow(workflow: Workflow) {
    this.manageWorkflow('copy', workflow);
  }

  editWorkflow(workflow: Workflow) {
    this.manageWorkflow('edit', workflow);
  }

  manageWorkflow(mode: 'copy' | 'create' | 'edit', workflow?: Workflow) {
    this.dialog.open(WorkflowDialogComponent, {
      width: '600px',
      data: {mode, workflow}
    }).afterClosed().pipe(
      filter((result): result is Workflow => !!result),
      switchMap(result => {
        if (result.id) {
          return this.workflowService.update(result);
        } else {
          return this.workflowService.create(result);
        }
      }),
      take(1),
      tap((x) => this.loadWorkflows(x))
    ).subscribe();
  }

  saveSelectedWorkflow() {
    this.workflowService.update(this.selectedWorkflow!)
      .pipe(
        take(1),
        tap((updatedWorkflow: Workflow) => {
          const index = this.workflows!.findIndex(w => w.id === updatedWorkflow.id);

          this.workflows![index] = updatedWorkflow;
          this.selectWorkflow(updatedWorkflow);
        })
      )
      .subscribe();
  }

  createNewStep() {
    this.dialog.open(StepDialogComponent, {width: '600px'}).afterClosed().pipe(
      take(1),
      filter(result => !!result),
      switchMap(result => this.stepService.create(result)),
      tap((x) => {
        this.steps!.push(x);
        this.refreshLocalStepPool();
      })
    ).subscribe();
  }

  editStep(step: Step) {
    this.dialog.open(StepDialogComponent, {data: step, width: '600px'}).afterClosed().pipe(
      take(1),
      filter(result => !!result),
      switchMap(result => this.stepService.update(result)),
      tap((updatedStep) => {
        const index = this.steps!.findIndex(s => s.id === step.id);

        this.steps![index] = updatedStep;

        this.refreshLocalStepPool();
      })
    ).subscribe();
  }

  addStepToWorkflow(step: Step) {
    if (!this.selectedWorkflow) return;

    this.workflowStepsDirty = true;

    const workflowStep: WorkflowStep = {
      blockDownstream: false,
      id: 0,
      isAdminConfirmationRequired: false,
      step: step,
      stepId: step.id,
      stepIndex: 0,
      stepName: step.nameDefault,
      workflowId: this.selectedWorkflow.id
    };

    this.selectedWorkflow.workflowSteps!.push(workflowStep);
    this.refreshLocalStepPool();
  }

  removeStepFromWorkflow(workflowStep: WorkflowStep) {
    if (!this.selectedWorkflow) return;

    this.workflowStepsDirty = true;

    this.selectedWorkflow!.workflowSteps = this.selectedWorkflow!.workflowSteps!.filter(s => s.id !== workflowStep.id);

    this.refreshLocalStepPool();
  }

  moveStepUp(index: number) {
    if (index === 0 || !this.selectedWorkflow) return;

    this.workflowStepsDirty = true;

    const steps = this.selectedWorkflow.workflowSteps!;

    [steps[index - 1], steps[index]] = [steps[index], steps[index - 1]];
  }

  moveStepDown(index: number) {
    if (!this.selectedWorkflow) return;

    this.workflowStepsDirty = true;

    const steps = this.selectedWorkflow.workflowSteps!;

    if (index >= steps.length - 1) return;

    [steps[index], steps[index + 1]] = [steps[index + 1], steps[index]];
  }

  editWorkflowStep(workflowStep: WorkflowStep) {
    const workflowStepCopy = structuredClone(workflowStep);

    this.dialog.open(WorkflowStepConfigDialogComponent, {
      width: '600px',
      data: workflowStepCopy
    }).afterClosed().pipe(
      take(1),
      filter((result): result is WorkflowStep => !!result),
      tap(result => {
        this.workflowStepsDirty = true;

        const workflowStepIndex = this.selectedWorkflow!.workflowSteps!.findIndex(x => x.id === workflowStep.id);

        if (workflowStepIndex !== -1) {
          this.selectedWorkflow!.workflowSteps![workflowStepIndex] = {...result};
        }
      })
    ).subscribe();
  }

  deleteWorkflow(selectedWorkflow: Workflow) {
    this.dialog.open(ConfirmDeleteDialogComponent, {
      data: {
        message: `Are you sure you want to delete the <strong>${selectedWorkflow.name}</strong> workflow?`
      }
    }).afterClosed().pipe(
      take(1),
      filter(result => !!result),
      switchMap(() => this.workflowService.delete(selectedWorkflow)),
      tap(() => {
        this.loadWorkflows();
      })
    ).subscribe();
  }

  deleteStep(step: Step) {
    this.dialog.open(ConfirmDeleteDialogComponent, {
      data: {
        message: `Are you sure you want to delete <strong>${step.nameDefault}</strong>? This will delete it from the pool and from any/all workflows to which it's been assigned.`
      }
    }).afterClosed().pipe(
      take(1),
      filter(result => !!result),
      switchMap(() => this.stepService.delete(step)),
      tap(() => {
        this.loadSteps();
        this.loadWorkflows();
      })
    ).subscribe();
  }

  manageJobAssignments(workflow: Workflow) {
    this.router.navigate(['/internal', 'workflows', workflow.id, 'assignments']);
  }
}
