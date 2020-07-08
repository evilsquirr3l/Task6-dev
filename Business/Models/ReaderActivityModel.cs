using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class ReaderActivityModel
    {
        public int ReaderId { get; set; }

        public string ReaderName { get; set; }

        public int BooksCount { get; set; }
    }
}
