using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Books;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class BooksControllerTests : TestBase
    {
        private async Task<Author> SeedAuthor()
        {
            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            await DbContext.Authors.AddAsync(author);
            await DbContext.SaveChangesAsync();
            return author;
        }

        private async Task<HttpResponseMessage> DeleteAsJsonAsync(string url, object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(value)
            };
            return await Client.SendAsync(request);
        }

        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Books/List/?page=1&pageSize=10";

            var author = await SeedAuthor();
            await DbContext.Books.AddAsync(new Book { Title = "Tõde ja õigus", Year = 1926, AuthorId = author.Id });
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<Book>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Single(response.Value.Results);
        }

        [Fact]
        public async Task List_should_return_not_found_when_page_is_invalid()
        {
            // Arrange
            var url = "/api/Books/List/?page=0&pageSize=0";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_should_return_book()
        {
            // Arrange
            var url = "/api/Books/Get/?id=1";

            var author = await SeedAuthor();
            await DbContext.Books.AddAsync(new Book { Title = "Tõde ja õigus", Year = 1926, AuthorId = author.Id });
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<BookDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(1, response.Value.Id);
            Assert.Equal("Tõde ja õigus", response.Value.Title);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_book()
        {
            // Arrange
            var url = "/api/Books/Get/?id=131";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_add_new_book()
        {
            // Arrange
            var author = await SeedAuthor();
            var command = new SaveBookCommand { Id = 0, Title = "New book", Year = 2000, AuthorId = author.Id };

            // Act
            var response = await Client.PostAsJsonAsync("/api/Books/Save", command);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var saved = await DbContext.Books.SingleOrDefaultAsync(book => book.Id == 1);
            Assert.NotNull(saved);
            Assert.Equal("New book", saved.Title);
        }

        [Fact]
        public async Task Save_should_update_existing_book()
        {
            // Arrange
            var author = await SeedAuthor();
            await DbContext.Books.AddAsync(new Book { Title = "Old book", Year = 1999, AuthorId = author.Id });
            await DbContext.SaveChangesAsync();

            var command = new SaveBookCommand { Id = 1, Title = "Updated book", Year = 2001, AuthorId = author.Id };

            // Act
            var response = await Client.PostAsJsonAsync("/api/Books/Save", command);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var saved = await DbContext.Books.AsNoTracking().SingleOrDefaultAsync(book => book.Id == 1);
            Assert.NotNull(saved);
            Assert.Equal("Updated book", saved.Title);
            Assert.Equal(2001, saved.Year);
        }

        [Fact]
        public async Task Save_should_return_bad_request_when_book_is_missing()
        {
            // Arrange
            var author = await SeedAuthor();
            var command = new SaveBookCommand { Id = 20, Title = "Updated book", Year = 2000, AuthorId = author.Id };

            // Act
            var response = await Client.PostAsJsonAsync("/api/Books/Save", command);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_return_bad_request_when_id_is_negative()
        {
            // Arrange
            var author = await SeedAuthor();
            var command = new SaveBookCommand { Id = -10, Title = "New book", Year = 2000, AuthorId = author.Id };

            // Act
            var response = await Client.PostAsJsonAsync("/api/Books/Save", command);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_return_bad_request_when_data_is_invalid()
        {
            // Arrange
            var command = new SaveBookCommand { Id = 0, Title = "", Year = 2000, AuthorId = 1 };

            // Act
            var response = await Client.PostAsJsonAsync("/api/Books/Save", command);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_delete_book()
        {
            // Arrange
            var author = await SeedAuthor();
            await DbContext.Books.AddAsync(new Book { Title = "Tõde ja õigus", Year = 1926, AuthorId = author.Id });
            await DbContext.SaveChangesAsync();

            var command = new DeleteBookCommand { Id = 1 };

            // Act
            var response = await DeleteAsJsonAsync("/api/Books/Delete", command);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var count = await DbContext.Books.CountAsync();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_book()
        {
            // Arrange
            var command = new DeleteBookCommand { Id = 1034 };

            // Act
            var response = await DeleteAsJsonAsync("/api/Books/Delete", command);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
