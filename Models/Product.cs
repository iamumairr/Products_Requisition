#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name ="Stock Quantity")]
        public int StockQuantity { get; set; }
        [Required]
        [Display(Name ="Unitary Amount")]
        public int UnitaryAmount { get; set; }
        [Required]
        public Level Level { get; set; }
        public string Image { get; set; }

        [Display(Name = "Image")]
        [Required]
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }
        public ICollection<Request> Requests { get; set; }
    }
    public enum Level
    {
        Zero,One,Two
    }
}
