using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;

namespace KooliProjekt.WpfApplication.UnitTests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IDialogProvider> _dialogProviderMock;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _dialogProviderMock = new Mock<IDialogProvider>();
            _viewModel = new MainWindowViewModel(_apiClientMock.Object, _dialogProviderMock.Object);
        }

        [Fact]
        public void SelectedItem_should_return_correct_item()
        {
            // Arrange
            var item = new Book { Id = 1, Title = "Test" };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.Equal(item, _viewModel.SelectedItem);
        }

        [Fact]
        public void SelectedItem_should_call_notify_property_changed()
        {
            // Arrange
            var item = new Book { Id = 1, Title = "Test" };
            var propertyChangedRaised = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainWindowViewModel.SelectedItem))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public async Task LoadData_should_load_data_from_api_client()
        {
            // Arrange
            var apiResult = new OperationResult<PagedResult<Book>>
            {
                Value = new PagedResult<Book>
                {
                    Results = new List<Book>
                    {
                        new Book { Id = 1, Title = "Test 1" },
                        new Book { Id = 2, Title = "Test 2" }
                    }
                }
            };

            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(apiResult)
                .Verifiable();

            // Act            
            await _viewModel.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            Assert.Equal(2, _viewModel.Data.Count);
            Assert.Equal(1, _viewModel.Data[0].Id);
            Assert.Equal(2, _viewModel.Data[1].Id);
        }

        [Fact]
        public async Task LoadData_should_show_error_when_api_client_fails()
        {
            // Arrange
            var apiResult = new OperationResult<PagedResult<Book>>
            {
                Errors = new List<string> { "Error" }
            };

            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(apiResult)
                .Verifiable();

            // Act            
            await _viewModel.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            Assert.Empty(_viewModel.Data);
        }

        [Fact]
        public void AddNew_Command_Should_Set_Empty_SelectedItem()
        {
            // Act
            _viewModel.AddNewCommand.Execute(null);

            // Assert
            Assert.NotNull(_viewModel.SelectedItem);
            Assert.Equal(0, _viewModel.SelectedItem.Id);
        }

        [Fact]
        public void SaveCommand_should_load_data_if_no_errors()
        {
            // Arrange
            var loadDataApiResult = new OperationResult<PagedResult<Book>>
            {
                Value = new PagedResult<Book>
                {
                    Results = new List<Book>
                    {
                        new Book { Id = 1, Title = "Test 1" },
                        new Book { Id = 2, Title = "Test 2" }
                    }
                }
            };
            var saveDataApiResult = new OperationResult();
            var bookToSave = new Book { Id = 1, Title = "Test" };

            _apiClientMock.Setup(client => client.Save(It.IsAny<Book>()))
                .ReturnsAsync(saveDataApiResult)
                .Verifiable();
            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(loadDataApiResult)
                .Verifiable();

            // Act
            _viewModel.SaveCommand.Execute(bookToSave);

            // Assert
            _apiClientMock.VerifyAll();
        }

        [Fact]
        public void SaveCommand_should_return_when_api_gave_error()
        {
            // Arrange
            var saveDataApiResult = new OperationResult();
            saveDataApiResult.AddError("Test error");
            _apiClientMock.Setup(client => client.Save(It.IsAny<Book>()))
                .ReturnsAsync(saveDataApiResult)
                .Verifiable();

            // Act
            _viewModel.SaveCommand.Execute(new Book { Id = 1, Title = "Test" });

            // Assert
            _apiClientMock.VerifyAll();
            _dialogProviderMock.Verify(x => x.ShowError(It.IsAny<string>()), Times.Once);
            _apiClientMock.Verify(client => client.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void SaveCommand_can_execute_when_selected_item_is_not_null()
        {
            // Act && Assert
            Assert.False(_viewModel.SaveCommand.CanExecute(null));

            _viewModel.SelectedItem = new Book { Id = 1, Title = "Test" };

            Assert.True(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void DeleteCommand_should_return_when_no_confirmation()
        {
            // Arrange
            var item = new Book { Id = 1, Title = "Test" };
            _viewModel.SelectedItem = item;
            _dialogProviderMock.Setup(x => x.Confirm(It.IsAny<string>())).Returns(false);

            // Act
            _viewModel.DeleteCommand.Execute(item);

            // Assert
            _dialogProviderMock.Verify(x => x.Confirm(It.IsAny<string>()), Times.Once);
            _apiClientMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void DeleteCommand_should_load_data_if_no_errors()
        {
            // Arrange
            var item = new Book { Id = 1, Title = "Test" };
            _viewModel.SelectedItem = item;
            _dialogProviderMock.Setup(x => x.Confirm(It.IsAny<string>())).Returns(true);

            var loadDataApiResult = new OperationResult<PagedResult<Book>>
            {
                Value = new PagedResult<Book>
                {
                    Results = new List<Book> { item }
                }
            };
            _apiClientMock.Setup(client => client.Delete(It.IsAny<int>()))
                .ReturnsAsync(new OperationResult())
                .Verifiable();
            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(loadDataApiResult)
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(item);

            // Assert
            _apiClientMock.VerifyAll();
        }

        [Fact]
        public void DeleteCommand_should_return_when_api_gave_error()
        {
            // Arrange
            var item = new Book { Id = 1, Title = "Test" };
            _viewModel.SelectedItem = item;
            _dialogProviderMock.Setup(x => x.Confirm(It.IsAny<string>())).Returns(true);

            var deleteResult = new OperationResult();
            deleteResult.AddError("Test error");
            _apiClientMock.Setup(client => client.Delete(It.IsAny<int>()))
                .ReturnsAsync(deleteResult)
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(item);

            // Assert
            _apiClientMock.VerifyAll();
            _dialogProviderMock.Verify(x => x.ShowError(It.IsAny<string>()), Times.Once);
            _apiClientMock.Verify(client => client.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void DeleteCommand_can_execute_when_selected_item_is_not_null_and_id_is_not_zero()
        {
            // Act && Assert
            Assert.False(_viewModel.DeleteCommand.CanExecute(null));

            _viewModel.SelectedItem = new Book { Id = 0, Title = "New book" };
            Assert.False(_viewModel.DeleteCommand.CanExecute(null));

            _viewModel.SelectedItem = new Book { Id = 5, Title = "Test" };
            Assert.True(_viewModel.DeleteCommand.CanExecute(null));
        }
    }
}
