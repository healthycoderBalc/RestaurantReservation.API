namespace RestaurantReservation.API.Models.Authentication
{
    public class AuthenticationRequestBody
    {
        public string? UserName { get; set; }

        public string? LastName { get; set; }
        public int Password { get; set; }
    }
}
