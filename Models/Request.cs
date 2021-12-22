using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }
        [Display(Name ="Request Date")]
        public DateTime RequestDate { get; set; }
        [Display(Name ="Product")]
        public int ProductId { get; set; }
        [Display(Name ="Quantity")]
        public int QuantityRequest { get; set; }
        [Display(Name ="Total Amount")]
        public int TotalAmount { get; set; }
        public string? Justification { get; set; }
        public string? Note { get; set; }
        public Status RequestStatus { get; set; }
        public Product? Product { get; set; }
    }
    public enum Status
    {
        PENDING, APPROVED, REJECTED
    }
}
