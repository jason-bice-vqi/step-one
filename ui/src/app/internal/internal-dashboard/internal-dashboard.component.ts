import {Component, OnInit, ViewChild} from '@angular/core';
import {CandidateService} from "../../services/candidates/candidate.service";
import {debounceTime, take} from "rxjs";
import {Candidate} from "../../models/candidates/candidate";
import {SearchResponse} from "../../models/search/search.response";
import {MatPaginator, PageEvent} from "@angular/material/paginator";
import {CandidateSearchRequest} from "../../models/candidates/candidate-search.request";
import {Sort} from '@angular/material/sort';
import {AddJobTitleAliasComponent} from "../add-job-title-alias/add-job-title-alias.component";
import {MatDialog} from "@angular/material/dialog";
import {JobTitle} from "../../models/org/job-title";
import {Company} from "../../models/org/company";
import {OrgService} from "../../services/org.service";
import {CandidateWorkflowStatuses} from "../../models/candidates/candidate-workflow.statuses";
import {Workflow} from "../../models/workflows/workflow";
import {WorkflowService} from "../../services/workflows/workflow.service";

@Component({
  selector: 'app-internal-user-dashboard',
  templateUrl: './internal-dashboard.component.html',
  styleUrls: ['./internal-dashboard.component.scss']
})
export class InternalDashboardComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  private searchDebounceMilliseconds = 500;
  private defaultPageSize = 10;

  companies: Company[] = [];
  jobTitles: JobTitle[] = [];
  workflows: Workflow[] = [];

  searchRequest: CandidateSearchRequest = {desc: false, limit: this.defaultPageSize, page: 0};

  searchResponse?: SearchResponse<Candidate>;

  constructor(private candidateService: CandidateService,
              private dialog: MatDialog,
              private orgService: OrgService,
              private workflowService: WorkflowService) {
  }

  ngOnInit(): void {
    this.search();

    this.orgService.getCompanies().pipe(take(1)).subscribe(x => this.companies = x);
    this.orgService.getJobTitles().pipe(take(1)).subscribe(x => this.jobTitles = x);
    this.workflowService.get().pipe((take(1))).subscribe(x => this.workflows = x);

    this.candidateWorkflowStatusOptions = Object.keys(CandidateWorkflowStatuses)
      .filter(key => !isNaN(Number(key))) // only numeric keys
      .map(key => ({
        label: CandidateWorkflowStatuses[Number(key)],
        value: Number(key)
      }));
  }

  statusOptions: string[] = ['Invited - Active', 'Invited - Inactive', 'Pending'];
  companyOptions: string[] = ['ViaQuest Day & Employment Services LLC'];
  jobTitleOptions: string[] = [];
  candidateWorkflowStatusOptions: { label: string, value: number }[] = [];

  displayedColumns: string[] = [
    'name',
    'jobTitle',
    'status',
    'importedAt',
    'lastUpdatedCandidate',
    'lastUpdatedVQI',
    'percentComplete',
    'actions'
  ];

  search(resetPaging = false) {
    if (resetPaging) {
      this.paginator.pageIndex = 0;
      this.searchRequest.page = 0;
    }

    this.candidateService.search(this.searchRequest)
      .pipe(debounceTime(this.searchDebounceMilliseconds), take(1))
      .subscribe((x) => this.searchResponse = x);
  }

  searchReset() {
    this.searchRequest = {
      desc: false,
      limit: this.defaultPageSize,
      page: 0
    };

    this.search();
  }

  onPageChange($event: PageEvent) {
    this.searchRequest.limit = $event.pageSize;
    this.searchRequest.page = $event.pageIndex + 1;

    this.search();
  }

  onSortChange(sort: Sort): void {
    this.searchRequest.sortBy = sort.active;
    this.searchRequest.desc = sort.direction.toUpperCase() == 'DESC';

    this.search();
  }

  onboardCandidate(candidate: Candidate) {
    this.dialog.open(AddJobTitleAliasComponent, {
      minWidth: '750px',
      data: {
        candidate: candidate,
        companies: this.companies,
        jobTitles: this.jobTitles,
        workflows: this.workflows
      }
    }).afterClosed().subscribe((result: Candidate | null) => {
      if (result) {
        this.search();
      }
    });
  }
}
