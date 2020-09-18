using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesTest.DTOs
{
    public class FaceDto
    {
        public long Id { get; set; }
        public long PersonId { get; set; } // foreign key
        public IFormFile Picture { get; set; }
    }
}
