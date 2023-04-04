using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Infrastructure.Data.Models;
using HouseRentingSystem.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Core.Services
{
    public class AgentsService : IAgentService
    {
        private readonly IRepository repository;

        public AgentsService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Create(string userId, string phoneNumber)
        {
            var agent = new Agent()
            {
                UserId = userId,
                PhoneNumber = phoneNumber
            };

            await repository.AddAsync(agent);
            await repository.SaveChangesAsync();
        }

        public async Task<bool> ExistsById(string userId)
        {
            return await this.repository.All<Agent>().AnyAsync(x => x.UserId == userId);
        }

        public async Task<int> GetAgentById(string userId)
        {
            return (await repository.All<Agent>().FirstOrDefaultAsync(x => x.UserId == userId))?.Id ?? 0;
        }

        public async Task<int> GetAgentId(string userId)
        {
            return (await repository.AllReadonly<Agent>()
                .FirstOrDefaultAsync(a => a.UserId == userId))?.Id ?? 0;
        }

        public async Task<bool> UserHasRents(string userId)
        {
            return await repository.All<House>()
                .AnyAsync(h => h.RenterId == userId);
        }

        public async Task<bool> UserWithPhoneNumberExists(string phoneNumber)
        {
            return await repository.All<Agent>()
                .AnyAsync(a => a.PhoneNumber == phoneNumber);
        }
    }
}
