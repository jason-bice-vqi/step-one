import {Component} from '@angular/core';
import {MatTableDataSource} from "@angular/material/table";

@Component({
  selector: 'app-admin-user-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent {
  displayedColumns: string[] = [
    'name',
    'status',
    'importedOn',
    'lastUpdatedCandidate',
    'lastUpdatedVQI',
    'percentComplete',
    'review',
    'downloadAll',
    'offboard'
  ];

  dataSource = new MatTableDataSource([
    {
      name: 'John Doe',
      status: 'Active',
      importedOn: '2024-03-10',
      lastUpdatedCandidate: '2024-03-12',
      lastUpdatedVQI: '2024-03-13',
      percentComplete: 25,
      totalSteps: 4,
      stepsCompleted: 1
    },
    {
      name: 'Jane Smith',
      status: 'Pending Review',
      importedOn: '2024-03-09',
      lastUpdatedCandidate: '2024-03-10',
      lastUpdatedVQI: '2024-03-11',
      percentComplete: 100,
      totalSteps: 6,
      stepsCompleted: 6
    },
    {
      name: 'Other Guy',
      status: 'Active',
      importedOn: '2024-03-09',
      lastUpdatedCandidate: '2024-03-10',
      lastUpdatedVQI: '2024-03-11',
      percentComplete: 85,
      totalSteps: 20,
      stepsCompleted: 17
    }
    // Add more data as needed
  ]);
}
