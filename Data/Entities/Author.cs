using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }

        public Author()
        {
            Books = new List<Book>();
        }
    }
}
