using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using server.Mappers;
using server.Models;
using server.Repositories;

namespace server.Controllers
{
    [ApiController]
    [Route("api/commentaryBooks")]
    public class CommentaryBooksController : ControllerBase
    {
        private readonly ILogger<CommentaryBooksController> _logger;
        private readonly IRepository<CommentaryBook> _commentaryBookRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IMapper<CommentaryBook> _commentaryBookMapper;

        public CommentaryBooksController(ILogger<CommentaryBooksController> logger, IRepository<CommentaryBook> commentaryBookRepository, IRepository<Book> bookRepository, IMapper<CommentaryBook> commentaryBookMapper)
        {
            _logger = logger;
            _commentaryBookRepository = commentaryBookRepository;
            _bookRepository = bookRepository;
            _commentaryBookMapper = commentaryBookMapper;
        }

        [HttpGet]
        public IEnumerable<CommentaryBook> GetAll()
        {
            return _commentaryBookRepository.GetAll();
        }

        [HttpGet("{commentaryId}")]
        public ActionResult<IEnumerable<Book>> GetBooksForCommentary(int commentaryId)
        {
            var result = new List<Book>();
            try
            {
                using (var connection = new MySqlConnection(Startup.ConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT * FROM CommentaryBook WHERE commentary_id = {commentaryId}";
                    var command = new MySqlCommand(query, connection);
                    var commentaryBooks = new List<CommentaryBook>();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new List<object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader[i]);
                            }
                            commentaryBooks.Add(_commentaryBookMapper.FromFieldList(row));
                        }
                    }

                    if (commentaryBooks.Count == 0)
                    {
                        return NotFound();
                    }

                    foreach (var commentaryBook in commentaryBooks)
                    {
                        result.Add(_bookRepository.GetById(commentaryBook.BookId));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Commentary Book Controller (GetBooksForCommentary): {ex.Message}");
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
