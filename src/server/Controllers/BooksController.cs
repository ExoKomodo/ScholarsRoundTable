using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using server.Models;
using server.Repositories;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("Default")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IRepository<Book> _bookRepository;

        public BooksController(ILogger<BooksController> logger, IRepository<Book> bookRepository)
        {
            _logger = logger;
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public IEnumerable<Book> GetAll()
        {
            return _bookRepository.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetById(int id)
        {
            var result = _bookRepository.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
