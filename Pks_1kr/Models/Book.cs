using System;
using System.ComponentModel.DataAnnotations;

namespace Pks_1kr.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = "";
        
        [Required]
        [MaxLength(13)]
        public string ISBN { get; set; } = "";
        
        public int PublishYear { get; set; }
        
        public int QuantityInStock { get; set; }
        
        // Внешние ключи
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        
        // Навигационные свойства
        public Author Author { get; set; } = null!;
        public Genre Genre { get; set; } = null!;
    }
}