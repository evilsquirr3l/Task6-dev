using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Reader : BaseEntity
    {
        public string Name { get; set; }

        public Card Card { get; set; }
    }
}
