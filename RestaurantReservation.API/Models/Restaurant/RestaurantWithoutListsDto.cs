namespace RestaurantReservation.API.Models.Restaurant
{
    /// <summary>
    /// Restaurant
    /// </summary>
    public class RestaurantWithoutListsDto
    {
        /// <summary>
        /// id of restaurant
        /// </summary>
        public int RestaurantId { get; set; }

        /// <summary>
        /// Name of restaurant
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Address of restaurant
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Phone number of restaurant
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Opening Hours of restaurant
        /// </summary>
        public string OpeningHours { get; set; } = string.Empty;
    }
}
