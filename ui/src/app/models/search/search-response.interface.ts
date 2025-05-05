import {SearchRequestInterface} from "./search-request.interface";

export interface SearchResponseInterface<T> {
  pagedItems: T[];

  totalItems: number;

  totalPages: number;
}
