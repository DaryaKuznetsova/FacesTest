using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class FacesController : ControllerBase
    {
        private readonly FacesContext _context;
        private readonly FaceService _faceService;

        public FacesController(FacesContext context)
        {
            _context = context;
            _faceService = new FaceService(context);

            if (!_context.Faces.Any())
            {
                _context.Faces.Add(new Face { PersonId = 1 });
                _context.Faces.Add(new Face { PersonId = 1 });
                _context.Faces.Add(new Face { PersonId = 2 });
                _context.SaveChanges();
            }
        }

        //GET: api/Person/id/face
        public async Task<ActionResult<IEnumerable<Face>>> GetFace(long personId)
        {
            return await _faceService.GetFaces(personId);
        }

        //GET: api/person/id/face/id
        [HttpGet("{faceId}")]
        public async Task<ActionResult<Face>> GetFace(long personId, long faceId)
        {
            var face = await _faceService.GetFace(personId, faceId);
            if (face == null) return NotFound();
            else return face;
        }

        // api/person/id/face
        [HttpPost]
        public async Task<ActionResult<Face>> PostFace(long personId, Face face)
        {
            await _faceService.PostFace(personId, face);
            return CreatedAtAction(nameof(GetFace), new { personId = face.PersonId }, face);
        }

        [HttpPut("{faceId}")]
        public async Task<IActionResult> PutPerson(long personId, long faceId, Face face)
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
        public async Task<ActionResult<Face>> DeletePerson(long personId, long faceId)
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
    }
}
