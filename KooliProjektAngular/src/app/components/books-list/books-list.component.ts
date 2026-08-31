import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Book } from '../../models/book';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-books-list',
  templateUrl: './books-list.component.html',
  styleUrls: ['./books-list.component.css'],
  standalone: false
})
export class BooksListComponent implements OnInit {
  books: Book[] = [];

  constructor(private apiService: ApiService, private router: Router) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.apiService.list(1, 100).subscribe(result => {
      if (!result.hasErrors && result.value) {
        this.books = result.value.results;
      }
    });
  }

  addNew(): void {
    this.router.navigate(['/books/new']);
  }

  edit(book: Book): void {
    this.router.navigate(['/books', book.id]);
  }

  delete(book: Book): void {
    this.apiService.delete(book.id).subscribe(result => {
      if (!result.hasErrors) {
        this.loadData();
      }
    });
  }
}
