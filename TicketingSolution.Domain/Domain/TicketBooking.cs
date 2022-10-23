using TicketingSolution.Domain.Domain.BaseModels;

namespace TicketingSolution.Domain.Domain
{
    public class TicketBooking : ServiceBookingBase
    {
        public int Id { get; set; }


        public Ticket Ticket { get; set; }

        public int TicketId { get; set; }
    }
}