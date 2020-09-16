using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesTest.Models
{
    public class Face
    {
        public long Id { get; set; }
        public byte[] Picture { get; set; }
        public long PersonId { get; set; } // foreign key
        public virtual Person Person { get; set; } // navigation property
    }
}
