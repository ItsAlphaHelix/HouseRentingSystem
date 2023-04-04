using HouseRentingSystem.Core.ViewModels.Houses;

namespace HouseRentingSystem.Core.Contracts
{
	public interface IHouseService
	{
		Task<IEnumerable<HouseServiceViewModel>> LastThreeHouses();

		Task<IEnumerable<HouseCategoryViewModel>> AllCategories();

        Task<int> Create(HouseFormViewModel model, int agentId);

        Task<bool> CategoryExists(int categoryId);
    }
}
