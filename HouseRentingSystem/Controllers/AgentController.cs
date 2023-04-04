namespace HouseRentingSystem.Web.Controllers
{
	using HouseRentingSystem.Core.Contracts;
	using HouseRentingSystem.Core.ViewModels.Agents;
	using HouseRentingSystem.Web.Extensions;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	public class AgentController : Controller
	{
		private readonly IAgentService agentService;

		public AgentController(IAgentService agentService)
		{
			this.agentService = agentService;
		}

		[Authorize]
		public async Task<IActionResult> Become()
		{
			if (await this.agentService.ExistsById(this.User.Id()))
			{
				return BadRequest(); //Todo make good page maybe u can use toastNotyF
			}
			
			var agent = new BecomeAgentViewModel();

			return View(agent);
		}

        [Authorize]
		[HttpPost]
        public async Task<IActionResult> Become(BecomeAgentViewModel agent)
        {
			var userId = this.User.Id();

            if (!ModelState.IsValid)
            {
                return View(agent);
            }

            if (await this.agentService.ExistsById(userId))
			{
				return BadRequest(); //TODO: 
			}

			if (await this.agentService.UserWithPhoneNumberExists(agent.PhoneNumber))
			{
				ModelState.AddModelError(nameof(agent.PhoneNumber), "Phone number already exists. Enter another oen.");
			}

			if (await this.agentService.UserHasRents(userId))
			{
				ModelState.AddModelError("Error", "You should have no rents to become an agent!");
			}

			await this.agentService.Create(userId, agent.PhoneNumber);

            return RedirectToAction(nameof(HouseController.All), "House");
        }
    }
}
