using WebBanSach.Data;
using WebBanSach.Models.Domain;

namespace WebBanSach.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
        {
            _webHostEnviroment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public Image Upload(Image image)
        {
            var localFilePath = Path.Combine(_webHostEnviroment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            // upload Image to local Path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            image.File.CopyTo(stream);

            // https://localhost:8080/images/image.jpg
            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                $"{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}" +
                $"/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            //add Image to the Images table
            _dbContext.Images.Add(image);
            _dbContext.SaveChanges();

            return image;
        }

        public List<Image> GetAllInfoImages()
        {
            var allImages = _dbContext.Images.ToList();
            return allImages;
        }

        public (byte[], string, string) DownloadFile(int Id)
        {
            try
            {
                var FileById = _dbContext.Images.Where(x => x.Id == Id).FirstOrDefault();
                var path = Path.Combine(_webHostEnviroment.ContentRootPath, "Images",
                    $"{FileById.FileName}{FileById.FileExtension}");
                var stream = File.ReadAllBytes(path);
                var fileName = FileById.FileName + FileById.FileExtension;
                return (stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}