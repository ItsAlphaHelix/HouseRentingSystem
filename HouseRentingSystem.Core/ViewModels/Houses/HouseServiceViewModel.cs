﻿namespace HouseRentingSystem.Core.ViewModels.Houses
{
    using HouseRentingSystem.Core.Contracts;
    using System.ComponentModel.DataAnnotations;
    public class HouseServiceViewModel : IHouseModel
	{
        public int Id { get; init; }

        public string Title { get; init; } = null!;

        public string Address { get; init; } = null!;

        [Display(Name = "Image URL")]
        public string ImageUrl { get; init; } = null!;

        [Display(Name = "Price per month")]
        public decimal PricePerMonth { get; init; }

        [Display(Name = "Is Rented")]
        public bool IsRented { get; init; }
    }
}
