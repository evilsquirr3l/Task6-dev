﻿using Business.Models;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Task6
{
    internal class CardEqualityComparer : IEqualityComparer<Card>
    {
        public bool Equals([AllowNull] Card x, [AllowNull] Card y)
        {
            return x.Id == y.Id
                && x.Created == y.Created
                && x.ReaderId == y.ReaderId;
        }

        public int GetHashCode([DisallowNull] Card obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class CardModelEqualityComparer : IEqualityComparer<CardModel>
    {
        public bool Equals([AllowNull] CardModel x, [AllowNull] CardModel y)
        {
            return x.Id == y.Id
                && x.Created == y.Created
                && x.ReaderId == y.ReaderId;
        }

        public int GetHashCode([DisallowNull] CardModel obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class HistoryEqualityComparer : IEqualityComparer<History>
    {
        public bool Equals([AllowNull] History x, [AllowNull] History y)
        {
            return x.Id == y.Id
                && x.BookId == y.BookId
                && x.CardId == y.CardId
                && x.TakeDate == y.TakeDate
                && x.ReturnDate == y.ReturnDate;
        }

        public int GetHashCode([DisallowNull] History obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class BookEqualityComparer : IEqualityComparer<Book>
    {
        public bool Equals([AllowNull] Book x, [AllowNull] Book y)
        {
            return x.Id == y.Id
                && x.Year == y.Year
                && x.Author == y.Author;
        }

        public int GetHashCode([DisallowNull] Book obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class BookModelEqualityComparer : IEqualityComparer<BookModel>
    {
        public bool Equals([AllowNull] BookModel x, [AllowNull] BookModel y)
        {
            return x.Id == y.Id
                && x.Year == y.Year
                && x.Author == y.Author;
        }

        public int GetHashCode([DisallowNull] BookModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
