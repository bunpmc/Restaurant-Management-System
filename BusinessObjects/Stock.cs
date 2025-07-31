using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("Stock")]
    public partial class Stock
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ItemName { get; set; } = "";

        [StringLength(50)]
        public string Category { get; set; } = "";

        [Required]
        public int Quantity { get; set; }

        [StringLength(20)]
        public string Unit { get; set; } = "";

        [Column(TypeName = "decimal(10,2)")]
        public decimal? UnitPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalValue { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [StringLength(50)]
        public string Supplier { get; set; } = "";

        public int? MinimumStock { get; set; }

        public DateTime? LastUpdated { get; set; }

        [StringLength(200)]
        public string Notes { get; set; } = "";

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        // Computed property for stock status
        [NotMapped]
        public string StockStatus
        {
            get
            {
                if (MinimumStock.HasValue && Quantity <= MinimumStock.Value)
                    return "Low Stock";
                else if (ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.Now.AddDays(7))
                    return "Expiring Soon";
                else if (Quantity == 0)
                    return "Out of Stock";
                else
                    return "In Stock";
            }
        }

        // Computed property for stock status color
        [NotMapped]
        public string StatusColor
        {
            get
            {
                return StockStatus switch
                {
                    "Out of Stock" => "#D32F2F",
                    "Low Stock" => "#FF9800",
                    "Expiring Soon" => "#FF5722",
                    "In Stock" => "#4CAF50",
                    _ => "#757575"
                };
            }
        }
    }
}
