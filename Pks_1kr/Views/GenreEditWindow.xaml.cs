using System.Windows;
using Pks_1kr.Models;

namespace Pks_1kr.Views
{
    public partial class GenreEditWindow : Window
    {
        public Genre Genre { get; set; }
        
        public GenreEditWindow(Genre genre)
        {
            InitializeComponent();
            Genre = genre;
            DataContext = Genre;
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