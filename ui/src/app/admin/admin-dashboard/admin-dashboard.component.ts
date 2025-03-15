import {Component} from '@angular/core';
import {MatTableDataSource} from "@angular/material/table";

@Component({
  selector: 'app-admin-user-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent {
  statusOptions: string[] = ['Pending Invitation', 'Active', 'Pending Review'];
  companyOptions: string[] = ['ViaQuest Day & Employment Services LLC'];
  jobTitleOptions: string[] = [];

  searchCriteria = {
    name: '',
    status: '',
    company: '',
    jobTitle: ''
  };

  displayedColumns: string[] = [
    'name',
    'jobTitle',
    'status',
    'importedOn',
    'lastUpdatedCandidate',
    'lastUpdatedVQI',
    'percentComplete',
    'review',
    'downloadAll',
    'offboard',
    'invite'
  ];

  dataSource = new MatTableDataSource([
    {
      name: 'Susie Queue',
      phoneNumber: '5555555555',
      status: 'Pending Invitation',
      importedOn: '2024-03-15',
      lastUpdatedCandidate: null,
      lastUpdatedVQI: null,
      lastUpdatedBy: null,
      percentComplete: 0,
      totalSteps: 8,
      stepsCompleted: 0,
      jobTitle: 'Clinical Supervisor',
      company: 'ViaQuest Day & Employment Services LLC'
    },
    {
      name: 'John Doe',
      phoneNumber: '5555555555',
      status: 'Active',
      importedOn: '2024-03-10',
      lastUpdatedCandidate: '2024-03-12',
      lastUpdatedVQI: '2024-03-13',
      lastUpdatedBy: 'Lindsay Crowe',
      percentComplete: 25,
      totalSteps: 4,
      stepsCompleted: 1,
      jobTitle: 'Youth Support Specialist',
      company: 'ViaQuest Psychiatric & Behavioral Solutions LLC'
    },
    {
      name: 'Jane Smith',
      phoneNumber: '5555555555',
      status: 'Pending Review',
      importedOn: '2024-03-09',
      lastUpdatedCandidate: '2024-03-10',
      lastUpdatedVQI: '2024-03-11',
      lastUpdatedBy: 'Lindsay Crowe',
      percentComplete: 100,
      totalSteps: 6,
      stepsCompleted: 6,
      jobTitle: 'Behavior Consultant',
      company: 'ViaQuest Psychiatric & Behavioral Solutions LLC'
    },
    {
      name: 'Other Guy',
      phoneNumber: '5555555555',
      status: 'Active',
      importedOn: '2024-03-09',
      lastUpdatedCandidate: '2024-03-10',
      lastUpdatedVQI: '2024-03-11',
      lastUpdatedBy: 'Lindsay Crowe',
      percentComplete: 85,
      totalSteps: 20,
      stepsCompleted: 17,
      jobTitle: 'Medical Social Worker',
      company: 'ViaQuest Psychiatric & Behavioral Solutions LLC'
    }
    // Add more data as needed
  ]);

  onSearchChange() {

  }

  searchReset() {
    this.searchCriteria = {
      name: '',
      status: '',
      company: '',
      jobTitle: ''
    };

    this.onSearchChange();
  }
}
