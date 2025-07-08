import {Company} from "./company";
import {Workflow} from "../workflows/workflow";

export interface JobTitle {
  id: number;
  displayTitle: string;
  company: Company;
  workflowId?: number;
  workflow?: Workflow;
}
