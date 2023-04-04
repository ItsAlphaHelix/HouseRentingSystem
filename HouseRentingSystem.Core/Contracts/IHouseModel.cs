using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Contracts
{
    public interface IHouseModel
    {
        public string Title { get; }

        public string Address { get; }
    }
}
