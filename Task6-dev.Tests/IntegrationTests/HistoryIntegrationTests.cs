using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Business.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Task6.IntegrationTests
{
    [TestFixture]
    public class HistoryIntegrationTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private ReaderModelEqualityComparer _comparer;
        private string requestUri = "api/history/";

        [OneTimeSetUp]
        public void Init()
        {
            _comparer = new ReaderModelEqualityComparer();
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }
        //2020-07-24
        [Test, Order(0)]
        public async Task HistoryController_GetMostPopularBooks()
        {
            var httpResponse = await _client.GetAsync(requestUri + "popularBooks?bookCount=2");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<BookModel>>(stringResponse).ToList();
        }

        [Test, Order(0)]
        public async Task HistoryController_GetReadersWhoTookTheMostBooks()
        {
            var httpResponse = await _client.GetAsync($"{requestUri}biggestReaders?bookCount=1&" +
                                                      $"firstDate={DateTime.Now.AddDays(-1).ToShortDateString()}&" +
                                                      $"lastDate={DateTime.Now.ToShortDateString()}");

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<ReaderModel>>(stringResponse).ToList();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}