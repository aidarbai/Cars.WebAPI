using Cars.BLL.Helpers;
using Cars.DAL.DbContext;
using Cars.DAL.Models;
using Cars.DAL.Repositories;
using Cars.FACADE.CarFacade;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace Cars.Tests
{
    public class InMemoryContextFixture : IDisposable
    {
        public CarDbContext Context { get; }

        private readonly UserManager<ApplicationUser> _userManager;
        
        private ILogger<CarRepository> _loggerForCarRepository;
        
        public CarFacade CarFacade { get; }

        public IDistributedCache DistributedCache { get; }

        public InMemoryContextFixture()
        {
            DbContextOptions<CarDbContext> options = new DbContextOptionsBuilder<CarDbContext>()
                .UseInMemoryDatabase(databaseName: "CarMockDatabase")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            Context = ArrangeTestHelper.SeedDb(new CarDbContext(options));
            _userManager = ArrangeTestHelper.GetMockUserManager().Object;
            _loggerForCarRepository = ArrangeTestHelper.GetLogger<CarRepository>();

            DistributedCache = ArrangeTestHelper.GetDistributedCache();

            CarFacade = ArrangeTestHelper.GetCarFacade(
                new CarRepository(
                    Context,
                    _userManager,
                    _loggerForCarRepository),
                Context);

            AutoMapper.Mapper.Reset();
            MappingHelper.AutoMapperInit();
        }

        public void Dispose()
        {
            //Context.Dispose();
            GC.SuppressFinalize(this);
        }

        [CollectionDefinition("Fixture collection")]
        public class DatabaseCollection : ICollectionFixture<InMemoryContextFixture>
        {
        }
    }
}
