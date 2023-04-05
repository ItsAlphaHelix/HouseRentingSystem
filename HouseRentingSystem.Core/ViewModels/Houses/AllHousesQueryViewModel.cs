using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.ViewModels.Houses
{
    public class AllHousesQueryViewModel
    {
        public const int HousesPerPage = 3;

        public string? Category { get; set; }

        public string? SearchTerm { get; set; }

        public HouseSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalHousesCount { get; set; }

        public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<HouseServiceViewModel> Houses { get; set; } = Enumerable.Empty<HouseServiceViewModel>();
    }
}
