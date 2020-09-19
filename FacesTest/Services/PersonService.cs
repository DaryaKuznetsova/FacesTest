using FacesTest.DTOs;
using FacesTest.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FacesTest.Services
{
    public class PersonService
    {
        private readonly FacesContext _context;

        private static readonly Expression<Func<Person, PersonDto>> AsPersonDto =
    x => new PersonDto
    {
        Id = x.Id,
        Surname = x.Surname,
        Name = x.Name,
        MiddleName = x.MiddleName
    };

        private static PersonDto PersonDto(Person person)
        {
            PersonDto dto = new PersonDto
            {
                Id = person.Id,
                Surname = person.Surname,
                Name = person.Name,
                MiddleName = person.MiddleName
            };
            return dto;
        }

        public PersonService(FacesContext context)
        {
            _context = context;
        }
        public async Task<List<PersonDto>> GetPerson()
        {
            return await _context.People.Select(AsPersonDto).ToListAsync();
        }

        public async Task<PersonDto> GetPerson(long id)
        {
            var person = await _context.People.Select(AsPersonDto).FirstOrDefaultAsync(m => m.Id == id);
            return person;
        }

        public async Task PutPerson(PersonDto personDto)
        {
            var person = await _context.People.FindAsync(personDto.Id);
            person.Name = personDto.Name;
            person.Surname = personDto.Surname;
            person.MiddleName = personDto.MiddleName;
            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public bool PersonExists(long id)
        {
            return _context.People.Any(e => e.Id == id);
        }

        public async Task<PersonDto> PostPerson(PersonDto personDto)
        {
            var person = new Person()
            {
                Name = personDto.Name,
                Surname = personDto.Surname,
                MiddleName = personDto.MiddleName
            };
            _context.People.Add(person );
            await _context.SaveChangesAsync();
            return PersonDto(person);
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

        public async Task<PersonDto> FindPerson(byte[] face)
        {
            var foundFace = await _context.Faces.FirstOrDefaultAsync(x => x.Picture == face);
            if (foundFace != null)
            {
                return await _context.People.Select(AsPersonDto).FirstOrDefaultAsync(p => p.Id == foundFace.PersonId);
            }
            else return null;
        }
    }
}
