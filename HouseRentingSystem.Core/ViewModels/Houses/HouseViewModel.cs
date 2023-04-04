using HouseRentingSystem.Core.Contracts;

namespace HouseRentingSystem.Core.ViewModels.Houses
{
    public class HouseViewModel : IHouseModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Address { get; set; }
    }
}
