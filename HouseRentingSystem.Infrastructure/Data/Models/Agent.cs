namespace HouseRentingSystem.Infrastructure.Data.Models
{
    using HouseRentingSystem.Infrastructure.Constants;
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Agent
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MaxPhoneNumber)]
        public string PhoneNumber { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }
    }
}