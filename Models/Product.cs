using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        [Display(Name ="Stock Quantity")]
        public int StockQuantity { get; set; }
        [Required]
        [Display(Name ="Unitary Amount")]
        public int UnitaryAmount { get; set; }
        [Required]
        public Level Level { get; set; }
        [Required]
        public byte[]? Image { get; set; }
        public ICollection<Request>? Requests { get; set; }
    }
    public enum Level
    {
        level0,level1,level2
    }
}
