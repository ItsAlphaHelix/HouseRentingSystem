using HouseRentingSystem.Infrastructure.Configuration;
using HouseRentingSystem.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Infrastructure.Data
{
    public class HouseRentingDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public HouseRentingDbContext(DbContextOptions<HouseRentingDbContext> options)
            :base(options)
        { 

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new AgentConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new HouseConfiguration());

            builder.Entity<House>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Houses)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<House>()
                .HasOne(x => x.Agent)
                .WithMany()
                .HasForeignKey(x => x.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

        public DbSet<Agent> Agents { get; init; }
        public DbSet<Category> Categories { get; init; }
        public DbSet<House> Houses { get; init; }

    }
}
