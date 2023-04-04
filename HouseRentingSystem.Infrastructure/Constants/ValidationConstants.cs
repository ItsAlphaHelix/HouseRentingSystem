using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Infrastructure.Constants
{
    public static class ValidationConstants
    {
        public const int CategoryNameMaxLength = 30;

        public const int MaxHouseTitleLength = 50;
        public const int MinHouseTitleLength = 10;

        public const int MaxHouseAddressLength = 150;
        public const int MinHouseAddressLength = 30; 

        public const int MaxHouseDescriptionLength = 500;
        public const int MinHouseDescriptionLength = 50; 

        public const int MaxHousePricePerMonth = 2000; 
        public const int MinHousePricePerMonth = 0;

        public const int MaxAgentPhoneNumber = 15;
        public const int MinAgentPhoneNumber = 7; 
    }
}
