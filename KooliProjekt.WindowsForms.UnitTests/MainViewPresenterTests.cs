using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.WindowsForms.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WindowsForms.UnitTests
{
    public class MainViewPresenterTests
    {
        private readonly Mock<IApiClient> _apiClient;
        private readonly Mock<IMainView> _mainView;
        private readonly MainViewPresenter _presenter;

        public MainViewPresenterTests()
        {
            _apiClient = new Mock<IApiClient>();
            _mainView = new Mock<IMainView>();
            _presenter = new MainViewPresenter(_apiClient.Object, _mainView.Object);
        }

        [Fact]
        public async Task LoadData_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var response = new OperationResult<PagedResult<Book>>();
            response.AddError("Test error");
            _apiClient
                .Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(response);

            // Act
            await _presenter.LoadData();

            // Assert
            _mainView.Verify(x => x.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()), Times.Once);
            _mainView.VerifySet(x => x.DataSource = null, Times.Once);
        }

        [Fact]
        public async Task LoadData_should_set_DataSource_with_valid_response()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Tõde ja õigus", Year = 1926, AuthorId = 1 }
            };
            var response = new OperationResult<PagedResult<Book>>(new PagedResult<Book> { Results = books });
            _apiClient
                .Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(response);

            // Act
            await _presenter.LoadData();

            // Assert
            _mainView.VerifySet(x => x.DataSource = books, Times.Once);
            _mainView.Verify(x => x.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()), Times.Never);
        }

        [Fact]
        public void SetSelection_should_clear_fields_with_null_selection()
        {
            // Act
            _presenter.SetSelection(null);

            // Assert
            _mainView.VerifySet(x => x.CurrentId = 0);
            _mainView.VerifySet(x => x.CurrentTitle = "");
            _mainView.VerifySet(x => x.CurrentYear = 0);
            _mainView.VerifySet(x => x.CurrentAuthorId = 0);
        }

        [Fact]
        public void SetSelection_should_set_fields_with_valid_selection()
        {
            // Arrange
            var book = new Book { Id = 5, Title = "Kevade", Year = 1912, AuthorId = 4 };

            // Act
            _presenter.SetSelection(book);

            // Assert
            _mainView.VerifySet(x => x.CurrentId = 5);
            _mainView.VerifySet(x => x.CurrentTitle = "Kevade");
            _mainView.VerifySet(x => x.CurrentYear = 1912);
            _mainView.VerifySet(x => x.CurrentAuthorId = 4);
        }

        [Fact]
        public async Task Save_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var response = new OperationResult();
            response.AddError("Test error");
            _apiClient
                .Setup(x => x.Save(It.IsAny<Book>()))
                .ReturnsAsync(response);

            // Act
            await _presenter.Save();

            // Assert
            _mainView.Verify(x => x.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()), Times.Once);
            _apiClient.Verify(x => x.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Save_should_call_LoadData_with_valid_response()
        {
            // Arrange
            var response = new OperationResult();
            _apiClient
                .Setup(x => x.Save(It.IsAny<Book>()))
                .ReturnsAsync(response);
            _apiClient
                .Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new OperationResult<PagedResult<Book>>(new PagedResult<Book>()));

            // Act
            await _presenter.Save();

            // Assert
            _apiClient.Verify(x => x.List(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _mainView.Verify(x => x.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_return_when_user_didnot_confirmed()
        {
            // Arrange
            _mainView
                .Setup(x => x.ConfirmDelete())
                .Returns(false);

            // Act
            await _presenter.Delete();

            // Assert
            _apiClient.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            _mainView
                .Setup(x => x.ConfirmDelete())
                .Returns(true);

            var response = new OperationResult();
            response.AddError("Test error");
            _apiClient
                .Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(response);

            // Act
            await _presenter.Delete();

            // Assert
            _mainView.Verify(x => x.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()), Times.Once);
            _apiClient.Verify(x => x.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}
