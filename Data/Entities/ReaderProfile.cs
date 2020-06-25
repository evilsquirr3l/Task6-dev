namespace Data.Entities
{
    public class ReaderProfile
    {
        public Reader Reader { get; set; }
        
        public int ReaderId { get; set; }
        
        public string Phone { get; set; }

        public string Address { get; set; }
    }
}