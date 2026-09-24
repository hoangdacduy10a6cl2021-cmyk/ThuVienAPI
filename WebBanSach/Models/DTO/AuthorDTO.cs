namespace WebBanSach.Models.DTO
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }

    public class AuthorNoIdDTO
    {
        public string FullName { get; set; }
    }
}