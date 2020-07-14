using Business.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Task6.Tests.ReaderTests
{
    class ReaderModelComparer : IEqualityComparer<ReaderModel>
    {
        public bool Equals([AllowNull] ReaderModel x, [AllowNull] ReaderModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id 
                && string.Equals(x.Name, y.Name)
                && string.Equals(x.Email, y.Email)
                && string.Equals(x.Phone, y.Phone)
                && string.Equals(x.Address, y.Address);
        }

        public int GetHashCode([DisallowNull] ReaderModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
