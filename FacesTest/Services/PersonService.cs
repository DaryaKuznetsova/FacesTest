using FacesTest.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesTest.Services
{
    public class PersonService
    {
        private readonly FacesContext _context;
        public PersonService(FacesContext context)
        {
            _context = context;
        }
        public async Task<List<Person>> GetPerson()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<Person> GetPerson(long id)
        {
            var person = await _context.People.FindAsync(id);

            return person;
        }

        public async Task PutPerson(Person person)
        {
            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public bool PersonExists(long id)
        {
            return _context.People.Any(e => e.Id == id);
        }

        public async Task PostPerson(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();
        }

        public async Task<Person> DeletePerson(long id)
        {
            var person = await _context.People.FindAsync(id);
            try
            {
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
            }
            catch
            {

            }
            return person;

        }
    }
}
