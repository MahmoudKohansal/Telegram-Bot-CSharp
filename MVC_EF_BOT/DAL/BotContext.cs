using MVC_EF_BOT.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace MVC_EF_BOT.DAL
{
    public class BotContext:DbContext
    {
        public BotContext(): base()
        {
        }
        public DbSet<BotUser> BotUsers { get; set; }
        public DbSet<BotJoke> BotJokes { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Configurations.Add(new System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<BotUser>());
        }

    }
}