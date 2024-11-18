using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAccessLib.Models;

public partial class Product
{
   public int ProductId { get; set; }

   [Required(AllowEmptyStrings = false)]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string ProductName { get; set; } = null!;

   public int? SupplierId { get; set; }

   public int? CategoryId { get; set; }

   public string? QuantityPerUnit { get; set; }

   public decimal? UnitPrice { get; set; }

   public short? UnitsInStock { get; set; }

   public short? UnitsOnOrder { get; set; }

   public short? ReorderLevel { get; set; }

   public bool Discontinued { get; set; }

   [JsonIgnore]
   public virtual Category? Category { get; set; }
}
