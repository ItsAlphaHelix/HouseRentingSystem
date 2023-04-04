namespace HouseRentingSystem.Core.ViewModels.Agents
{
    using System.ComponentModel.DataAnnotations;
    using static HouseRentingSystem.Infrastructure.Constants.ValidationConstants;
    public class BecomeAgentViewModel
    {
        [Required]
        [StringLength(MaxAgentPhoneNumber, MinimumLength = MinAgentPhoneNumber)]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
