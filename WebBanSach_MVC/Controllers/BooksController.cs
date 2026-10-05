using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WebBanSach_MVC.Models.DTO;

namespace WebBanSach_MVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private const string ApiBaseUrl = "https://localhost:7207";

        public BooksController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        // GET: Books/Index
        public async Task<IActionResult> Index([FromQuery] string filterOn = null, [FromQuery] string filterQuery = null,
            [FromQuery] string sortBy = null, bool isAscending = true)
        {
            List<BookDTO> response = new List<BookDTO>();
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMess = await client.GetAsync($"{ApiBaseUrl}/api/Books/get-all-books?" +
                    $"filterOn=" + filterOn + "&filterQuery=" + filterQuery + "&sortBy=" + sortBy + "&isAscending=" + isAscending);
                httpResponseMess.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMess.Content.ReadFromJsonAsync<IEnumerable<BookDTO>>());
            }
            catch (Exception ex)
            {
                //log the exception
            }
            return View(response);
        }

        // GET: Books/addBook
        [HttpGet]
        public async Task<IActionResult> addBook()
        {
            var client = httpClientFactory.CreateClient();

            List<authorDTO> responseAu = new List<authorDTO>();
            var httpResponseAu = await client.GetAsync($"{ApiBaseUrl}/api/Authors/get-all-author");
            httpResponseAu.EnsureSuccessStatusCode();
            responseAu.AddRange(await httpResponseAu.Content.ReadFromJsonAsync<IEnumerable<authorDTO>>());
            ViewBag.listAuthor = responseAu;

            List<publisherDTO> responsePu = new List<publisherDTO>();
            var httpResponsePu = await client.GetAsync($"{ApiBaseUrl}/api/Publishers/get-all-publisher");
            httpResponsePu.EnsureSuccessStatusCode();
            responsePu.AddRange(await httpResponsePu.Content.ReadFromJsonAsync<IEnumerable<publisherDTO>>());
            ViewBag.listPublisher = responsePu;

            return View();
        }

        // POST: Books/addBook
        [HttpPost]
        public async Task<IActionResult> addBook(addBookDTO addBookDTO)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpRequestMess = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{ApiBaseUrl}/api/Books/add-book"),
                    Content = new StringContent(JsonSerializer.Serialize(addBookDTO), Encoding.UTF8,
                        "application/json")
                };

                var httpResponseMess = await client.SendAsync(httpRequestMess);
                httpResponseMess.EnsureSuccessStatusCode();
                var response = await httpResponseMess.Content.ReadFromJsonAsync<addBookDTO>();
                if (response != null)
                {
                    return RedirectToAction("Index", "Books");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        // GET: Books/listBook/5
        public async Task<IActionResult> listBook(int id)
        {
            BookDTO response = new BookDTO();
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMess = await client.GetAsync($"{ApiBaseUrl}/api/Books/get-book-by-id/" + id);
                httpResponseMess.EnsureSuccessStatusCode();
                var stringResponseBody = await httpResponseMess.Content.ReadAsStringAsync();
                response = await httpResponseMess.Content.ReadFromJsonAsync<BookDTO>();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }

        // GET: Books/editBook/5
        [HttpGet]
        public async Task<IActionResult> editBook(int id)
        {
            BookDTO responseBook = new BookDTO();
            var client = httpClientFactory.CreateClient();
            var httpResponseMess = await client.GetAsync($"{ApiBaseUrl}/api/Books/get-book-by-id/" + id);
            httpResponseMess.EnsureSuccessStatusCode();
            responseBook = await httpResponseMess.Content.ReadFromJsonAsync<BookDTO>();
            ViewBag.Book = responseBook;

            List<authorDTO> responseAu = new List<authorDTO>();
            var httpResponseAu = await client.GetAsync($"{ApiBaseUrl}/api/Authors/get-all-author");
            httpResponseAu.EnsureSuccessStatusCode();
            responseAu.AddRange(await httpResponseAu.Content.ReadFromJsonAsync<IEnumerable<authorDTO>>());
            ViewBag.listAuthor = responseAu;

            List<publisherDTO> responsePu = new List<publisherDTO>();
            var httpResponsePu = await client.GetAsync($"{ApiBaseUrl}/api/Publishers/get-all-publisher");
            httpResponsePu.EnsureSuccessStatusCode();
            responsePu.AddRange(await httpResponsePu.Content.ReadFromJsonAsync<IEnumerable<publisherDTO>>());
            ViewBag.listPublisher = responsePu;

            return View();
        }

        // POST: Books/editBook/5
        [HttpPost]
        public async Task<IActionResult> editBook([FromRoute] int id, editBookDTO bookDTO)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpRequestMess = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"{ApiBaseUrl}/api/Books/update-book-by-id/" + id),
                    Content = new StringContent(JsonSerializer.Serialize(bookDTO), Encoding.UTF8,
                        "application/json")
                };

                var httpResponseMess = await client.SendAsync(httpRequestMess);
                httpResponseMess.EnsureSuccessStatusCode();
                var response = await httpResponseMess.Content.ReadFromJsonAsync<addBookDTO>();
                if (response != null)
                {
                    return RedirectToAction("Index", "Books");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        // GET: Books/delBook/5
        [HttpGet]
        public async Task<IActionResult> delBook([FromRoute] int id)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMess = await client.DeleteAsync($"{ApiBaseUrl}/api/Books/delete-book-by-id/" + id);
                httpResponseMess.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Books");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View("Index");
        }
    }
}