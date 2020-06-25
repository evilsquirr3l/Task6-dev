using System.Collections.Generic;

namespace Business.Models
{
    public class ReaderModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public ICollection<int> CardsIds { get; set; }
    }
}