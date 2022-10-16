using TicketingSolution.Core.DataServices;
using TicketingSolution.Core.Domain;
using TicketingSolution.Core.Enums;
using TicketingSolution.Core.Model;

namespace TicketingSolution.Core.Handler
{
    public class TicketBookingRequestHandler
    {
        private readonly ITicketBookingService _ticketBookingService;

        public TicketBookingRequestHandler(ITicketBookingService ticketBookingService)
        {
            _ticketBookingService = ticketBookingService;
        }

        public ServiceBookingResult BookService(TicketBookingRequest? bookingRequest)
        {
            if (bookingRequest is null)
            {
                throw new ArgumentNullException(nameof(bookingRequest));
            }

            var availableTickets = _ticketBookingService.GetAvailableTickets(bookingRequest.Date);
            var result = CreateTicketBookingObject<ServiceBookingResult>(bookingRequest);

            if (availableTickets.Any())
            {
                var ticket = availableTickets.First();
                var ticketBooking = CreateTicketBookingObject<TicketBooking>(bookingRequest);
                ticketBooking.TicketId = ticket.Id;
                _ticketBookingService.Save(ticketBooking);
                result.TicketBookingId = ticketBooking.TicketId;
                result.Flag = BookingResultFlag.Success;
            }
            else
            {
                result.Flag = BookingResultFlag.Failure;
            }


            return result;
        }

        private static TTicketBooking CreateTicketBookingObject<TTicketBooking>(TicketBookingRequest bookingRequest)
            where TTicketBooking : ServiceBookingBase, new()
        {
            return new TTicketBooking
            {
                Name = bookingRequest.Name,
                Family = bookingRequest.Family,
                Email = bookingRequest.Email,
            };
        }
    }
}



