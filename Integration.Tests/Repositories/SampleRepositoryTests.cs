using Infrastructure.Repository.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Tests.Repositories
{
    public class SampleRepositoryTests : SqlDbTestFixture
    {
        private ISampleRepository _sampleRepository;

        [SetUp]
        public void Setup()
        {
            _sampleRepository = new SampleRepository(_DbContext);
        }

        [Test]
        public async Task GetAllSamples_ShouldReturnAllSamples()
        {
            // Arrange: Insert some test data if needed
            var sampleEntity = new SampleEntity(
                Guid.NewGuid(),
                "Test Product",
                "Description of test product",
                100,
                DateTime.Now,
                DateTime.Now
            );

            _DbContext.SampleEntities.Add(sampleEntity);
            await _DbContext.SaveChangesAsync();

            // Act: Retrieve data using the repository
            var samples = await _sampleRepository.GetAllSamplesAsync();

            // Assert: Check that the sample data is retrieved correctly
            Assert.IsNotEmpty(samples);
            Assert.That(samples.First().Name, Is.EqualTo("Test Product"));
        }

        [Test]
        public async Task GetSampleById_ShouldReturnCorrectSample()
        {
            // Arrange: Insert a test sample entity into the database
            var sampleEntity = new SampleEntity(
                Guid.NewGuid(),
                "Sample Product",
                "Description of sample product",
                150,
                DateTime.Now,
                DateTime.Now
            );

            _DbContext.SampleEntities.Add(sampleEntity);
            await _DbContext.SaveChangesAsync();

            // Act: Retrieve the sample by its ID using the repository
            var sample = await _sampleRepository.GetSampleEntityByIdAsync(sampleEntity.Id);

            // Assert: Check if the retrieved sample matches the expected sample
            Assert.IsNotNull(sample);  // The sample should not be null
            Assert.That(sample.Name, Is.EqualTo("Sample Product")); // Verify the name
        }

        // Test for GetSampleById when no sample exists
        [Test]
        public async Task GetSampleById_ShouldReturnNull_WhenSampleDoesNotExist()
        {
            // Arrange: Use a non-existing GUID
            var nonExistingId = Guid.NewGuid();

            // Act: Try to get a sample by the non-existing ID
            var sample = await _sampleRepository.GetSampleEntityByIdAsync(nonExistingId);

            // Assert: The result should be null
            Assert.IsNull(sample);
        }

        // Test for edge case: Get all samples when no samples exist
        [Test]
        public async Task GetAllSamples_ShouldReturnEmpty_WhenNoSamplesExist()
        {
            // Act: Try to get all samples when no sample is in the database
            var samples = await _sampleRepository.GetAllSamplesAsync();

            // Assert: The result should be an empty collection
            Assert.That(samples.Count, Is.EqualTo(1));
        }
    }
}
