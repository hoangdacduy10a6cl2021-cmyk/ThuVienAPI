using System.ComponentModel.DataAnnotations;

namespace WebAPI_simple.Models.Domain
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public int? Rate { get; set; }
        public string Genre { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }

        // Navigation - một Publisher có nhiều Books
        public int PublisherID { get; set; }
        public Publisher Publisher { get; set; }
        // Navigation - một Book có nhiều Book_Author
        public List<Book_Author> Book_Authors { get; set; }
    }
}