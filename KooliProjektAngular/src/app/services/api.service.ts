import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Book } from '../models/book';
import { OperationResult, PagedResult } from '../models/operation-result';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly baseUrl = 'http://localhost:5086/api/Books/';

  constructor(private http: HttpClient) { }

  list(page: number, pageSize: number): Observable<OperationResult<PagedResult<Book>>> {
    const url = this.baseUrl + 'List?page=' + page + '&pageSize=' + pageSize;
    return this.http.get(url, { responseType: 'text' })
      .pipe(map(body => JSON.parse(body) as OperationResult<PagedResult<Book>>));
  }

  get(id: number): Observable<OperationResult<Book>> {
    const url = this.baseUrl + 'Get?id=' + id;
    return this.http.get(url, { responseType: 'text' })
      .pipe(map(body => JSON.parse(body) as OperationResult<Book>));
  }

  save(book: Book): Observable<OperationResult> {
    return this.http.post(this.baseUrl + 'Save', book, { responseType: 'text' })
      .pipe(
        map(body => JSON.parse(body) as OperationResult),
        catchError(err => of(this.normalizeError(err)))
      );
  }

  delete(id: number): Observable<OperationResult> {
    return this.http.delete(this.baseUrl + 'Delete', { body: { id }, responseType: 'text' })
      .pipe(
        map(body => JSON.parse(body) as OperationResult),
        catchError(err => of(this.normalizeError(err)))
      );
  }

  private normalizeError(err: HttpErrorResponse): OperationResult {
    const result: OperationResult = { hasErrors: true };
    if (err.error) {
      try {
        const parsed = JSON.parse(err.error);
        result.propertyErrors = parsed.propertyErrors;
        result.errors = parsed.errors;
      } catch {
        // ignore parse errors
      }
    }
    return result;
  }
}
