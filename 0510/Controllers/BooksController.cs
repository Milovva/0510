using _0510.Data;
using _0510.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace _0510.Controllers
{
    public class BooksController : Controller
    {
        private readonly string _connectionString;

        public BooksController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IActionResult> Index()
        {
            var books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Books", connection);
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var book = new Book
                    {
                        Id = reader.GetGuid(0),
                        Title = reader.GetString(1),
                        Author = reader.GetString(2),
                        Genre = reader.GetString(3),
                        Price = reader.GetDecimal(4),
                        Comments = new List<Comment>() 
                    };

                    books.Add(book);
                }
            }

            foreach (var book in books)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("SELECT * FROM Comments WHERE BookId = @BookId", connection);
                    command.Parameters.AddWithValue("@BookId", book.Id);
                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        var comment = new Comment
                        {
                            Id = reader.GetGuid(0),
                            Content = reader.GetString(1),
                            CreatedAt = reader.GetDateTime(2),
                            BookId = book.Id
                        };
                        book.Comments.Add(comment);
                    }
                }
            }

            return View(books); 
        }

        [HttpPost]
        public async Task<IActionResult> Comment(Guid bookId, string commentText)
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Content = commentText,
                CreatedAt = DateTime.UtcNow,
                BookId = bookId
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Comments (Id, Content, CreatedAt, BookId) VALUES (@Id, @Content, @CreatedAt, @BookId)", connection);
                command.Parameters.AddWithValue("@Id", comment.Id);
                command.Parameters.AddWithValue("@Content", comment.Content);
                command.Parameters.AddWithValue("@CreatedAt", comment.CreatedAt);
                command.Parameters.AddWithValue("@BookId", comment.BookId);
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
