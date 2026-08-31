using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Authors;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class AuthorsControllerTests : TestBase
    {
        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Authors/List/?page=1&pageSize=10";

            await DbContext.Authors.AddAsync(new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" });
            await DbContext.Authors.AddAsync(new Author { FirstName = "Jaan", LastName = "Kross" });
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<Author>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(2, response.Value.Results.Count);
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
        public async Task List_should_return_not_found_when_page_is_invalid()
        {
            // Arrange
            var url = "/api/Authors/List/?page=0&pageSize=0";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_should_return_author()
        {
            // Arrange
            var url = "/api/Authors/Get/?id=1";

            await DbContext.Authors.AddAsync(new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" });
            await DbContext.SaveChangesAsync();

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<AuthorDetailsDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
            Assert.NotNull(response.Value);
            Assert.Equal(1, response.Value.Id);
            Assert.Equal("Tammsaare", response.Value.LastName);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_author()
        {
            // Arrange
            var url = "/api/Authors/Get/?id=131";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_add_new_author()
        {
            // Arrange
            var command = new SaveAuthorCommand { Id = 0, FirstName = "Jaan", LastName = "Kross" };

            // Act
            var response = await Client.PostAsJsonAsync("/api/Authors/Save", command);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var saved = await DbContext.Authors.SingleOrDefaultAsync(author => author.Id == 1);
            Assert.NotNull(saved);
            Assert.Equal("Kross", saved.LastName);
        }

        [Fact]
        public async Task Save_should_update_existing_author()
        {
            // Arrange
            await DbContext.Authors.AddAsync(new Author { FirstName = "Old", LastName = "Name" });
            await DbContext.SaveChangesAsync();

            var command = new SaveAuthorCommand { Id = 1, FirstName = "Jaan", LastName = "Kross" };

            // Act
            var response = await Client.PostAsJsonAsync("/api/Authors/Save", command);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var saved = await DbContext.Authors.AsNoTracking().SingleOrDefaultAsync(author => author.Id == 1);
            Assert.NotNull(saved);
            Assert.Equal("Kross", saved.LastName);
        }

        [Fact]
        public async Task Save_should_return_bad_request_when_author_is_missing()
        {
            // Arrange
            var command = new SaveAuthorCommand { Id = 20, FirstName = "Jaan", LastName = "Kross" };

            // Act
            var response = await Client.PostAsJsonAsync("/api/Authors/Save", command);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_return_bad_request_when_id_is_negative()
        {
            // Arrange
            var command = new SaveAuthorCommand { Id = -10, FirstName = "Jaan", LastName = "Kross" };

            // Act
            var response = await Client.PostAsJsonAsync("/api/Authors/Save", command);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Save_should_return_bad_request_when_data_is_invalid()
        {
            // Arrange
            var command = new SaveAuthorCommand { Id = 0, FirstName = "", LastName = "Kross" };

            // Act
            var response = await Client.PostAsJsonAsync("/api/Authors/Save", command);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_delete_author()
        {
            // Arrange
            var author = new Author { FirstName = "Anton Hansen", LastName = "Tammsaare" };
            await DbContext.Authors.AddAsync(author);
            await DbContext.Books.AddAsync(new Book { Title = "Tõde ja õigus", Year = 1926, Author = author });
            await DbContext.SaveChangesAsync();

            var command = new DeleteAuthorCommand { Id = 1 };

            // Act
            var response = await DeleteAsJsonAsync("/api/Authors/Delete", command);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var authorCount = await DbContext.Authors.CountAsync();
            var bookCount = await DbContext.Books.CountAsync();
            Assert.Equal(0, authorCount);
            Assert.Equal(0, bookCount);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_author()
        {
            // Arrange
            var command = new DeleteAuthorCommand { Id = 1034 };

            // Act
            var response = await DeleteAsJsonAsync("/api/Authors/Delete", command);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
