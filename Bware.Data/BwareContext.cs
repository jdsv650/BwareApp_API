using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bware.Data
{
    public class BwareContext :DbContext
    {
        public BwareContext() : base("SQLSERVER2012") { }

        public DbSet<Model.Bridge> Bridges { get; set; }
        public DbSet<Model.Message> Messages { get; set; }

    }
}
