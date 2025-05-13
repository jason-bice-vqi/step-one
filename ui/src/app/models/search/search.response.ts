export interface SearchResponse<T> {
  pagedItems: T[];

  totalItems: number;

  totalPages: number;
}
