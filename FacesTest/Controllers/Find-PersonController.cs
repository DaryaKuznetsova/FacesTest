using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FacesTest.DTOs;
using FacesTest.Models;
using FacesTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FacesTest.Controllers
{
    [Route("api/find-person")]
    [ApiController]
    public class Find_PersonController : ControllerBase
    {
        private readonly FacesContext _context;
        private readonly PersonService _personService;

        public Find_PersonController(FacesContext context)
        {
            _context = context;
            _personService = new PersonService(context);
        }

        [HttpPost]
        public async Task<ActionResult<PersonDto>> PostFace(IFormFile filePicture)
        {
            if (filePicture != null)
            {
                byte[] imageData = null;
                // read file to byte array
                using (var binaryReader = new BinaryReader(filePicture.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)filePicture.Length);
                }
                // find person
                var person = await _personService.FindPerson(imageData);
                if (person != null) return person; else return NotFound();
            }
            return BadRequest();
        }
    }
}
