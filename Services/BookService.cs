public class BookService
{
  private readonly HttpClient _httpClient;

  // BookService Constructor
  public BookService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  /// <summary>
  /// Request to /books to get all /books
  /// </summary>
  /// <returns>List<BookSummary></returns>
  public async Task<List<BookSummary>> GetBooksAsync()
  {
    var bookSummaries = await _httpClient.GetFromJsonAsync<List<BookSummary>>("books");
    // If bookSummaries is null send empty BookSummary list
    return bookSummaries ?? new List<BookSummary>();
  }
/// <summary>
/// Gets details of a single book from /books/{id}
/// </summary>
/// <param name="id"></param>
/// <returns>BookDetail object</returns>
  public async Task<BookDetails> GetBookAsync(int id)
  {
    var bookDetails = await _httpClient.GetFromJsonAsync<BookDetails>($"books/{id}");
    return bookDetails ?? new BookDetails();
  }
}