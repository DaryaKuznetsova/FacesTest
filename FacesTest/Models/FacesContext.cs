using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FacesTest.Models
{
    public class FacesContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Face> Faces { get; set; }
        public FacesContext(DbContextOptions<FacesContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }


    }
}
