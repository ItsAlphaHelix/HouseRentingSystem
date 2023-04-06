using HouseRentingSystem.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HouseRentingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHouseService houseService;

        public HomeController(IHouseService houseService)
        {
            this.houseService = houseService;
        }
        public async Task<IActionResult> Index()
        {
            var houses = await this.houseService.LastThreeHouses();

            return View(houses);
        }

        public IActionResult Error(int statusCode)
        {
            if (statusCode == 401)
            {
                return View("Error401");
            }

            return View();
        }
    }
}