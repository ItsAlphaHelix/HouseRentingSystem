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

        public IActionResult All()
        {
            return View(new HouseQueryViewModel());
        }

        [Authorize]
        public IActionResult Mine()
        {
            return View(new HouseQueryViewModel());
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

            //redirect to details when its implemented
            return RedirectToAction(nameof(HomeController.Index), new { id = id, information = model.GetInformation() });
        }

        //[Authorize]
        //[HttpPost]

        //public IActionResult Add(HouseFormModel model)
        //{
        //    return RedirectToAction(nameof(Details), new { id = "1" });
        //}

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
