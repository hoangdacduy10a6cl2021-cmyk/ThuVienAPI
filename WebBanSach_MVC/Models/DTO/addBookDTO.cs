namespace WebBanSach_MVC.Models.DTO
{
    public class addBookDTO
    {
        public string title { get; set; }
        public string? description { get; set; }
        public bool isRead { get; set; }
        public DateTime? dateRead { get; set; }
        public int? rate { get; set; }
        public string? genre { get; set; }
        public string? coverUrl { get; set; }
        public DateTime dateAdded { get; set; }
        public int publisherID { get; set; }
        public List<int> authorIds { get; set; }
    }
}