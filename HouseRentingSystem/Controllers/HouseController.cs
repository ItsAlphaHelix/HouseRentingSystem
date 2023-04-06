using HouseRentingSystem.Controllers;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Extenstions;
using HouseRentingSystem.Core.ViewModels.Houses;
using HouseRentingSystem.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HouseRentingSystem.Web.Controllers
{
    public class HouseController : Controller
    {
        private readonly IAgentService agentService;
        private readonly IHouseService houseService;
        private readonly ILogger logger;

        public HouseController(IAgentService agentService,
            IHouseService houseService,
            ILogger<HouseController> logger)
        {
            this.agentService = agentService;
            this.houseService = houseService;
            this.logger = logger;
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
                //TODO:
                return BadRequest();
            }

            var model = await houseService.HouseDetailsById(id);

            if (await this.houseService.IsHouseExists(id) == false)
            {
                TempData["ErrorMessage"] = "Don't touch my slug!";

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (await this.houseService.IsHouseExists(id) == false)
            {
                //TODO:
                return BadRequest();
            }

            if (await this.houseService.HasAgentWithId(id, this.User.Id()) == false)
            {
                logger.LogInformation("User with id {0} attempted to open other agent house", User.Id());

                return Unauthorized();
            }

            var house = await houseService.HouseDetailsById(id);
            var categoryId = await houseService.GetHouseCategoryId(id);

            var model = new HouseFormViewModel()
            {
                Id = id,
                Address = house.Address,
                CategoryId = categoryId,
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                Title = house.Title,
                HouseCategories = await houseService.AllCategories()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HouseFormViewModel house)
        {
            if (id != house.Id)
            {
                return BadRequest();
            }

            if ((await this.houseService.IsHouseExists(house.Id)) == false)
            {
                ModelState.AddModelError("", "House does not exist");
                house.HouseCategories = await houseService.AllCategories();

                return View(house);
            }

            if ((await houseService.HasAgentWithId(house.Id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if ((await houseService.CategoryExists(house.CategoryId)) == false)
            {
                ModelState.AddModelError(nameof(house.CategoryId), "Category does not exist");
                house.HouseCategories = await houseService.AllCategories();

                return View(house);
            }

            if (ModelState.IsValid == false)
            {
                house.HouseCategories = await houseService.AllCategories();

                return View(house);
            }

            await houseService.EditHouse(house.Id, house);

            return RedirectToAction(nameof(Details), new { id = house.Id});
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await this.houseService.IsHouseExists(id)) == false)
            {
                //TODO:
                return BadRequest();
            }

            if ((await houseService.HasAgentWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            var house = await houseService.HouseDetailsById(id);
            var model = new HouseDetailsViewModel()
            {
                Address = house.Address,
                ImageUrl = house.ImageUrl,
                Title = house.Title
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, HouseViewModel house)
        {
            if ((await this.houseService.IsHouseExists(id)) == false)
            {
                //TODO:
                return BadRequest();
            }

            if ((await houseService.HasAgentWithId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await houseService.Delete(id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            if ((await this.houseService.IsHouseExists(id)) == false)
            {
                return BadRequest();
            }

            if (await agentService.ExistsById(User.Id()))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (await houseService.IsRented(id))
            {
                return BadRequest();
            }

            await houseService.Rent(id, User.Id());

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]

        public async Task<IActionResult> Leave(int id)
        {
            if ((await this.houseService.IsHouseExists(id)) == false ||
               (await houseService.IsRented(id)) == false)
            {
                return RedirectToAction(nameof(All));
            }

            if ((await this.houseService.IsRentedByUserId(id, User.Id())) == false)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await this.houseService.Leave(id);

            return RedirectToAction(nameof(Mine));
        }
    }
}
