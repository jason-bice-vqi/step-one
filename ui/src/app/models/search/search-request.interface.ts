export interface SearchRequestInterface<T> {
  searchModel?: T;

  page: number;

  limit: number;
}
