import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {Company} from "../../models/org/company";
import {JobTitle} from "../../models/org/job-title";
import {JobTitleService} from "../../services/job-title.service";
import {OrgService} from "../../services/org.service";
import {take} from "rxjs";


@Component({
  selector: 'app-add-job-title-alias',
  templateUrl: './add-job-title-alias.component.html',
  styleUrls: ['./add-job-title-alias.component.scss']
})
export class AddJobTitleAliasComponent implements OnInit {
  atsJobTitle: string
  companies: Company[] = [];
  jobTitles: JobTitle[] = [];
  selectedCompanyId: number | null = null;
  selectedJobTitleId: number | null = null;

  get filteredJobTitles(): JobTitle[] {
    if (!this.selectedCompanyId) return [];

    return this.jobTitles
      .filter(x => x.company.id === this.selectedCompanyId)
      .sort((a, b) => a.displayTitle.localeCompare(b.displayTitle));
  }

  constructor(
    public dialogRef: MatDialogRef<AddJobTitleAliasComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { atsJobTitle: string, companies: Company[], jobTitles: JobTitle[] },
    private orgService: OrgService
  ) {
    this.atsJobTitle = data.atsJobTitle;
    this.companies = data.companies;
    this.jobTitles = data.jobTitles;
  }

  ngOnInit(): void {
  }

  createAlias(): void {
    this.orgService.createJobTitleAlias(this.selectedJobTitleId!, this.atsJobTitle).pipe((take(1))).subscribe(x => {
      this.dialogRef.close(x);
    });
  }

  cancel(): void {
    this.dialogRef.close(null);
  }
}
