﻿namespace RestaurantReservation.API.Models.MenuItem
{
    public class MenuItemForUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int RestaurantId { get; set; }
    }
}