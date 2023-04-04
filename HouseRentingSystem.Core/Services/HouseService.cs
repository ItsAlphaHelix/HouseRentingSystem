namespace HouseRentingSystem.Core.Services
{
	using HouseRentingSystem.Core.Contracts;
	using HouseRentingSystem.Core.ViewModels.Houses;
	using HouseRentingSystem.Infrastructure.Data.Models;
	using HouseRentingSystem.Infrastructure.Data.Repositories;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	public class HouseService : IHouseService
	{
		private readonly IRepository repository;
		private readonly ILogger<HouseService> logger;

		public HouseService(IRepository repository, ILogger<HouseService> logger)
		{
            this.repository = repository;
			this.logger = logger;
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
	}
}
