﻿using FacesTest.Models;
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
            var face = await _context.Faces.Where(e => e.Id == faceId && e.PersonId == personId).ToListAsync();
            if (face.Count!=0) return face[0]; else return null;
        }

        public async Task PostFace(long personId, Face face)
        {
            face.PersonId = personId;
            _context.Faces.Add(face);
            await _context.SaveChangesAsync();
        }

        public async Task PutFace(Face face)
        {
            _context.Entry(face).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public bool FaceExists(long personId, long id)
        {
            return _context.Faces.Any(e => e.Id == id&& e.PersonId==personId);
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

        public async Task<Person> FindPerson(byte[] face)
        {
            var res = await _context.Faces.Where(x => x.Picture == face).ToListAsync();
            if (res.Count != 0)
            {
                return await _context.People.FindAsync(res[0].PersonId);
            }
            else return null;
        }
    }
}
