namespace RestaurantReservation.API.Models.Order
{
    public class OrderForUpdateDto
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int ReservationId { get; set; }
        public int EmployeeId { get; set; }
    }
}
