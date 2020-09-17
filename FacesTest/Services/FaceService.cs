using FacesTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FacesTest.Services
{
    public class FaceService
    {
        private readonly FacesContext _context;

        public FaceService(FacesContext context)
        {
            _context = context;
        }

        public async Task<List<Face>> GetFaces(long personId)
        {
            return await _context.Faces.Where(e => e.PersonId == personId).ToListAsync();
        }

        public async Task<Face> GetFace(long personId, long faceId)
        {
            var face = await _context.Faces.FindAsync(faceId);
            if (face.PersonId == personId) return face;
            else return null; 
        }
    }
}
