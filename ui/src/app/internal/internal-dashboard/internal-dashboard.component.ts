import {Component, OnInit} from '@angular/core';
import {CandidateService} from "../../services/candidates/candidate.service";
import {debounceTime, take} from "rxjs";
import {CandidateInterface} from "../../models/candidates/candidate.interface";
import {SearchResponseInterface} from "../../models/search/search-response.interface";
import {PageEvent} from "@angular/material/paginator";
import {CandidateSearchRequestInterface} from "../../models/candidates/candidate-search-request.interface";
import { Sort } from '@angular/material/sort';

@Component({
  selector: 'app-internal-user-dashboard',
  templateUrl: './internal-dashboard.component.html',
  styleUrls: ['./internal-dashboard.component.scss']
})
export class InternalDashboardComponent implements OnInit {
  private searchDebounceMilliseconds = 500;
  private defaultPageSize = 10;

  searchRequest: CandidateSearchRequestInterface = {desc: false, limit: this.defaultPageSize, page: 0};

  searchResponse?: SearchResponseInterface<CandidateInterface>;

  constructor(private candidateService: CandidateService) {
  }

  ngOnInit(): void {
    this.search();
  }

  statusOptions: string[] = ['Invited - Active', 'Invited - Inactive', 'Pending'];
  companyOptions: string[] = ['ViaQuest Day & Employment Services LLC'];
  jobTitleOptions: string[] = [];
  workflowStatusOptions: string[] = ['Assigned', 'Completed by Candidate', 'Completed and Confirmed', 'Not Started', 'In Progress', 'Unassigned'];

  displayedColumns: string[] = [
    'name',
    'jobTitle',
    'status',
    'importedAt',
    'lastUpdatedCandidate',
    'lastUpdatedVQI',
    'percentComplete',
    // 'review',
    // 'downloadAll',
    // 'offboard',
    // 'invite',
    'actions'
  ];

  search() {
    this.candidateService.search(this.searchRequest)
      .pipe(debounceTime(this.searchDebounceMilliseconds), take(1))
      .subscribe((x) => this.searchResponse = x);
  }

  searchReset() {
    this.searchRequest = {
      desc: false,
      limit: this.defaultPageSize,
      page: 1
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
}
