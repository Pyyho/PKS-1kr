using System.Collections.Generic;
using System.Windows;
using Pks_1kr.Models;

namespace Pks_1kr.Views
{
    public partial class BookEditWindow : Window
    {
        public Book Book { get; set; }
        
        public BookEditWindow(Book book, List<Author> authors, List<Genre> genres)
        {
            InitializeComponent();
            Book = book;
            DataContext = Book;
            
            AuthorCombo.ItemsSource = authors;
            GenreCombo.ItemsSource = genres;
        }
        
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}