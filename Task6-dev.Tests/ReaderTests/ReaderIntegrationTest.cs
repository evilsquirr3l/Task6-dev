using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi;
using Data;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Business.Models;

namespace Task6.Tests.ReaderTests
{
    [TestFixture]
    public class ReaderIntegrationTest
    {
        private HttpClient _client;
        private string requestUri = "api/reader";

        [OneTimeSetUp]
        public void Init()
        {
            _client = new CustomWebApplicationFactory().CreateClient();
        }

        [Test]
        public async Task Get()
        {
            //act
            var httpResponse = await _client.GetAsync(requestUri);

            // Must be successful.
            httpResponse.EnsureSuccessStatusCode();

            // Deserialize and examine results.
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var readers = JsonConvert.DeserializeObject<IEnumerable<ReaderModel>>(stringResponse);
            Assert.AreEqual(2, readers.Count());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
        }
    }

    internal class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<LibraryDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context (AppDbContext) using an in-memory database for testing.
                services.AddDbContextPool<LibraryDbContext>(options =>
                {
                    options.UseInMemoryDatabase("IntegrationTest");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                //services.AddScoped<IReaderRepository, ReaderRepository>();
                //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                //services.AddScoped<IUnitOfWork, UnitOfWork>();

                //var mapper = new MapperConfiguration(c => c.AddProfile(new AutomapperProfile())).CreateMapper();
                //services.AddSingleton(mapper);
                //services.AddTransient<IReaderService, ReaderService>();

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<LibraryDbContext>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with some specific test data.
                        UnitTestHelper.SeedData(db);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred seeding the database with test messages. Error: {ex.Message}");
                    }
                }
            });
        }
    }
    
}
