namespace HouseRentingSystem.Core.ViewModels.Houses
{
    using System.ComponentModel.DataAnnotations;
    using static HouseRentingSystem.Infrastructure.Constants.ValidationConstants;
    using static HouseRentingSystem.Core.Constants.ErrorMessageConstants;
    using HouseRentingSystem.Core.Contracts;

    public class HouseFormViewModel : IHouseModel
    {
        public HouseFormViewModel()
        {
            this.HouseCategories = new List<HouseCategoryViewModel>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(MaxHouseTitleLength, MinimumLength = MinHouseTitleLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(MaxHouseAddressLength, MinimumLength = MinHouseAddressLength)]
        public string Address { get; set; }

        [Required]
        [StringLength(MaxHouseDescriptionLength, MinimumLength = MinHouseDescriptionLength)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Required]
        [Display(Name = "Price per month")]
        [Range(MinHousePricePerMonth, MaxHousePricePerMonth, ErrorMessage = PriceErrorMessage)]
        public decimal PricePerMonth { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<HouseCategoryViewModel> HouseCategories { get; set; }
    }
}
