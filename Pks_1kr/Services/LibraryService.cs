using Microsoft.EntityFrameworkCore;
using Pks_1kr.Data;
using Pks_1kr.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pks_1kr.Services
{
    public class LibraryService
    {
        private readonly LibraryContext _context;
        
        public LibraryService()
        {
            _context = new LibraryContext();
            _context.Database.EnsureCreated();
        }
        
        // Книги
        public List<Book> GetAllBooks()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .ToList();
        }
        
        public List<Book> GetBooksByFilter(string searchTitle, int? authorId, int? genreId)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(searchTitle))
                query = query.Where(b => b.Title.Contains(searchTitle));
            
            if (authorId.HasValue)
                query = query.Where(b => b.AuthorId == authorId);
            
            if (genreId.HasValue)
                query = query.Where(b => b.GenreId == genreId);
            
            return query.ToList();
        }
        
        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }
        
        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }
        
        public void DeleteBook(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
        }
        
        // Авторы
        public List<Author> GetAllAuthors()
        {
            return _context.Authors.ToList();
        }
        
        public void AddAuthor(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
        }
        
        public void UpdateAuthor(Author author)
        {
            _context.Authors.Update(author);
            _context.SaveChanges();
        }
        
        public void DeleteAuthor(Author author)
        {
            _context.Authors.Remove(author);
            _context.SaveChanges();
        }
        
        // Жанры
        public List<Genre> GetAllGenres()
        {
            return _context.Genres.ToList();
        }
        
        public void AddGenre(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
        }
        
        public void UpdateGenre(Genre genre)
        {
            _context.Genres.Update(genre);
            _context.SaveChanges();
        }
        
        public void DeleteGenre(Genre genre)
        {
            _context.Genres.Remove(genre);
            _context.SaveChanges();
        }
    }
}