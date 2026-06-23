using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;

public class BookDetails
{
  public int Id { get; set; }
  public bool Available { get; set; }
  public int ISBN { get; set; }
  [JsonPropertyName("name")]
  public string Title { get; set; } = string.Empty;
  public double Price { get; set; }
  public string Author { get; set; } = string.Empty;
  public string Type { get; set; } = string.Empty;
}