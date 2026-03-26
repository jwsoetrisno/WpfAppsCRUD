namespace BizManager.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public double Amount { get; set; }
        public string Method { get; set; } // Cash, Card
        public DateTime Date { get; set; }
    }
}
