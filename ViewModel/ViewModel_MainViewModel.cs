using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using MovieApp.Model;

namespace MovieApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Movie> Movies { get; set; }
        public ICollectionView FilteredMovies { get; }
        public string NewMovieTitle { get; set; }
        public string FilterText { get; set; }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }

        private Movie _selectedMovie;
        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                _selectedMovie = value;
                OnPropertyChanged(nameof(SelectedMovie));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            Movies = new ObservableCollection<Movie>();
            FilteredMovies = CollectionViewSource.GetDefaultView(Movies);
            FilteredMovies.Filter = FilterMovies;
            AddCommand = new RelayCommand(AddMovie, () => !string.IsNullOrWhiteSpace(NewMovieTitle));
            RemoveCommand = new RelayCommand(RemoveMovie, () => SelectedMovie != null);
        }

        private bool FilterMovies(object obj)
        {
            if (obj is Movie movie)
            {
                return string.IsNullOrEmpty(FilterText) || movie.Title.ToLower().Contains(FilterText.ToLower());
            }
            return false;
        }

        private void AddMovie()
        {
            Movies.Add(new Movie { Title = NewMovieTitle });
            NewMovieTitle = string.Empty;
            OnPropertyChanged(nameof(NewMovieTitle));
            FilteredMovies.Refresh();
        }

        private void RemoveMovie()
        {
            if (SelectedMovie != null)
            {
                Movies.Remove(SelectedMovie);
                FilteredMovies.Refresh();
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}