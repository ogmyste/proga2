using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class MainViewPresenter
    {
        private readonly IApiClient _apiClient;
        private readonly IMainView _mainView;

        private Book _selectedBook;

        public MainViewPresenter(IApiClient apiClient, IMainView mainView)
        {
            _apiClient = apiClient;
            _mainView = mainView;
            _mainView.SetPresenter(this);
        }

        public async Task LoadData()
        {
            var response = await _apiClient.List(1, 100);
            if (response.HasErrors)
            {
                _mainView.ShowError("Viga andmete laadimisel", response);
                _mainView.DataSource = null;
                return;
            }

            _mainView.DataSource = response.Value.Results;
        }

        public void SetSelection(Book selectedBook)
        {
            _selectedBook = selectedBook;
            if (_selectedBook == null)
            {
                _mainView.CurrentId = 0;
                _mainView.CurrentTitle = "";
                _mainView.CurrentYear = 0;
                _mainView.CurrentAuthorId = 0;
            }
            else
            {
                _mainView.CurrentId = _selectedBook.Id;
                _mainView.CurrentTitle = _selectedBook.Title;
                _mainView.CurrentYear = _selectedBook.Year;
                _mainView.CurrentAuthorId = _selectedBook.AuthorId;
            }
        }

        public async Task Save()
        {
            var book = new Book();
            book.Id = _mainView.CurrentId;
            book.Title = _mainView.CurrentTitle;
            book.Year = _mainView.CurrentYear;
            book.AuthorId = _mainView.CurrentAuthorId;

            var result = await _apiClient.Save(book);
            if (result.HasErrors)
            {
                _mainView.ShowError("Viga salvestamisel", result);
                return;
            }

            await LoadData();
        }

        public async Task Delete()
        {
            if (!_mainView.ConfirmDelete())
            {
                return;
            }

            var result = await _apiClient.Delete(_mainView.CurrentId);
            if (result.HasErrors)
            {
                _mainView.ShowError("Viga kustutamisel", result);
                return;
            }

            await LoadData();
        }
    }
}
