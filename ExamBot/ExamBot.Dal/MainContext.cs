using ExamBot.Dal.Entities;
using ExamBot.Dal.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamBot.Dal;

public class MainContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<FillData> FillDatas { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "Data Source=NOMOZOV\\SQLEXPRESS01;User ID=sa;Password=1;Initial Catalog=MyBot;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        
    }
}
