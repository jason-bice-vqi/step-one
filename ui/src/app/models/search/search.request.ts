export interface SearchRequest {
  page: number;

  limit: number;

  sortBy?: string;

  desc: boolean;
}
