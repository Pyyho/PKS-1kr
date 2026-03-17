using Microsoft.EntityFrameworkCore;
using Pks_1kr.Models;

namespace Pks_1kr.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=LibraryDB;Trusted_Connection=True;");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация Author
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.Id);
                
                entity.Property(a => a.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);
                    
                entity.Property(a => a.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
                    
                entity.Property(a => a.Country)
                    .HasMaxLength(100);
            });
            
            // Конфигурация Genre
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(g => g.Id);
                
                entity.Property(g => g.Name)
                    .IsRequired()
                    .HasMaxLength(50);
                    
                entity.Property(g => g.Description)
                    .HasMaxLength(500);
            });
            
            // Конфигурация Book
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                
                entity.Property(b => b.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                    
                entity.Property(b => b.ISBN)
                    .IsRequired()
                    .HasMaxLength(13);
                    
                entity.Property(b => b.PublishYear)
                    .IsRequired();
                    
                entity.Property(b => b.QuantityInStock)
                    .IsRequired()
                    .HasDefaultValue(0);
                
                // Связь с Author (один-ко-многим)
                entity.HasOne(b => b.Author)
                    .WithMany(a => a.Books)
                    .HasForeignKey(b => b.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // Связь с Genre (один-ко-многим)
                entity.HasOne(b => b.Genre)
                    .WithMany(g => g.Books)
                    .HasForeignKey(b => b.GenreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Начальные данные
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Роман", Description = "Художественная литература" },
                new Genre { Id = 2, Name = "Научная фантастика", Description = "Жанр фантастики" },
                new Genre { Id = 3, Name = "Детектив", Description = "Преступления и расследования" }
            );
            
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FirstName = "Лев", LastName = "Толстой", BirthDate = new DateTime(1828, 9, 9), Country = "Россия" },
                new Author { Id = 2, FirstName = "Фёдор", LastName = "Достоевский", BirthDate = new DateTime(1821, 11, 11), Country = "Россия" }
            );
            
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Война и мир", ISBN = "978-5-17-123456-7", PublishYear = 1869, QuantityInStock = 10, AuthorId = 1, GenreId = 1 },
                new Book { Id = 2, Title = "Преступление и наказание", ISBN = "978-5-04-123456-8", PublishYear = 1866, QuantityInStock = 5, AuthorId = 2, GenreId = 3 }
            );
        }
    }
}