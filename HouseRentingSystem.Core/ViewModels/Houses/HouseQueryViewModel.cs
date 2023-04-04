namespace HouseRentingSystem.Core.ViewModels.Houses
{
    public class HouseQueryViewModel
    {
        public HouseQueryViewModel()
        {
            this.Houses = new List<HouseServiceViewModel>();
        }
        public int TotalHousesCount { get; set; }

        public IEnumerable<HouseServiceViewModel> Houses { get; set; }
    }
}
