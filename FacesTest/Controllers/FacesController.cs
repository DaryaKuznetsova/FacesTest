using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacesTest.Models;
using FacesTest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        //[HttpGet("{personId}")]
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
    }
}
