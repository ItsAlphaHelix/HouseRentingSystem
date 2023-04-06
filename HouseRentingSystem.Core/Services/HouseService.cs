namespace HouseRentingSystem.Core.Services
{
	using HouseRentingSystem.Core.Contracts;
    using HouseRentingSystem.Core.Exceptions;
    using HouseRentingSystem.Core.ViewModels.Houses;
	using HouseRentingSystem.Infrastructure.Data.Models;
	using HouseRentingSystem.Infrastructure.Data.Repositories;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;
    using System;

	public class HouseService : IHouseService
	{
		private readonly IRepository repository;
		private readonly ILogger logger;
        private readonly IGuard guard;
        public HouseService(IRepository repository,
            ILogger<HouseService> logger,
            IGuard guard)
		{
            this.repository = repository;
			this.logger = logger;
            this.guard = guard;
		}

		public async Task<HouseQueryViewModel> All(
			string category = null,
			string searchTerm = null,
			HouseSorting sorting = HouseSorting.Newest,
			int currentPage = 1,
			int housesPerPage = 1)
		{
			var result = new HouseQueryViewModel();
			var houses = this.repository.AllReadonly<House>()
				.Where(x => x.IsActive == true);

			if (string.IsNullOrWhiteSpace(category) == false)
			{
                houses = houses
					.Where(x => x.Category.Name == category);
			}

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                houses = houses
                    .Where(h => EF.Functions.Like(h.Title.ToLower(), searchTerm) ||
                        EF.Functions.Like(h.Address.ToLower(), searchTerm) ||
                        EF.Functions.Like(h.Description.ToLower(), searchTerm));
            }

            houses = sorting switch
            {
                HouseSorting.Price => houses
                    .OrderBy(h => h.PricePerMonth),
                HouseSorting.NotRentedFirst => houses
                    .OrderBy(h => h.RenterId),
                _ => houses.OrderByDescending(h => h.Id)
            };

            result.Houses = await houses
                .Skip((currentPage - 1) * housesPerPage)
                .Take(housesPerPage)
                .Select(h => new HouseServiceViewModel()
                {
                    Address = h.Address,
                    Id = h.Id,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth,
                    Title = h.Title
                })
                .ToListAsync();

            result.TotalHousesCount = await houses.CountAsync();

            return result;
        }

		public async Task<IEnumerable<HouseCategoryViewModel>> AllCategories()
		{
			var categories = await this.repository.AllReadonly<Category>()
				.Select(x => new HouseCategoryViewModel()
				{
					Id = x.Id,
					Name = x.Name,
				})
				.ToListAsync();

			return categories;
		}

		public async Task<IEnumerable<string>> AllCategoriesName()
		{
            return await repository.AllReadonly<Category>()
               .Select(c => c.Name)
               .Distinct()
               .ToListAsync();
        }

		public async Task<IEnumerable<HouseServiceViewModel>> AllHousesByAgentId(int id)
		{
            return await repository.AllReadonly<House>()
               .Where(c => c.IsActive)
               .Where(c => c.AgentId == id)
               .Select(c => new HouseServiceViewModel()
               {
                   Address = c.Address,
                   Id = c.Id,
                   ImageUrl = c.ImageUrl,
                   IsRented = c.RenterId != null,
                   PricePerMonth = c.PricePerMonth,
                   Title = c.Title
               })
               .ToListAsync();
        }

		public async Task<IEnumerable<HouseServiceViewModel>> AllHousesByUserId(string userId)
		{
            return await repository.AllReadonly<House>()
               .Where(c => c.RenterId == userId)
               .Where(c => c.IsActive)
               .Select(c => new HouseServiceViewModel()
               {
                   Address = c.Address,
                   Id = c.Id,
                   ImageUrl = c.ImageUrl,
                   IsRented = c.RenterId != null,
                   PricePerMonth = c.PricePerMonth,
                   Title = c.Title
               })
               .ToListAsync();
        }

		public async Task<bool> CategoryExists(int categoryId)
		{
            return await repository.AllReadonly<Category>()
                .AnyAsync(c => c.Id == categoryId);
        }

		public async Task<int> Create(HouseFormViewModel model, int agentId)
		{
            var house = new House()
            {
                Address = model.Address,
                CategoryId = model.CategoryId,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                Title = model.Title,
                AgentId = agentId
            };

            try
            {
                await repository.AddAsync(house);
                await repository.SaveChangesAsync();
            }
			catch (Exception ex)
			{
                logger.LogError(nameof(Create), ex);
                throw new ApplicationException("Database failed to save info", ex);
            }

            return house.Id;
        }

        public async Task Delete(int houseId)
        {
            var house = await this.repository.GetByIdAsync<House>(houseId);
            house.IsActive = false;

            await this.repository.SaveChangesAsync();
        }

        public async Task EditHouse(int houseId, HouseFormViewModel houseModel)
        {
            var house = await repository.GetByIdAsync<House>(houseId);

            if (house == null)
            {
                throw new ArgumentException("The house can not be null!");
            }

            house.Title = houseModel.Title;
            house.Address = houseModel.Address;
            house.Description = houseModel.Description;
            house.ImageUrl = houseModel.ImageUrl;
            house.PricePerMonth = houseModel.PricePerMonth;
            house.CategoryId = houseModel.CategoryId;

            await this.repository.SaveChangesAsync();
        }

        public async Task<int> GetHouseCategoryId(int houseId)
        {
            return (await repository.GetByIdAsync<House>(houseId)).CategoryId;
        }

        public async Task<bool> HasAgentWithId(int houseId, string currentUserId)
        {
            bool result = false;

            var house = await this.repository.AllReadonly<House>()
                .Where(h => h.IsActive)
                .Where(h => h.Id == houseId)
                .Include(h => h.Agent)
                .FirstOrDefaultAsync();

            if (house?.Agent != null && house.Agent.UserId == currentUserId)
            {
                result = true;
            }

            return result;
        }

        public async Task<HouseDetailsModel> HouseDetailsById(int id)
        {
            return await repository.AllReadonly<House>()
               .Where(h => h.IsActive)
               .Where(h => h.Id == id)
               .Select(h => new HouseDetailsModel()
               {
                   Address = h.Address,
                   Category = h.Category.Name,
                   Description = h.Description,
                   Id = id,
                   ImageUrl = h.ImageUrl,
                   IsRented = h.RenterId != null,
                   PricePerMonth = h.PricePerMonth,
                   Title = h.Title,
                   Agent = new ViewModels.Agents.AgentServiceViewModel()
                   {
                       Email = h.Agent.User.Email,
                       PhoneNumber = h.Agent.PhoneNumber
                   }

               })
               .FirstAsync();
        }

        public async Task<bool> IsHouseExists(int id)
        {
            return await repository.AllReadonly<House>()
                .AnyAsync(h => h.Id == id && h.IsActive == true);
        }

        public async Task<bool> IsRented(int houseId)
        {
            return (await this.repository.GetByIdAsync<House>(houseId)).RenterId != null;
        }

        public async Task<bool> IsRentedByUserId(int houseId, string userId)
        {
            bool result = false;
            var house = await repository.AllReadonly<House>()
                .Where(h => h.IsActive)
                .Where(h => h.Id == houseId)
                .FirstOrDefaultAsync();

            if (house != null && house.RenterId == userId)
            {
                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<HouseServiceViewModel>> LastThreeHouses()
		{
			var lastThreeHouses = await this.repository.AllReadonly<House>()
				.Where(x => x.IsActive == true)
				.OrderByDescending(x => x.Id)
				.Select(x => new HouseServiceViewModel()
				{
					Id = x.Id,
					Title = x.Title,
					ImageUrl = x.ImageUrl
				})
				.Take(3)
				.ToListAsync();

			return lastThreeHouses;
		}

        public async Task Leave(int houseId)
        {
            var house = await repository.GetByIdAsync<House>(houseId);
            guard.AgainstNull(house, "House can not be found");
            house.RenterId = null;

            await this.repository.SaveChangesAsync();
        }

        public async Task Rent(int houseId, string userId)
        {
            var house = await this.repository.GetByIdAsync<House>(houseId);

            if (house != null && house.RenterId != null)
            {
                throw new ArgumentException("House is already rented");
            }

            guard.AgainstNull(house, "House can not be found");
            house.RenterId = userId;

            await repository.SaveChangesAsync();
        }
    }
}
