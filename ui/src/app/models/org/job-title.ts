import {Company} from "./company";
import {Workflow} from "../workflows/workflow";
import {JobTitleAlias} from "./job-title-alias";

export interface JobTitle {
  id: number;
  displayTitle: string;
  displayTitleWithAbbr: string;
  company: Company;
  jobTitleAliases: JobTitleAlias[];
  workflowId?: number;
  workflow?: Workflow;
}
