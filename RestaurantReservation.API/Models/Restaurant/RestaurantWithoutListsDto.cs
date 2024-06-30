namespace RestaurantReservation.API.Models.Restaurant
{
    public class RestaurantWithoutListsDto
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string OpeningHours { get; set; } = string.Empty;
    }
}
