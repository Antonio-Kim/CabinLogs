using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabinLogsApiTests.Fakes;

public class ApplicationDbContextFake : ApplicationDbContext
{
    public ApplicationDbContextFake() : base(new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: $"CabinLog-{Guid.NewGuid()}").Options)
    { }
}
