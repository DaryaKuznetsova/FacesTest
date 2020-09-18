using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FacesTest.Models;
using FacesTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FacesTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Find_PersonController : ControllerBase
    {
        private readonly FacesContext _context;
        private readonly FaceService _faceService;

        public Find_PersonController(FacesContext context)
        {
            _context = context;
            _faceService = new FaceService(context);
        }

        [HttpPost]
        public async Task<ActionResult<Person>> PostFace(IFormFile face)
        {
            //Person person = new Person { Name = pvm.Name };
            if (face != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(face.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)face.Length);
                }
                // установка массива байтов
                var person = await _faceService.FindPerson(imageData);
                if (person != null) return person; else return NotFound();
            }
            return BadRequest();
        }
    }
}
