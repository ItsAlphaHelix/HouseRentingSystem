namespace HouseRentingSystem.Infrastructure.Data.Models
{
    using HouseRentingSystem.Infrastructure.Constants;
    using System.ComponentModel.DataAnnotations;
    public class Category
    {
        public Category()
        {
            this.Houses = new List<House>();
        }
        public int Id { get; init; }

        [Required]
        [MaxLength(ValidationConstants.CategoryNameMaxLength)]
        public string Name { get; set; }

        public IEnumerable<House> Houses  { get; init; }
    }
}
