using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FacesTest.Models;
using FacesTest.Services;
using FacesTest.DTOs;
//using System.Web.Http.Description;

namespace FacesTest.Controllers
{

    [Route("api/Person")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        // Database context
        private readonly FacesContext _context;
        // Class containing methods for working with Person
        private readonly PersonService _personService;

        public PersonController(FacesContext context)
        {
            _context = context;
            _personService = new PersonService(context);
        }

        // GET: localhost:44376/api/person
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetPerson()
        {
           return await _personService.GetPerson();
        }

        // GET: localhost:44376/api/person/1
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDto>> GetPerson(long id)
        {
            var person = await _personService.GetPerson(id);
            if (person == null)
            {
                return NotFound();
            }
            return person;
        }

        // PUT: localhost:44376/api/person/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(long id, PersonDto person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != person.Id || !_personService.PersonExists(person.Id))
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

        // POST: localhost:44376/api/person
        [HttpPost]
        public async Task<ActionResult<PersonDto>> PostPerson(PersonDto person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            person = await _personService.PostPerson(person);
            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }

        // DELETE: localhost:44376/api/person/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<PersonDto>> DeletePerson(long id)
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
