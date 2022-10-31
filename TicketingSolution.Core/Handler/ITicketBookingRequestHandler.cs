using TicketingSolution.Core.Model;

namespace TicketingSolution.Core.Handler
{
    public interface ITicketBookingRequestHandler
    {
        ServiceBookingResult BookService(TicketBookingRequest bookingRequest);
    }
}
