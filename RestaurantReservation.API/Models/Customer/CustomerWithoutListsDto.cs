namespace RestaurantReservation.API.Models.Customer
{
    /// <summary>
    /// A customer without associated lists
    /// </summary>
    public class CustomerWithoutListsDto
    {
        /// <summary>
        /// Id of customer
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        ///  First name of customer
        /// </summary>
        public string FirstName { get; set; } = string.Empty;
        
        /// <summary>
        /// Last name of customer
        /// </summary>
        public string LastName { get; set; } = string.Empty;
        
        /// <summary>
        /// Email address of customer
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        ///  Phone number of customer
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
