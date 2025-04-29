import {SearchRequestInterface} from "./search-request.interface";

export interface SearchResponseInterface<T> {
  pagedItems: T[];

  searchRequest: SearchRequestInterface<T>;

  totalItems: number;

  totalPages: number;
}
