import {Component, OnInit} from '@angular/core';
import {OrgService} from "../../services/org.service";
import {take} from "rxjs";
import {Company} from "../../models/org/company";
import {JobTitle} from "../../models/org/job-title";

interface Alias {
  id: number;
  name: string;
}

@Component({
  selector: 'app-job-titles',
  templateUrl: './job-titles.component.html',
  styleUrls: ['./job-titles.component.scss']
})
export class JobTitlesComponent implements OnInit {
  companies: Company[] = [];

  jobTitles: JobTitle[] = [];

  aliases: { [key: number]: Alias[] } = {
    1: [{id: 1, name: 'Dev'}, {id: 2, name: 'Programmer'}],
    2: [{id: 3, name: 'PM'}, {id: 4, name: 'Product Owner'}]
  };

  selectedCompany: number | null = null;
  selectedJobTitle: number | null = null;
  filteredJobTitles: JobTitle[] = [];
  jobTitleAliases: Alias[] = [];
  newAlias: string = '';

  constructor(private orgService: OrgService) {
  }

  ngOnInit(): void {
    this.orgService.getCompanies().pipe(take(1)).subscribe((x) => this.companies = x);
  }

  onCompanyChange(): void {
    if (this.selectedCompany) {
      this.orgService.getJobTitles(this.selectedCompany).pipe(take(1)).subscribe((x) => this.filteredJobTitles = x);
    }
  }

  onJobTitleChange(): void {
    if (this.selectedJobTitle) {
      // Fetch the aliases for the selected job title
      this.jobTitleAliases = this.aliases[this.selectedJobTitle] || [];
    }
  }

  addAlias(): void {
    if (this.selectedJobTitle && this.newAlias.trim()) {
      const newAlias: Alias = {
        id: this.jobTitleAliases.length + 1, // Simple ID generator
        name: this.newAlias.trim()
      };
      this.jobTitleAliases.push(newAlias);
      this.newAlias = '';
      //this.snackBar.open('Alias added successfully!', '', { duration: 2000 });
    } else {
      //this.snackBar.open('Please select a job title and enter a valid alias.', '', { duration: 2000 });
    }
  }

  deleteAlias(alias: Alias): void {
    const index = this.jobTitleAliases.indexOf(alias);
    if (index > -1) {
      this.jobTitleAliases.splice(index, 1);
      //this.snackBar.open('Alias deleted successfully!', '', { duration: 2000 });
    }
  }
}
