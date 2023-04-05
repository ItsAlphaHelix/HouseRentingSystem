using HouseRentingSystem.Controllers;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Extenstions;
using HouseRentingSystem.Core.ViewModels.Houses;
using HouseRentingSystem.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Web.Controllers
{
    public class HouseController : Controller
    {
        private readonly IAgentService agentService;
        private readonly IHouseService houseService;

        public HouseController(IAgentService agentService, IHouseService houseService)
        {
            this.agentService = agentService;
            this.houseService = houseService;
        }

        public async Task<IActionResult> All([FromQuery] AllHousesQueryViewModel query)
        {
            var result = await houseService.All(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllHousesQueryViewModel.HousesPerPage);

            query.TotalHousesCount = result.TotalHousesCount;
            query.Categories = await houseService.AllCategoriesName();
            query.Houses = result.Houses;

            return View(query);
        }

        [Authorize]
        public async Task<IActionResult> Mine()
        {

            IEnumerable<HouseServiceViewModel> myHhouses;
            var userId = User.Id();

            if (await agentService.ExistsById(userId))
            {
                int agentId = await agentService.GetAgentId(userId);
                myHhouses = await houseService.AllHousesByAgentId(agentId);
            }
            else
            {
                myHhouses = await houseService.AllHousesByUserId(userId);
            }

            return View(myHhouses);
        }


        [Authorize]
        public async Task<IActionResult> Add()
        {
            if ((await this.agentService.ExistsById(this.User.Id())) == false)
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }

            var house = new HouseFormViewModel()
            {
                HouseCategories = await this.houseService.AllCategories()
            };

            return View(house);
        }

        [HttpPost]
        public async Task<IActionResult> Add(HouseFormViewModel model)
        {
            if ((await agentService.ExistsById(User.Id())) == false)
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }

            if ((await houseService.CategoryExists(model.CategoryId)) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exists");
            }

            if (!ModelState.IsValid)
            {
                model.HouseCategories = await houseService.AllCategories();

                return View(model);
            }

            int agentId = await agentService.GetAgentId(User.Id());

            int id = await houseService.Create(model, agentId);

            return RedirectToAction(nameof(Details), new { id = id, information = model.GetInformation() });
        }

        public async Task<IActionResult> Details(int id)
        {
            if ((await houseService.IsHouseExists(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            var model = await houseService.HouseDetailsById(id);

            if (await this.houseService.IsHouseExists(id) == false)
            {
                TempData["ErrorMessage"] = "Don't touch my slug!";

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [Authorize]
        public IActionResult Edit()
        {
            return View();
        }

        //[Authorize]
        //[HttpPost]
        //public IActionResult Edit(int id, HouseFormViewModel house)
        //{
        //    return RedirectToAction(nameof(Details), new {id = "1"});
        //}

        [Authorize]
        [HttpPost]
        public IActionResult Delete(HouseDetailsViewModel house)
        {
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Rent(int id)
        {
            return RedirectToAction(nameof(Mine));
        }
    }
}
