using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using WebApi;

namespace Task6.IntegrationTests
{
    [TestFixture]
    public class BooksIntegrationTests
    {
        private HttpClient _client;
        
        [OneTimeSetUp]
        public void Init()
        {
            _client = new CustomWebApplicationFactory().CreateClient();
        }
        
        [Test]
        public async Task GetAll()
        {
            var httpResponse = await _client.GetAsync("api/books");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<IEnumerable<Book>>(stringResponse);
            
            Assert.AreEqual(1, books.Count());
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
        }
    }
}