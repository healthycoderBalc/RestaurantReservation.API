namespace RestaurantReservation.API.Contracts.Responses
{
    /// <summary>
    /// Token comming from Authorization
    /// </summary>
    public class AuthorizedResponse
    {
        /// <summary>
        /// A valid token
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
