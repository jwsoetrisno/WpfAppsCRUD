using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApps.Models;

[Table("Products")]
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SKU { get; set; }   // unique code
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}
