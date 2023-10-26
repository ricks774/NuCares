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

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Nutritionist> Nutritionists { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<DailyCourseMenu> DailyCourseMenus { get; set; }
        public virtual DbSet<DailyLog> DailyLogs { get; set; }
        public virtual DbSet<DailyMealTime> DailyMealTimes { get; set; }
        public virtual DbSet<FavoriteList> FavoriteLists { get; set; }
        public virtual DbSet<BodyInfo> BodyInfos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}