using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Pks_1kr.Models;
using Pks_1kr.Services;
using Pks_1kr.Views;

namespace Pks_1kr.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly LibraryService _libraryService;
        private readonly ObservableCollection<Book> _books;
        private readonly ObservableCollection<Author> _authors;
        private readonly ObservableCollection<Genre> _genres;
        private string _searchText;
        private Author? _selectedAuthor;
        private Genre? _selectedGenre;
        private Book? _selectedBook;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public MainViewModel()
        {
            _libraryService = new LibraryService();
            _books = new ObservableCollection<Book>();
            _authors = new ObservableCollection<Author>();
            _genres = new ObservableCollection<Genre>();
            _searchText = string.Empty;
            _selectedAuthor = null;
            _selectedGenre = null;
            _selectedBook = null;
            
            // Команды
            AddBookCommand = new RelayCommand(ExecuteAddBook);
            EditBookCommand = new RelayCommand(ExecuteEditBook, CanEditOrDeleteBook);
            DeleteBookCommand = new RelayCommand(ExecuteDeleteBook, CanEditOrDeleteBook);
            RefreshCommand = new RelayCommand(ExecuteRefresh);
            ManageAuthorsCommand = new RelayCommand(ExecuteManageAuthors);
            ManageGenresCommand = new RelayCommand(ExecuteManageGenres);
            
            LoadData();
        }
        
        private void LoadData()
        {
            var authorsList = _libraryService.GetAllAuthors();
            _authors.Clear();
            foreach (var author in authorsList)
            {
                _authors.Add(author);
            }
            
            var genresList = _libraryService.GetAllGenres();
            _genres.Clear();
            foreach (var genre in genresList)
            {
                _genres.Add(genre);
            }
            
            ApplyFilter();
        }
        
        private void ApplyFilter()
        {
            var booksList = _libraryService.GetBooksByFilter(
                SearchText, 
                SelectedAuthor?.Id, 
                SelectedGenre?.Id
            );
            
            _books.Clear();
            foreach (var book in booksList)
            {
                _books.Add(book);
            }
            OnPropertyChanged(nameof(Books));
        }
        
        // Свойства
        public ObservableCollection<Book> Books => _books;
        
        public ObservableCollection<Author> Authors => _authors;
        
        public ObservableCollection<Genre> Genres => _genres;
        
        public string SearchText
        {
            get => _searchText;
            set 
            { 
                _searchText = value ?? string.Empty; 
                OnPropertyChanged();
                ApplyFilter();
            }
        }
        
        public Author? SelectedAuthor
        {
            get => _selectedAuthor;
            set 
            { 
                _selectedAuthor = value; 
                OnPropertyChanged();
                ApplyFilter();
            }
        }
        
        public Genre? SelectedGenre
        {
            get => _selectedGenre;
            set 
            { 
                _selectedGenre = value; 
                OnPropertyChanged();
                ApplyFilter();
            }
        }
        
        public Book? SelectedBook
        {
            get => _selectedBook;
            set 
            { 
                _selectedBook = value; 
                OnPropertyChanged();
            }
        }
        
        // Команды
        public ICommand AddBookCommand { get; }
        public ICommand EditBookCommand { get; }
        public ICommand DeleteBookCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ManageAuthorsCommand { get; }
        public ICommand ManageGenresCommand { get; }
        
        private bool CanEditOrDeleteBook(object? param)
        {
            return SelectedBook != null;
        }
        
        private void ExecuteAddBook(object? param)
        {
            var authors = _libraryService.GetAllAuthors();
            var genres = _libraryService.GetAllGenres();
            
            var dialog = new BookEditWindow(new Book(), authors, genres);
            if (dialog.ShowDialog() == true)
            {
                _libraryService.AddBook(dialog.Book);
                ApplyFilter();
            }
        }
        
        private void ExecuteEditBook(object? param)
        {
            if (SelectedBook == null) return;
            
            var bookCopy = new Book
            {
                Id = SelectedBook.Id,
                Title = SelectedBook.Title,
                ISBN = SelectedBook.ISBN,
                PublishYear = SelectedBook.PublishYear,
                QuantityInStock = SelectedBook.QuantityInStock,
                AuthorId = SelectedBook.AuthorId,
                GenreId = SelectedBook.GenreId
            };
            
            var authors = _libraryService.GetAllAuthors();
            var genres = _libraryService.GetAllGenres();
            
            var dialog = new BookEditWindow(bookCopy, authors, genres);
            if (dialog.ShowDialog() == true)
            {
                _libraryService.UpdateBook(dialog.Book);
                ApplyFilter();
            }
        }
        
        private void ExecuteDeleteBook(object? param)
        {
            if (SelectedBook == null) return;
            
            var result = System.Windows.MessageBox.Show(
                $"Удалить книгу '{SelectedBook.Title}'?",
                "Подтверждение",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);
                
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                _libraryService.DeleteBook(SelectedBook);
                ApplyFilter();
            }
        }
        
        private void ExecuteRefresh(object? param)
        {
            LoadData();
        }
        
        private void ExecuteManageAuthors(object? param)
        {
            var dialog = new AuthorsWindow(_libraryService);
            dialog.ShowDialog();
            
            // Обновляем список авторов после закрытия окна
            var authorsList = _libraryService.GetAllAuthors();
            _authors.Clear();
            foreach (var author in authorsList)
            {
                _authors.Add(author);
            }
            
            // Обновляем список книг (возможно, изменились авторы)
            ApplyFilter();
        }
        
        private void ExecuteManageGenres(object? param)
        {
            var dialog = new GenresWindow(_libraryService);
            dialog.ShowDialog();
            
            // Обновляем список жанров после закрытия окна
            var genresList = _libraryService.GetAllGenres();
            _genres.Clear();
            foreach (var genre in genresList)
            {
                _genres.Add(genre);
            }
            
            // Обновляем список книг (возможно, изменились жанры)
            ApplyFilter();
        }
        
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}