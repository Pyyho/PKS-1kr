using System.Collections.ObjectModel;
using System.Windows;
using Pks_1kr.Models;
using Pks_1kr.Services;

namespace Pks_1kr.Views
{
    public partial class GenresWindow : Window
    {
        private readonly LibraryService _libraryService;
        private readonly ObservableCollection<Genre> _genres;
        
        public GenresWindow(LibraryService libraryService)
        {
            InitializeComponent();
            _libraryService = libraryService;
            _genres = new ObservableCollection<Genre>();
            LoadData();
        }
        
        private void LoadData()
        {
            _genres.Clear();
            foreach (var genre in _libraryService.GetAllGenres())
            {
                _genres.Add(genre);
            }
            GenresGrid.ItemsSource = _genres;
        }
        
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new GenreEditWindow(new Genre());
            if (dialog.ShowDialog() == true)
            {
                _libraryService.AddGenre(dialog.Genre);
                LoadData();
            }
        }
        
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (GenresGrid.SelectedItem is Genre selected)
            {
                var dialog = new GenreEditWindow(selected);
                if (dialog.ShowDialog() == true)
                {
                    _libraryService.UpdateGenre(dialog.Genre);
                    LoadData();
                }
            }
        }
        
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (GenresGrid.SelectedItem is Genre selected)
            {
                var result = MessageBox.Show(
                    $"Удалить жанр {selected.Name}?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                    
                if (result == MessageBoxResult.Yes)
                {
                    _libraryService.DeleteGenre(selected);
                    LoadData();
                }
            }
        }
        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}