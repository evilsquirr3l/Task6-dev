using System.Linq;
using AutoMapper;
using Business;
using Business.Interfaces;
using Business.Services;
using Data;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi;

namespace Task6.IntegrationTests
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //Remove the app's ApplicationDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<LibraryDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                //TODO: Pool vs simple db context
                services.AddDbContextPool<LibraryDbContext>(options =>
                {
                    UnitTestHelper.GetUnitTestDbOptions();
                    options.UseInMemoryDatabase("squirr3l");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                services.AddScoped<IBookRepository, BookRepository>();
                services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                services.AddScoped<IUnitOfWork, UnitOfWork>();

                var mapper = new MapperConfiguration(c => c.AddProfile(new AutomapperProfile())).CreateMapper();
                services.AddSingleton(mapper);
                services.AddTransient<IBooksService, BooksService>();

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<LibraryDbContext>();

                    UnitTestHelper.SeedData(context);
                }
            });
        }
    }
}