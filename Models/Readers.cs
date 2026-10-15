namespace WebApplication1.Models
{
    public class Readers
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public  int Rating { get; set; }
        public string Review { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
