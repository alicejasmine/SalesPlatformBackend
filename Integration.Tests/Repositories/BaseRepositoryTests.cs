//using Domain;
//using Infrastructure;
//using Integration.Tests;
//using Microsoft.EntityFrameworkCore;
//using NUnit.Framework;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Infrastructure.Tests.Repositories
//{
//    public class BaseRepositoryTests : SqlDbTestFixture
//    {
//        private BaseTestRepository _baseTestRepository = null!;

//        [SetUp]
//        public void SetupRepository()
//        {
//            _baseTestRepository = new BaseTestRepository(SalesPlatformDbContext);
//        }

//        [Test]
//        public async Task Upsert_Entity_ShouldAdd_WhenNotExist()
//        {
//            var model = new BaseTestModel
//            {
//                Id = Guid.NewGuid(),
//                Name = "Test Model"
//            };

//            var result = await _baseTestRepository.UpsertAsync(model);

//            Assert.That(result, Is.Not.Null);
//            Assert.That(result.Id, Is.EqualTo(model.Id));
//            Assert.That(result.Name, Is.EqualTo(model.Name));
//            Assert.That(_context.BaseTestEntities?.Count(), Is.EqualTo(1));
//        }

//        [Test]
//        public async Task Get_Entity_ShouldExist()
//        {
//            var entity = new BaseTestEntity { Id = Guid.NewGuid(), Name = "Existing Entity" };
//            _context.BaseTestEntities?.Add(entity);
//            await _context.SaveChangesAsync();

//            var model = await _baseTestRepository.GetByIdAsync(entity.Id);

//            Assert.That(model, Is.Not.Null);
//            Assert.That(model!.Id, Is.EqualTo(entity.Id));
//            Assert.That(model.Name, Is.EqualTo(entity.Name));
//        }

//        [Test]
//        public async Task Delete_Entity_Success()
//        {
//            var entity = new BaseTestEntity { Id = Guid.NewGuid(), Name = "Entity to Delete" };
//            _context.BaseTestEntities?.Add(entity);
//            await _context.SaveChangesAsync();

//            await _baseTestRepository.DeleteAsync(entity.Id);

//            Assert.That(_context.BaseTestEntities?.Any(e => e.Id == entity.Id), Is.False);
//        }

//        [Test]
//        public async Task Upsert_Entity_ShouldUpdate_WhenExists()
//        {
//            var entity = new BaseTestEntity { Id = Guid.NewGuid(), Name = "Original Name" };
//            _context.BaseTestEntities?.Add(entity);
//            await _context.SaveChangesAsync();

//            var model = new BaseTestModel
//            {
//                Id = entity.Id,
//                Name = "Updated Name"
//            };

//            var result = await _baseTestRepository.UpsertAsync(model);

//            Assert.That(result.Name, Is.EqualTo("Updated Name"));
//            Assert.That(_context.BaseTestEntities?.First().Name, Is.EqualTo("Updated Name"));
//        }
//    }

//    public class SalesPlatformDbContext : DbContext
//    {
//        public DbSet<BaseTestEntity> BaseTestEntities { get; set; } = null!;

//        public SalesPlatformDbContext(DbContextOptions<SalesPlatformDbContext> options) : base(options) { }
//    }
//}
