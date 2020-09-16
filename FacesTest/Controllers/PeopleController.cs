using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacesTest.Models;
using FacesTest.Services;

namespace FacesTest.Controllers
{
    [Route("api/People")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly FacesContext _context;
        private readonly PersonService _personService;

        public PeopleController(FacesContext context)
        {
            _context = context;
            _personService = new PersonService(context);

            if (!_context.People.Any())
            {
                _context.People.Add(new Person { Name = "Tom" });
                _context.People.Add(new Person { Name = "Alice" });
                _context.SaveChanges();
            }
        }


        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPerson()
        {
            return await _personService.GetPerson();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await _personService.GetPerson(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }


        // PUT: api/People/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(long id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }            

            try
            {
                await _personService.PutPerson(person);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_personService.PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // api/People
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            await _personService.PostPerson(person);
            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Person>> DeletePerson(long id)
        {
            var person = await _personService.DeletePerson(id);
            if (person == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
