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

        //public virtual DbSet<DailyCourseMenu> DailyCourseMenus { get; set; }
        //public virtual DbSet<DailyLog> DailyLogs { get; set; }
        //public virtual DbSet<DailyMealTime> DailyMealTimes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}