//using Infrastructure;
//using Microsoft.EntityFrameworkCore;

//namespace Integration.Tests
//{
//    public abstract class SqlDbTestFixture : ConfigurationTestFixture
//    {
//        protected DbContextOptions<SalesPlatformDbContext> DbContextOptions;
//        protected SalesPlatformDbContext DbContext;

//        [SetUp]
//        public void Setup()
//        {
//            DbContextOptions = new DbContextOptionsBuilder<SalesPlatformDbContext>()
//                .UseInMemoryDatabase("TestDatabase")
//                .Options;

//            DbContext = new SalesPlatformDbContext(DbContextOptions);
//            DbContext.Database.EnsureDeleted();
//            DbContext.Database.EnsureCreated();

//            ConfigureDatabase();
//            DoSetup();
//        }

//        [TearDown]
//        public void Teardown()
//        {
//            DbContext?.Dispose();
//            DoTeardown();
//        }

//        private void ConfigureDatabase()
//        {
//            // Add seed data or initial setup if needed
//        }

//        public virtual Task DoSetup() { return Task.CompletedTask; }
//        public virtual Task DoTeardown() { return Task.CompletedTask; }
//    }
//}
