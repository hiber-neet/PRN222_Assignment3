using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ECommerceContext(
                serviceProvider.GetRequiredService<DbContextOptions<ECommerceContext>>()))
            {
                // Ensure database is created
                context.Database.EnsureCreated();

                // Look for any members
                if (context.Members.Any())
                {
                    return; // DB has been seeded
                }

                // Seed Members
                var members = new Member[]
                {
                    new Member
                    {
                        Email = "admin@estore.com",
                        CompanyName = "Admin Company",
                        City = "Admin City",
                        Country = "USA",
                        Password = "admin123"
                    },
                    new Member
                    {
                        Email = "john.doe@example.com",
                        CompanyName = "Doe Enterprises",
                        City = "New York",
                        Country = "USA",
                        Password = "password123"
                    },
                    new Member
                    {
                        Email = "jane.smith@example.com",
                        CompanyName = "Smith & Co",
                        City = "London",
                        Country = "UK",
                        Password = "password123"
                    },
                    new Member
                    {
                        Email = "david.wang@example.com",
                        CompanyName = "Wang Trading",
                        City = "Shanghai",
                        Country = "China",
                        Password = "password123"
                    },
                    new Member
                    {
                        Email = "maria.rodriguez@example.com",
                        CompanyName = "Rodriguez Solutions",
                        City = "Madrid",
                        Country = "Spain",
                        Password = "password123"
                    }
                };

                context.Members.AddRange(members);
                context.SaveChanges();
            }
        }
    }
} 