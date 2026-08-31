import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Book } from '../../models/book';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-book-form',
  templateUrl: './book-form.component.html',
  styleUrls: ['./book-form.component.css'],
  standalone: false
})
export class BookFormComponent implements OnInit {
  book: Book = { id: 0, title: '', year: 2000, authorId: 1 };
  isEdit = false;
  errors: string[] = [];
  propertyErrors: { [key: string]: string } = {};

  constructor(
    private apiService: ApiService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEdit = true;
      this.apiService.get(+id).subscribe(result => {
        if (!result.hasErrors && result.value) {
          this.book = result.value;
        }
      });
    }
  }

  save(): void {
    this.errors = [];
    this.propertyErrors = {};

    this.apiService.save(this.book).subscribe(result => {
      if (!result.hasErrors) {
        this.router.navigate(['/books']);
        return;
      }

      this.errors = result.errors ?? [];
      this.propertyErrors = result.propertyErrors ?? {};
    });
  }
}
