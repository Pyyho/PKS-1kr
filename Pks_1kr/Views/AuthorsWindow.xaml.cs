using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Pks_1kr.Models;
using Pks_1kr.Services;

namespace Pks_1kr.Views
{
    public partial class AuthorsWindow : Window
    {
        private readonly LibraryService _libraryService;
        private ObservableCollection<Author> _authors;
        
        public AuthorsWindow(LibraryService libraryService)
        {
            InitializeComponent();
            _libraryService = libraryService;
            LoadData();
        }
        
        private void LoadData()
        {
            _authors = new ObservableCollection<Author>(_libraryService.GetAllAuthors());
            AuthorsGrid.ItemsSource = _authors;
        }
        
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AuthorEditWindow(new Author());
            if (dialog.ShowDialog() == true)
            {
                _libraryService.AddAuthor(dialog.Author);
                LoadData();
            }
        }
        
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsGrid.SelectedItem is Author selected)
            {
                var dialog = new AuthorEditWindow(selected);
                if (dialog.ShowDialog() == true)
                {
                    _libraryService.UpdateAuthor(dialog.Author);
                    LoadData();
                }
            }
        }
        
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsGrid.SelectedItem is Author selected)
            {
                var result = MessageBox.Show(
                    $"Удалить автора {selected.FullName}?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                    
                if (result == MessageBoxResult.Yes)
                {
                    _libraryService.DeleteAuthor(selected);
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