using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pks_1kr.Models
{
    public class Genre
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = "";
        
        [MaxLength(500)]
        public string Description { get; set; } = "";
        
        // Навигационное свойство
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}