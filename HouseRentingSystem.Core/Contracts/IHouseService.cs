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

		Task EditHouse(int houseId, HouseFormViewModel houseModel);

		Task<bool> HasAgentWithId(int houseId, string currentUserId);

		Task<int> GetHouseCategoryId(int houseId);

		Task Delete(int houseId);

		Task<bool> IsRented(int houseId);

		Task<bool> IsRentedByUserId(int houseId, string userId);

		Task Rent(int houseId, string userId);

		Task Leave(int houseId);
    }
}
