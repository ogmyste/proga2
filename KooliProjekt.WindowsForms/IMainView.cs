using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public interface IMainView
    {
        IList<Book> DataSource { get; set; }
        Book SelectedItem { get; set; }
        void SetPresenter(MainViewPresenter presenter);
        void ShowError(string message, OperationResult result);
        int CurrentId { get; set; }
        string CurrentTitle { get; set; }
        int CurrentYear { get; set; }
        int CurrentAuthorId { get; set; }
        bool ConfirmDelete();
    }
}
