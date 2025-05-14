import {Component, OnInit} from '@angular/core';
import {
  WorkflowStepConfig,
  WorkflowStepConfigDialogComponent,
} from "../workflow-step-config-dialog/workflow-step-config-dialog.component";
import {MatDialog} from "@angular/material/dialog";
import {ConfirmDeleteDialogComponent} from "../../confirm-delete-dialog/confirm-delete-dialog.component";
import {filter, switchMap, take, tap} from "rxjs";
import {Workflow} from "../../models/workflows/workflow";
import {WorkflowStep} from "../../models/workflows/workflow.step";
import {WorkflowService} from "../../services/workflows/workflow.service";
import {StepService} from "../../services/workflows/step.service";
import {Step} from "../../models/workflows/step";
import {StepDialogComponent} from "../step-dialog/step-dialog.component";
import {WorkflowDialogComponent} from "../workflow-dialog/workflow-dialog.component";

@Component({
  selector: 'app-workflows',
  templateUrl: './workflows.component.html',
  styleUrls: ['./workflows.component.scss']
})
export class WorkflowsComponent implements OnInit {
  /** All workflows. **/
  workflows: Workflow[] = [];

  /** All potential steps. **/
  steps: Step[] = [];

  /** Filtered/sorted steps available for the currently selected workflow. **/
  stepPool: Step[] = [];

  /** The currently selected workflow. **/
  selectedWorkflow: Workflow | null = null;

  constructor(private dialog: MatDialog, private stepService: StepService, private workflowService: WorkflowService) {
  }

  ngOnInit(): void {
    this.loadSteps();
    this.loadWorkflows();
  }

  loadSteps() {
    this.stepService.get().pipe(take(1)).subscribe((x) => {
      this.steps = x;
      this.refreshLocalStepPool();
    });
  }

  loadWorkflows(): void {
    this.workflowService.get().pipe(take(1)).subscribe(x => {
      this.workflows = x;
      this.selectedWorkflow = null;
    });
  }

  refreshLocalStepPool() {
    if (!this.selectedWorkflow) return;

    const workflowSteps: WorkflowStep[] = this.selectedWorkflow.workflowSteps!;
    const steps: Step[] = workflowSteps.map(x => x.step);
    const stepIds: number[] = steps.map(x => x.id);

    this.stepPool = this.steps
      .filter(x => stepIds.indexOf(x.id) === -1)
      .sort((a, b) => a.nameDefault.localeCompare(b.nameDefault));
  }

  selectWorkflow(workflow: Workflow) {
    this.selectedWorkflow = workflow;
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
      data: { mode, workflow }
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
      tap(() => this.loadWorkflows())
    ).subscribe();
  }

  saveSelectedWorkflow() {
    this.workflowService.update(this.selectedWorkflow!).pipe(take(1), tap(x => this.selectedWorkflow = x)).subscribe();
  }

  createNewStep() {
    this.dialog.open(StepDialogComponent, {width: '600px'}).afterClosed().pipe(
      take(1),
      filter(result => !!result),
      switchMap(result => this.stepService.create(result)),
      tap((x) => {
        this.steps.push(x);
        this.refreshLocalStepPool();
      })
    ).subscribe();
  }

  addStepToWorkflow(step: Step) {
    if (!this.selectedWorkflow) return;

    const workflowStep: WorkflowStep = {
      blockDownstream: false,
      id: 0,
      isAdminConfirmationRequired: false,
      requiresAdminConfirmation: false,
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

    this.selectedWorkflow!.workflowSteps = this.selectedWorkflow!.workflowSteps!.filter(s => s.id !== workflowStep.id);
    this.refreshLocalStepPool();
  }

  moveStepUp(index: number) {
    if (index === 0 || !this.selectedWorkflow) return;

    const steps = this.selectedWorkflow.workflowSteps!;
    [steps[index - 1], steps[index]] = [steps[index], steps[index - 1]];
  }

  moveStepDown(index: number) {
    if (!this.selectedWorkflow) return;

    const steps = this.selectedWorkflow.workflowSteps!;
    if (index >= steps.length - 1) return;

    [steps[index], steps[index + 1]] = [steps[index + 1], steps[index]];
  }

  configureStep(step: WorkflowStep) {
    this.dialog.open(WorkflowStepConfigDialogComponent, {
      width: '400px',
      data: {
        name: step.stepName,
        blockDownstream: step.blockDownstream || false,
        requiresAdminApproval: step.requiresAdminConfirmation || false
      }
    }).afterClosed().pipe(
      take(1),
      filter((result): result is WorkflowStepConfig => !!result),
      tap(result => {
        step.stepName = result.name;
        step.blockDownstream = result.blockDownstream;
        step.requiresAdminConfirmation = result.requiresAdminApproval;
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
}
