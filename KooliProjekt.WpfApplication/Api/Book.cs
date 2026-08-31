using System.ComponentModel;

namespace KooliProjekt.WpfApplication
{
    public class Book : NotifyPropertyChangedBase
    {
        private int _id;
        private string _title;
        private int _year;
        private int _authorId;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }
        public int Year
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
                NotifyPropertyChanged();
            }
        }
        public int AuthorId
        {
            get
            {
                return _authorId;
            }
            set
            {
                _authorId = value;
                NotifyPropertyChanged();
            }
        }
    }
}
