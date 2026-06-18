public class BookService
{
  private readonly HttpClient _httpClient;

  // BookService Constructor
  public BookService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  // API request to get book summary data
  public async Task<List<BookSummary>> GetBookAsync()
  {
    var bookSummaries = await _httpClient.GetFromJsonAsync<List<BookSummary>>("books");

    // Check is bookSummaries is null if it is, return an empty list
    return bookSummaries ?? new List<BookSummary>();
  }
}