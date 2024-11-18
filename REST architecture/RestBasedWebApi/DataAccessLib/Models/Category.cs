using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAccessLib.Models;

public partial class Category
{
   public int CategoryId { get; set; }

   [Required(AllowEmptyStrings = false)]
   [DisplayFormat(ConvertEmptyStringToNull = false)]
   public string CategoryName { get; set; } = null!;

   public string? Description { get; set; }

   [JsonIgnore]
   public byte[]? Picture { get; set; }

   [JsonIgnore]
   public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
