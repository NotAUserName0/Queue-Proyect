using Domain.Entities;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Config
{
    public static class DbSeeder
    {
        public static async Task InitializeDataAsync(AppDbContext dbContext)
        {
            var registros = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Fermin Simulated 1",
                    Age = 26,
                    Email = "fjimenez@gmail.com",
                    Password = "AQAAAAIAAYagAAAAEGkh66BBgjW+DFc0mbsTn+QJhN5C+Ottu7cOLbtLP3fxLWtSZR0/z3Jkk3B9YBUfsA==",
                    CretedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 2,
                    Name = "Fermin Simulated 2",
                    Age = 26,
                    Email = "fjimenez@gmail.com",
                    Password = "AQAAAAIAAYagAAAAEGkh66BBgjW+DFc0mbsTn+QJhN5C+Ottu7cOLbtLP3fxLWtSZR0/z3Jkk3B9YBUfsA==",
                    CretedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = 3,
                    Name = "Fermin Simulated 3",
                    Age = 26,
                    Email = "fjimenez@gmail.com",
                    Password = "AQAAAAIAAYagAAAAEGkh66BBgjW+DFc0mbsTn+QJhN5C+Ottu7cOLbtLP3fxLWtSZR0/z3Jkk3B9YBUfsA==",
                    CretedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
            };

            dbContext.Users.AddRange(registros);
            await dbContext.SaveChangesAsync();
        }
    }
}
