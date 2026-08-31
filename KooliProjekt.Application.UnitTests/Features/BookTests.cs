using System.Linq;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Books;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class BookTests : TestBase
    {
        // GET

        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetBookQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetBookQuery)null;
            var handler = new GetBookQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_return_null_when_request_id_is_zero_or_negative(int id)
        {
            // Arrange
            var query = new GetBookQuery { Id = id };
            var handler = new GetBookQueryHandler(GetFaultyDbContext());

            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            await DbContext.Authors.AddAsync(author);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_should_return_existing_book()
        {
            // Arrange
            var query = new GetBookQuery { Id = 1 };
            var handler = new GetBookQueryHandler(DbContext);

            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            var book = new Book { Title = "Tõde ja õigus", Year = 1926, Author = author };
            await DbContext.Books.AddAsync(book);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, result.Value.Id);
            Assert.Equal("Tõde ja õigus", result.Value.Title);
            Assert.Equal("Anton Hansen", result.Value.AuthorFirstName);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_book_does_not_exist(int id)
        {
            // Arrange
            var query = new GetBookQuery { Id = id };
            var handler = new GetBookQueryHandler(DbContext);

            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            var book = new Book { Title = "Tõde ja õigus", Year = 1926, Author = author };
            await DbContext.Books.AddAsync(book);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        // LIST

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListBooksQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListBooksQuery)null;
            var handler = new ListBooksQueryHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 5)]
        [InlineData(4, -10)]
        [InlineData(5, -5)]
        [InlineData(0, 0)]
        [InlineData(-5, -10)]
        public async Task List_should_return_null_when_page_or_page_size_is_zero_or_negative(int page, int pageSize)
        {
            // Arrange
            var query = new ListBooksQuery { Page = page, PageSize = pageSize };
            var handler = new ListBooksQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_books()
        {
            // Arrange
            var query = new ListBooksQuery { Page = 1, PageSize = 5 };
            var handler = new ListBooksQueryHandler(DbContext);

            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            foreach (var i in Enumerable.Range(1, 15))
            {
                var book = new Book { Title = $"Book {i}", Year = 2000, Author = author };
                await DbContext.Books.AddAsync(book);
            }

            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Page, result.Value.CurrentPage);
            Assert.Equal(query.PageSize, result.Value.Results.Count);
        }

        [Fact]
        public async Task List_should_return_empty_result_if_books_doesnt_exist()
        {
            // Arrange
            var query = new ListBooksQuery { Page = 1, PageSize = 5 };
            var handler = new ListBooksQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_filter_books_by_title()
        {
            // Arrange
            var query = new ListBooksQuery { Page = 1, PageSize = 10, Title = "õigus" };
            var handler = new ListBooksQueryHandler(DbContext);

            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            await DbContext.Books.AddAsync(new Book { Title = "Tõde ja õigus", Year = 1926, Author = author });
            await DbContext.Books.AddAsync(new Book { Title = "Kevade", Year = 1912, Author = author });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("Tõde ja õigus", result.Value.Results[0].Title);
        }

        [Fact]
        public async Task List_should_filter_books_by_author()
        {
            // Arrange
            var query = new ListBooksQuery { Page = 1, PageSize = 10, AuthorId = 2 };
            var handler = new ListBooksQueryHandler(DbContext);

            var tammsaare = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            var kross = new Author { FirstName = "Jaan", LastName = "Kross" };
            await DbContext.Books.AddAsync(new Book { Title = "Tõde ja õigus", Year = 1926, Author = tammsaare });
            await DbContext.Books.AddAsync(new Book { Title = "Keisri hull", Year = 1978, Author = kross });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("Keisri hull", result.Value.Results[0].Title);
        }

        // DELETE

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteBookCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteBookCommand)null;
            var handler = new DeleteBookCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Delete_should_not_use_dbcontext_if_id_is_zero_or_less(int id)
        {
            // Arrange
            var query = new DeleteBookCommand { Id = id };
            var handler = new DeleteBookCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_book()
        {
            // Arrange
            var query = new DeleteBookCommand { Id = 1 };
            var handler = new DeleteBookCommandHandler(DbContext);

            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            var book = new Book { Title = "Tõde ja õigus", Year = 1926, Author = author };
            await DbContext.Books.AddAsync(book);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var count = DbContext.Books.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_book()
        {
            // Arrange
            var query = new DeleteBookCommand { Id = 1034 };
            var handler = new DeleteBookCommandHandler(DbContext);

            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            var book = new Book { Title = "Tõde ja õigus", Year = 1926, Author = author };
            await DbContext.Books.AddAsync(book);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        // SAVE

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveBookCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveBookCommand)null;
            var handler = new SaveBookCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_return_if_id_is_negative()
        {
            // Arrange
            var request = new SaveBookCommand { Id = -10 };
            var handler = new SaveBookCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_book()
        {
            // Arrange
            var request = new SaveBookCommand { Id = 0, Title = "New book", Year = 2000, AuthorId = 1 };
            var handler = new SaveBookCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedBook = await DbContext.Books.SingleOrDefaultAsync(book => book.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedBook);
            Assert.Equal(1, savedBook.Id);
            Assert.Equal(request.Title, savedBook.Title);
        }

        [Fact]
        public async Task Save_should_update_existing_book()
        {
            // Arrange
            var request = new SaveBookCommand { Id = 1, Title = "Updated book", Year = 2000, AuthorId = 1 };
            var handler = new SaveBookCommandHandler(DbContext);

            var book = new Book { Id = 0, Title = "New book", Year = 1999, AuthorId = 1 };
            await DbContext.Books.AddAsync(book);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedBook = await DbContext.Books.SingleOrDefaultAsync(b => b.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedBook);
            Assert.Equal(request.Title, savedBook.Title);
        }

        [Fact]
        public async Task Save_should_not_update_missing_book()
        {
            // Arrange
            var request = new SaveBookCommand { Id = 20, Title = "Updated book", Year = 2000, AuthorId = 1 };
            var handler = new SaveBookCommandHandler(DbContext);

            var book = new Book { Id = 0, Title = "New book", Year = 1999, AuthorId = 1 };
            await DbContext.Books.AddAsync(book);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        // VALIDATOR

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")]
        public void SaveValidator_should_return_false_when_title_is_invalid(string title)
        {
            // Arrange
            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            DbContext.Authors.Add(author);
            DbContext.SaveChanges();

            var command = new SaveBookCommand { Id = 0, Title = title, Year = 2000, AuthorId = 1 };
            var validator = new SaveBookCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveBookCommand.Title), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_title_is_valid()
        {
            // Arrange
            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            DbContext.Authors.Add(author);
            DbContext.SaveChanges();

            var command = new SaveBookCommand { Id = 0, Title = "New book", Year = 2000, AuthorId = 1 };
            var validator = new SaveBookCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(2101)]
        public void SaveValidator_should_return_false_when_year_is_invalid(int year)
        {
            // Arrange
            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            DbContext.Authors.Add(author);
            DbContext.SaveChanges();

            var command = new SaveBookCommand { Id = 0, Title = "New book", Year = year, AuthorId = 1 };
            var validator = new SaveBookCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveBookCommand.Year), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_false_when_author_does_not_exist()
        {
            // Arrange
            var command = new SaveBookCommand { Id = 0, Title = "New book", Year = 2000, AuthorId = 999 };
            var validator = new SaveBookCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveBookCommand.AuthorId), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_author_exists()
        {
            // Arrange
            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            DbContext.Authors.Add(author);
            DbContext.SaveChanges();

            var command = new SaveBookCommand { Id = 0, Title = "New book", Year = 2000, AuthorId = 1 };
            var validator = new SaveBookCommandValidator(DbContext);

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
