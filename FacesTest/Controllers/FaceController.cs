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
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace FacesTest.Controllers
{
    [Route("api/person/{personId}/[controller]")]
    [ApiController]
    public class FaceController : ControllerBase
    {
        private readonly FacesContext _context;
        // Class containing methods for working with Face
        private readonly FaceService _faceService;
        private readonly PersonService _personService;

        public FaceController(FacesContext context)
        {
            _context = context;
            _faceService = new FaceService(context);
            _personService = new PersonService(context);
        }

        //GET: localhost:44376/api/person/1/face
        public async Task<ActionResult<IEnumerable<FaceDto>>> GetFace(long personId)
        {
            return await _faceService.GetFaces(personId);
        }

        //GET: localhost:44376/api/person/1/face/36
        [HttpGet("{faceId}")]
        public async Task<ActionResult<FaceDto>> GetFace(long personId, long faceId)
        {
            var face = await _faceService.GetFace(personId, faceId);
            if (face == null) return NotFound();
            else return face;
        }


        [HttpPut("{faceId}")]
        public async Task<IActionResult> PutFace(long personId, long faceId, FaceDto face)
        {
            if (faceId != face.Id || !_faceService.FaceExists(personId, faceId))
            {
                return BadRequest();
            }

            try
            {
                await _faceService.PutFace(face);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_faceService.FaceExists(personId, faceId))
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

        [HttpDelete("{faceId}")]
        public async Task<ActionResult<Face>> DeleteFace(long personId, long faceId)
        {
            if (_faceService.FaceExists(personId, faceId))
            {
                var face = await _faceService.DeleteFace(faceId);
                if (face == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            else return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<Face>> PostFace(long personId, IFormFile filePicture)
        {
            if(_personService.PersonExists(personId))
            {
                Face face = new Face();
                if (filePicture != null)
                {
                    byte[] imageData = null;
                    // Reading the file into an array of bytes
                    using (var binaryReader = new BinaryReader(filePicture.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)filePicture.Length);
                    }
                    // Setting a byte array to the Face instance 
                    face.Picture = imageData;
                }
                var faceDto = await _faceService.PostFace(personId, face);
                return CreatedAtAction(nameof(GetFace), new { personId = face.PersonId }, faceDto);
            }
            return BadRequest();
        }
    }
}
