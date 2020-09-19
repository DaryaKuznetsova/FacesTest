using FacesTest.DTOs;
using FacesTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        private static readonly Expression<Func<Face, FaceDto>> AsFaceDto =
        x => new FaceDto
        {
            Id = x.Id,
            PersonId = x.PersonId
        };

        private static FaceDto FaceDto(Face face)
        {
            return new FaceDto
            {
                Id = face.Id,
                PersonId = face.PersonId
            };
        }

        public async Task<List<FaceDto>> GetFaces(long personId)
        {
            return await _context.Faces.Where(e => e.PersonId == personId).Select(AsFaceDto).ToListAsync();
        }

        public async Task<FaceDto> GetFace(long personId, long faceId)
        {
            // Both the personId and faceId must describe the correct data
            var face = await _context.Faces.Where(e => e.Id == faceId && e.PersonId == personId).Select(AsFaceDto).ToListAsync();
            if (face.Count != 0) return face[0]; else return null;
        }

        public async Task<FaceDto> PostFace(long personId, Face face)
        {
            face.PersonId = personId;
            _context.Faces.Add(face);
            await _context.SaveChangesAsync();
            return FaceDto(face);
        }

        public async Task PutFace(FaceDto faceDto)
        {
            var face = await _context.Faces.FindAsync(faceDto.Id);
            face.PersonId = faceDto.PersonId;
            _context.Entry(face).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public bool FaceExists(long personId, long id)
        {
            return _context.Faces.Any(e => e.Id == id && e.PersonId == personId);
        }

        public async Task<Face> DeleteFace(long id)
        {
            var face = await _context.Faces.FindAsync(id);
            try
            {
                _context.Faces.Remove(face);
                await _context.SaveChangesAsync();
            }
            catch
            {

            }
            return face;
        }


    }
}
