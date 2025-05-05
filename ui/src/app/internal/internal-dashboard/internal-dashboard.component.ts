import {Component, OnInit} from '@angular/core';
import {CandidateService} from "../../services/candidates/candidate.service";
import {take} from "rxjs";
import {CandidateInterface} from "../../models/candidates/candidate.interface";
import {SearchResponseInterface} from "../../models/search/search-response.interface";
import {PageEvent} from "@angular/material/paginator";
import {SearchRequestInterface} from "../../models/search/search-request.interface";

@Component({
  selector: 'app-internal-user-dashboard',
  templateUrl: './internal-dashboard.component.html',
  styleUrls: ['./internal-dashboard.component.scss']
})
export class InternalDashboardComponent implements OnInit {
  searchRequest: SearchRequestInterface<CandidateInterface> = {page: 0, limit: 10};
  searchResponse?: SearchResponseInterface<CandidateInterface>;

  constructor(private candidateService: CandidateService) {
  }

  ngOnInit(): void {
    this.onSearchChange();
  }

  statusOptions: string[] = ['Invited - Active', 'Invited - Inactive', 'Pending Invite'];
  companyOptions: string[] = ['ViaQuest Day & Employment Services LLC'];
  jobTitleOptions: string[] = [];
  workflowStatusOptions: string[] = ['Assigned', 'Completed by Candidate', 'Completed and Confirmed', 'Not Started', 'In Progress', 'Unassigned'];

  searchCriteria = {
    name: '',
    status: '',
    company: '',
    jobTitle: '',
    workflowStatus: ''
  };

  displayedColumns: string[] = [
    'name',
    'jobTitle',
    'status',
    'importedAt',
    'lastUpdatedCandidate',
    'lastUpdatedVQI',
    'percentComplete',
    'review',
    'downloadAll',
    'offboard',
    'invite'
  ];

  onSearchChange() {
    this.candidateService.search(this.searchRequest).pipe(take(1)).subscribe((x) => this.searchResponse = x);
  }

  searchReset() {
    this.searchCriteria = {
      name: '',
      status: '',
      company: '',
      jobTitle: '',
      workflowStatus: ''
    };

    this.onSearchChange();
  }

  onPageChange($event: PageEvent) {
    this.searchRequest.limit = $event.pageSize;
    this.searchRequest.page = $event.pageIndex + 1;

    this.onSearchChange();
  }
}
