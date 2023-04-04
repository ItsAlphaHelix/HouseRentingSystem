using HouseRentingSystem.Core.ViewModels.Agents;

namespace HouseRentingSystem.Core.ViewModels.Houses
{
    public class HouseDetailsModel : HouseServiceViewModel
    {
        public string Description { get; set; }

        public string Category { get; set; }

        public AgentServiceViewModel Agent { get; set; }
    }
}
