using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Infrastructure.Constants
{
    public static class ValidationConstants
    {
        public const int NameMaxLength = 30;

        public const int MaxTitleLength = 50; //min = 10

        public const int MaxAddressLength = 150; // min = 30

        public const int MaxDescriptionLength = 500; //min 50

        public const string MaxPricePerMonth = "2000"; 

        public const string MinPricePerMonth = "0";

        public const int MaxPhoneNumber = 15; // min = 7
    }
}
