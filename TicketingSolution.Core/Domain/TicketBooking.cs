using TicketingSolution.Core.Model;

namespace TicketingSolution.Core.Domain
{
    public class TicketBooking : ServiceBookingBase
    {
        public static int Id { get; set; }
        public int TicketId { get; set; }
    }
}