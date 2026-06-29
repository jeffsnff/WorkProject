# WorkProject

## Work Items To Do  

- [x] Consume API to display books
- [x] Put books in a data table
- [x] Add Search to search books on Title and Author
- [ ] Add ability to click on book record then display more information about that book

## My working Blog

### 2026-06-23

I decided today I should have been keeping a blog on this project. I watched a talk that is called Thriving in AI Era. The presenter main points were;

- Talk about bugs you encountered and how you solved them
- Talk about architecture trade offs and what you decided on and why
- Reflect on the experience during the project

That is what the blog will be about. I will do an overview for each section that I have done so far breaking it down from the initial GET request to getting data for a single book and displaying it to the screen. Then from there, I will blog about what I did for that day, what decisions I made, what I struggled with and what I learned. I expect this to also give me practice in writing as well as patience. Already I know I will be rewriting things I write to make them more concise and clear to the reader.

#### Project Idea

The goal of this project is to learn how to request data from an external API and display that data to the user using Blazor and compoenets.

#### API /books request

The first thing I had to figure out was how to consume an API. The Microsoft Learn tutorial I used for learning Blazor didn't focus on consuming and API and combined creating a database and using HttpClient to access the data. I did try to google search my questions, but I kept finding stuff about minimal API or creating my own backend which is not what I wanted. I ended up leaning on ChatGPT to help me implement this feature.\
It instructed me to add the following in Program.cs which is creating the base URL for my API Request in BookService.cs
```c#
builder.Services.AddHttpClient<BookService>(client =>
{
  client.BaseAddress = new Uri("https://simple-books-api.click");
});
```

Then it instructed me to create a Model for my data in BookSummary.cs
Because I wanted to rename the "name" property to "Title" I had to pass in a Json apptribute so the request would know to expect a "name" and put it in "Title" everything else could stay the same though since it matched.
```c#
using System.Text.Json.Serialization;

public class BookSummary
{
  public int Id { get; set; }
  [JsonPropertyName("name")]
  public string Title { get; set; } = string.Empty;
  public string Type { get; set; }  = string.Empty;
  public bool Available{ get; set; }
}
```
Then in BookService.cs I had to make a property for HttpClient which would handle the request, and create a method that would use an endpoint to request all the books.
```c#
public class BookService
{
  private readonly HttpClient _httpClient;

  public BookService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<List<BookSummary>> GetBooksAsync()
  {
    var bookSummaries = await _httpClient.GetFromJsonAsync<List<BookSummary>>("books");

    return bookSummaries ?? new List<BookSummary>();
  }
}
```
Then in BooksList.razor I emplemented the API request
```c#
protected override async Task OnInitializedAsync()
  {
    books = await BookService.GetBooksAsync();
  }
```
which to confirm it worked, I did a simple foreach loop to go through the list and display the books in the console.

##### What I learned
I did learn a bit about HttpClient through this process however I didn't learn it well. I definitly need to do a tutorial on this and practice building it from scratch on my own which will help me greatly for future frontend development

#### Displaying Data in a table
I wanted to use a built in table feature to display the books in Blazor. At work, they use Syncfusion and I was wondering why not to use Blazor built in tools to create tables. 
I tried implementing QuickGrid, but I was running into errors that I was struggling to understand. I noticed I was leaning heavily on ChatGPT to get things working even for a simple project so I took a break and asked myself what was the point of this project. The point of this project was to practice Blazor and get comfortable with programming again so I can apply for a developer position. So I scrapped QuickGrid in favor of a simple HTML table. I did have to search questions that I was running into, but instead of using ChatGPT, I was using documentation from https://www.w3schools.com/html/html_tables.asp and https://developer.mozilla.org/en-US/docs/Web/HTML/Reference/Elements/table.

##### What I learned
I need to slow down when using new features. In a few hours, I was able to implement the API GET request and display some data to the console. This may not seem like much, but I am new to programming in C# and I need to learn to slowdown and read the documentation better. Yes, I did get something to work, however, I used ChatGPT to much and that was hindering my learning. I would like to revisit this in the future again but right now, I am going in a different direction.

#### API /books/id request
Next I needed to implement a single book request. The flow would be a user would click on a title that want more information about and that would display a modal with more book information from the /books/id endpoint. To start off, I figured out how to add an onclick handler for each record that would display the book.Id from the record that was clicked. First I got it displaying to the console then I wanted to display it on the website. That took awhile and I had to add @rendermode InteractiveServer to get it to work correctly. From there I created a new API request in BookServices using the already created GET all books request. I was able to confirm it worked by logging in the console a single book title, author and id.

##### What did I learn
If there is any type of interactivity by the user, you need to set rendermode to InteractiveServer. I talked to a developer at work about this and they were struggling with the Auto working intermentiatly. This made me think of creating a template that I could just copy and rename as I need new Razor Components that would have some html, the code block, pages, some basic using statements and InteractiveServer already configured.

#### Displaying book data in a modal

That brings me to today. Today I wanted to display the book information in a modal. I used a this blog post https://amarozka.dev/blazor-custom-modal-component/ to help me create a modal. I ended up scrapping a lot of what it implemented since I didn't quite understand everything he was talking about. My first goal was to just get it to render. I achieved this by creating an if statement that when showModal is true would render the modal. However, I was unable to close it. I created a button in the Modal that would set IsVisible which is a property in the modal to false and though that worked, I was unable to open the modal again. I used the blogs Close method inside the modal to implement a simple close method of my own, pass that to the Modal and I was able to open and close the modal as I liked. 

Now I had to figure out how to pass book information to the modal. I was unable to just pass @book.Title based on where the Modal was being rendered. If I tried to render the modal in the table record, it turned the page black and just displayed the Modal. Looking through my @code block, I realized that I was initialized a book object then in the SelectBook method I was instaniating that object with the book the user selected. I decided I could use that same object and pass it to the Modal component instead of passing @book.Title, @book.Author etc which would be more efficeint anyways. 
At first, I wasn't able to get the data in the Modal. An error kept showing that book wasn't set to an object. 
This was what my SelectBook method looked liked

```csharp
public async Task SelectBook(int id)
  {
    showModal = true;
    book = await BookService.GetBookAsync(id);
    Console.WriteLine($"{book.Id} - {book.Author} - {book.Title}");
    message = $"Record {id} Clicked!";
  }
```

The issue is when this method is called, the first thing it does is set showModal to true. This displays the modal right away before the book object gets any information from the GetBookAsync method. 
I moved showModal = true to the bottom so it would execute last. Once I did that, I was able to display book.Title in the modal.

##### What did I learn
It is obvious, but code execution does matter. Just like PEMDAS which is math done in a certain order or you will come to a different conclusion, code is executed from top to bottom.
As I learn more C# and Blazor and design practices, I need to revisit that blog. There was a lot of good information there, but I am definitly hacking along and not engineering (in my eyes).

#### Designing the Modal
I have never been good at design. The best website I have ever created I was able to use a friend who drafted me up a design in Figma and I used to design my website.
I asked ChatGPT, based on the data I wanted to display to give me a design for the modal. I then used that design to create my modal. I got the icons from an open source library.
This was all finished today and I am happy with the results. I had to do some searching to figure out how to get things done, like the lines which are <hr /> tags as well as the CSS for them.

##### What did I learn
I wanted to do less class names for CSS and just use tags. For the icons, I used <img /> tags and I was able to find a way to select images based on their source for the price and availability information to display them green and red.
While building the design, I had to figure out how to make a horizontal line. I came across the <hr> tag which is used to show a change in topic in HTML. I never knew about this tag before, but it makes it really easy to make a line.