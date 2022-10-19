using TicketingSolution.Core.DataServices;
using TicketingSolution.Domain.Domain;

namespace TicketingSolution.Persistence.Repositories
{
    public class TicketBookingService : ITicketBookingService
    {
        public void Save(TicketBooking ticketBooking)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Ticket> GetAvailableTickets(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
