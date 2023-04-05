using HouseRentingSystem.Core.ViewModels.Houses;

namespace HouseRentingSystem.Core.Contracts
{
	public interface IHouseService
	{
		Task<IEnumerable<HouseServiceViewModel>> LastThreeHouses();

		Task<IEnumerable<HouseCategoryViewModel>> AllCategories();

        Task<int> Create(HouseFormViewModel model, int agentId);

        Task<bool> CategoryExists(int categoryId);

		Task<HouseQueryViewModel> All(string category = null,
			string searchTerm = null,
			HouseSorting sorting = HouseSorting.Newest,
			int currentPage = 1,
			int housesPerPage = 1);

		Task<IEnumerable<string>> AllCategoriesName();

        Task<IEnumerable<HouseServiceViewModel>> AllHousesByAgentId(int id);

        Task<IEnumerable<HouseServiceViewModel>> AllHousesByUserId(string userId);

		Task<bool> IsHouseExists(int id);

		Task<HouseDetailsModel> HouseDetailsById(int id);
    }
}
