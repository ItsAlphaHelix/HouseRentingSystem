namespace HouseRentingSystem.Infrastructure.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static HouseRentingSystem.Infrastructure.Constants.ValidationConstants;
    public class House
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxHouseTitleLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(MaxHouseAddressLength)]
        public string Address { get; set; }

        [Required]
        [MaxLength(MaxHouseDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Column(TypeName = "money")]
        [Precision(18, 2)]
        public decimal PricePerMonth { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [ForeignKey(nameof(Agent))]
        public int AgentId { get; set; }

        public Agent Agent { get; set; }

        public string? RenterId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}