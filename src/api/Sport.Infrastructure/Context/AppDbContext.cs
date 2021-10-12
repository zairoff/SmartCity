using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sport.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<SportEvent> SportEvents { get; set; }

        public DbSet<SportType> SportTypes { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Trainer> Trainers { get; set; }

        public DbSet<Trainee> Trainees { get; set; }

        public DbSet<Vacancy> Vacancies { get; set; }

        public DbSet<Applicant> Applicants { get; set; }

        public DbSet<Pocket> Pockets { get; set; }

        public DbSet<SportGroup> SportGroups { get; set; }

        public DbSet<EventParticipant> EventParticipants { get; set; }

        public DbSet<EventSubscriber> EventSubscribers { get; set; }

        public DbSet<EventWinner> EventWinners { get; set; }

        public DbSet<TrainerGroup> TrainerGroups { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(@Directory.GetCurrentDirectory() + "/../Sport.API/appsettings.json")
                    .Build();
                var builder = new DbContextOptionsBuilder<AppDbContext>();
                var connectionString = configuration.GetConnectionString("DatabaseConnection");
                builder.UseSqlServer(connectionString);
                return new AppDbContext(builder.Options);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            Seed(builder);
        }

        private static void Seed(ModelBuilder builder)
        {
            builder.Entity<TrainerGroup>()
                .HasOne(g => g.Group)
                .WithMany()
                .HasForeignKey(g => g.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TrainerGroup>()
                .HasOne(g => g.Trainer)
                .WithMany()
                .HasForeignKey(g => g.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SportType>().HasData(
                new SportType
                {
                    Id = 1,
                    Name = "Football"
                },
                new SportType
                {
                    Id = 2,
                    Name = "Backetball"
                },
                new SportType
                {
                    Id = 3,
                    Name = "Tennis"
                });

            builder.Entity<SportGroup>().HasData(
                new SportGroup
                {
                    Id = 1,
                    SportTypeId = 1,
                    Name = "Future Makers"
                },
                new SportGroup
                {
                    Id = 2,
                    SportTypeId = 1,
                    Name = "Young Boyz"
                },
                new SportGroup
                {
                    Id = 3,
                    SportTypeId = 2,
                    Name = "Champions"
                });

            builder.Entity<Pocket>().HasData(
                new Pocket
                {
                    Id = 1,
                    Name = "A",
                    PricePerMonth = 10
                },
                new Pocket
                {
                    Id = 2,
                    Name = "B",
                    PricePerMonth = 120
                },
                new Pocket
                {
                    Id = 3,
                    Name = "C",
                    PricePerMonth = 123
                });

            builder.Entity<Position>().HasData(
                new Position
                {
                    Id = 1,
                    Name = "Director"
                },
                new Position
                {
                    Id = 2,
                    Name = "Accountant"
                },
                new Position
                {
                    Id = 3,
                    Name = "Trainer"
                });
        }
    }
}
