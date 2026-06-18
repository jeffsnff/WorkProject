using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class BookSummary
{
  public int Id { get; set; }
  [JsonPropertyName("name")]
  public string Title { get; set; } = string.Empty;
  public string Type { get; set; }  = string.Empty; // Want to make this an enum eventually
  public bool Available{ get; set; }
}