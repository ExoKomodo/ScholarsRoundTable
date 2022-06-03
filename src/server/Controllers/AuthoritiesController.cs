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
    public class AuthoritiesController : ControllerBase
    {
        private readonly ILogger<AuthoritiesController> _logger;
        private readonly IRepository<Authority> _authorityRepository;

        public AuthoritiesController(ILogger<AuthoritiesController> logger, IRepository<Authority> authorityRepository)
        {
            _logger = logger;
            _authorityRepository = authorityRepository;
        }

        [HttpPost]
        public ActionResult<Authority> Create([FromBody] Authority authority)
        {
            int newId = _authorityRepository.Create(authority);
            if (newId == 0)
            {
                return BadRequest();
            }
            authority.Id = newId;
            return authority;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            try
            {
                bool success = _authorityRepository.Delete(id);
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
        public IEnumerable<Authority> GetAll()
        {
            return _authorityRepository.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Authority> GetById(int id)
        {
            var result = _authorityRepository.GetById(id);
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
        public Authority Update(int id, [FromBody] Authority authority)
        {
            authority.Id = id;
            return _authorityRepository.Update(authority);
        }
    }
}
