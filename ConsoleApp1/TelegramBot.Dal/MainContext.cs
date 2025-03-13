using ChatBot.Dal.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Dal.Entities;

namespace TelegramBot.Dal;

public class MainContext : DbContext
{

    public DbSet<BotUser> Users { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "Data Source=localhost\\SQLEXPRESS;User ID=sa;Password=1;Initial Catalog=telegramBot;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BotUserConfiguration());

    }
}
