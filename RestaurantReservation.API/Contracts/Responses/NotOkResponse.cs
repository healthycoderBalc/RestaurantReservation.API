namespace RestaurantReservation.API.Contracts.Responses
{
    /// <summary>
    ///  Error response
    /// </summary>
    public class NotOkResponse
    {
        /// <summary>
        ///  type
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        ///  Error Title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Error status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Error trace ID
        /// </summary>
        public string TraceId { get; set; } = string.Empty;


    }
}
