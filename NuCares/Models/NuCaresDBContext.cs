using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace NuCares.Models
{
    public partial class NuCaresDBContext : DbContext
    {
        public NuCaresDBContext()
            : base("name=NuCaresDB")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
