using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacesTest.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MiddleName { get; set; }
        public virtual ICollection<Face> Faces { get; set; }
    }
}
