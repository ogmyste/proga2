export interface OperationResult<T = any> {
  value?: T;
  propertyErrors?: { [key: string]: string };
  errors?: string[];
  hasErrors: boolean;
}

export interface PagedResult<T> {
  results: T[];
  currentPage: number;
  pageCount: number;
  pageSize: number;
  rowCount: number;
}
