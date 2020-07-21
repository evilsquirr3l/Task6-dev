using System;
using System.Collections.Generic;
using Business.Models;
using Data.Entities;

namespace Business.Interfaces
{
    public interface IHistoryService
    {
        IEnumerable<BookModel> GetMostPopularBooks(int bookCount);
        
        IEnumerable<ReaderActivityModel> GetReadersWhoTookTheMostBooks(int readersCount, DateTime firstDate, DateTime lastDate);
    }
}