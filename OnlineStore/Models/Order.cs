using OnlineStore.Areas.Identity.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Total price must be a positive value.")]
        public int TotalPrice { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
