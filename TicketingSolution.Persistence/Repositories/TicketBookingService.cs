using TicketingSolution.Core.DataServices;
using TicketingSolution.Domain.Domain;

namespace TicketingSolution.Persistence.Repositories
{
    public class TicketBookingService : ITicketBookingService
    {
        private readonly TicketingSolutionDbContext _context;

        public TicketBookingService(TicketingSolutionDbContext context)
        {
            _context = context;
        }

        public void Save(TicketBooking ticketBooking)
        {
            _context.Add(ticketBooking);
            _context.SaveChanges();
        }

        public IEnumerable<Ticket> GetAvailableTickets(DateTime date)
        {
            var availableTickets = _context.Tickets.Where(x=> !x.TicketBookings
                    .Any(x=>x.Date ==date)).ToList();

            return availableTickets;
        }
    }
}
