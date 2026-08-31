using System.Linq;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Authors;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class AuthorTests : TestBase
    {
        // GET

        [Fact]
        public void Get_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetAuthorQueryHandler(null);
            });
        }

        [Fact]
        public async Task Get_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (GetAuthorQuery)null;
            var handler = new GetAuthorQueryHandler(DbContext);

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
            var query = new GetAuthorQuery { Id = id };
            var handler = new GetAuthorQueryHandler(GetFaultyDbContext());

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
        public async Task Get_should_return_existing_author()
        {
            // Arrange
            var query = new GetAuthorQuery { Id = 1 };
            var handler = new GetAuthorQueryHandler(DbContext);

            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            var book = new Book { Title = "Tõde ja õigus", Year = 1926, Author = author };
            await DbContext.Authors.AddAsync(author);
            await DbContext.Books.AddAsync(book);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(query.Id, result.Value.Id);
            Assert.Equal("Anton Hansen", result.Value.FirstName);
            Assert.Single(result.Value.Books);
        }

        [Theory]
        [InlineData(101)]
        public async Task Get_should_return_null_when_author_does_not_exist(int id)
        {
            // Arrange
            var query = new GetAuthorQuery { Id = id };
            var handler = new GetAuthorQueryHandler(DbContext);

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

        // LIST

        [Fact]
        public void List_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListAuthorsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (ListAuthorsQuery)null;
            var handler = new ListAuthorsQueryHandler(DbContext);

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
            var query = new ListAuthorsQuery { Page = page, PageSize = pageSize };
            var handler = new ListAuthorsQueryHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task List_should_return_page_of_authors()
        {
            // Arrange
            var query = new ListAuthorsQuery { Page = 1, PageSize = 5 };
            var handler = new ListAuthorsQueryHandler(DbContext);

            foreach (var i in Enumerable.Range(1, 15))
            {
                var author = new Author { FirstName = $"First {i}", LastName = $"Last {i}" };
                await DbContext.Authors.AddAsync(author);
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
        public async Task List_should_return_empty_result_if_authors_doesnt_exist()
        {
            // Arrange
            var query = new ListAuthorsQuery { Page = 1, PageSize = 5 };
            var handler = new ListAuthorsQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Results);
        }

        [Fact]
        public async Task List_should_filter_authors_by_first_name()
        {
            // Arrange
            var query = new ListAuthorsQuery { Page = 1, PageSize = 10, FirstName = "Anton" };
            var handler = new ListAuthorsQueryHandler(DbContext);

            await DbContext.Authors.AddAsync(new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" });
            await DbContext.Authors.AddAsync(new Author { FirstName = "Jaan", LastName = "Kross" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("Tammsaare", result.Value.Results[0].LastName);
        }

        [Fact]
        public async Task List_should_filter_authors_by_last_name()
        {
            // Arrange
            var query = new ListAuthorsQuery { Page = 1, PageSize = 10, LastName = "Kross" };
            var handler = new ListAuthorsQueryHandler(DbContext);

            await DbContext.Authors.AddAsync(new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" });
            await DbContext.Authors.AddAsync(new Author { FirstName = "Jaan", LastName = "Kross" });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("Jaan", result.Value.Results[0].FirstName);
        }

        // DELETE

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteAuthorCommandHandler(null);
            });
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteAuthorCommand)null;
            var handler = new DeleteAuthorCommandHandler(DbContext);

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
            var query = new DeleteAuthorCommand { Id = id };
            var handler = new DeleteAuthorCommandHandler(GetFaultyDbContext());

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_author()
        {
            // Arrange
            var query = new DeleteAuthorCommand { Id = 1 };
            var handler = new DeleteAuthorCommandHandler(DbContext);

            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            await DbContext.Books.AddAsync(new Book { Title = "Tõde ja õigus", Year = 1926, Author = author });
            await DbContext.Books.AddAsync(new Book { Title = "Kõrboja peremees", Year = 1922, Author = author });
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            var authorCount = DbContext.Authors.Count();
            var bookCount = DbContext.Books.Count();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(0, authorCount);
            Assert.Equal(0, bookCount);
        }

        [Fact]
        public async Task Delete_should_work_with_not_existing_author()
        {
            // Arrange
            var query = new DeleteAuthorCommand { Id = 1034 };
            var handler = new DeleteAuthorCommandHandler(DbContext);

            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            await DbContext.Authors.AddAsync(author);
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
                new SaveAuthorCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveAuthorCommand)null;
            var handler = new SaveAuthorCommandHandler(DbContext);

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
            var request = new SaveAuthorCommand { Id = -10 };
            var handler = new SaveAuthorCommandHandler(GetFaultyDbContext());

            // Act 
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_add_new_author()
        {
            // Arrange
            var request = new SaveAuthorCommand { Id = 0, FirstName = "Jaan", LastName = "Kross" };
            var handler = new SaveAuthorCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedAuthor = await DbContext.Authors.SingleOrDefaultAsync(author => author.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedAuthor);
            Assert.Equal(1, savedAuthor.Id);
            Assert.Equal(request.FirstName, savedAuthor.FirstName);
        }

        [Fact]
        public async Task Save_should_update_existing_author()
        {
            // Arrange
            var request = new SaveAuthorCommand { Id = 1, FirstName = "Jaan", LastName = "Kross" };
            var handler = new SaveAuthorCommandHandler(DbContext);

            var author = new Author { Id = 0, FirstName = "Old", LastName = "Name" };
            await DbContext.Authors.AddAsync(author);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedAuthor = await DbContext.Authors.SingleOrDefaultAsync(a => a.Id == request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedAuthor);
            Assert.Equal(request.LastName, savedAuthor.LastName);
        }

        [Fact]
        public async Task Save_should_not_update_missing_author()
        {
            // Arrange
            var request = new SaveAuthorCommand { Id = 20, FirstName = "Jaan", LastName = "Kross" };
            var handler = new SaveAuthorCommandHandler(DbContext);

            var author = new Author { Id = 0, FirstName = "Old", LastName = "Name" };
            await DbContext.Authors.AddAsync(author);
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
        [InlineData("012345678901234567890123456789012345678901234567890")]
        public void SaveValidator_should_return_false_when_first_name_is_invalid(string firstName)
        {
            // Arrange
            var command = new SaveAuthorCommand { Id = 0, FirstName = firstName, LastName = "Kross" };
            var validator = new SaveAuthorCommandValidator();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveAuthorCommand.FirstName), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("012345678901234567890123456789012345678901234567890")]
        public void SaveValidator_should_return_false_when_last_name_is_invalid(string lastName)
        {
            // Arrange
            var command = new SaveAuthorCommand { Id = 0, FirstName = "Jaan", LastName = lastName };
            var validator = new SaveAuthorCommandValidator();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveAuthorCommand.LastName), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_true_when_names_are_valid()
        {
            // Arrange
            var command = new SaveAuthorCommand { Id = 0, FirstName = "Jaan", LastName = "Kross" };
            var validator = new SaveAuthorCommandValidator();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
