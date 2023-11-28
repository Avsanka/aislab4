using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace lab2server
{
    class PersonContext: DbContext
    {
        public PersonContext() : base("DbConnection") { }
        public DbSet<classPerson.Person> People { get; set; }
    }
}
