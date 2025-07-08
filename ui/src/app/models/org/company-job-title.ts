import {Company} from "./company";
import {JobTitle} from "./job-title";

export interface CompanyJobTitle {
  company: Company;
  jobTitles: JobTitle[];
}
