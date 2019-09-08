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
    public class SeminariesController : ControllerBase
    {
        private readonly ILogger<SeminariesController> _logger;
        private readonly IRepository<Seminary> _seminaryRepository;

        public SeminariesController(ILogger<SeminariesController> logger, IRepository<Seminary> seminaryRepository)
        {
            _logger = logger;
            _seminaryRepository = seminaryRepository;
        }

        [HttpPost]
        public ActionResult<Seminary> Create([FromBody] Seminary seminary)
        {
            int newId = _seminaryRepository.Create(seminary);
            if (newId == 0)
            {
                return BadRequest();
            }
            seminary.Id = newId;
            return seminary;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            try
            {
                bool success = _seminaryRepository.Delete(id);
                if (success)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        public IEnumerable<Seminary> GetAll()
        {
            return _seminaryRepository.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Seminary> GetById(int id)
        {
            var result = _seminaryRepository.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPut("{id}")]
        public Seminary Update(int id, [FromBody] Seminary seminary)
        {
            seminary.Id = id;
            return _seminaryRepository.Update(seminary);
        }
    }
}
