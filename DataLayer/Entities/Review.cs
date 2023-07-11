using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
    }

}
