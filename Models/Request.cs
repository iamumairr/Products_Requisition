#nullable disable
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Justification { get; set; }
        public string Note { get; set; }
        [Display(Name ="Request Status")]
        public Status RequestStatus { get; set; }
        public Product Product { get; set; }
        [ForeignKey("ApplicationUser")]
        [Display(Name ="User")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
    public enum Status
    {
        PENDING, PARTIAL,APPROVED, REJECTED
    }
}
