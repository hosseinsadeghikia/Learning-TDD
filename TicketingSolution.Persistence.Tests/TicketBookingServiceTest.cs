using Microsoft.EntityFrameworkCore;
using TicketingSolution.Domain.Domain;
using TicketingSolution.Persistence.Repositories;
using Xunit;

namespace TicketingSolution.Persistence.Tests
{
    public class TicketBookingServiceTest
    {
        [Fact]
        public void Should_Return_Available_Services()
        {
            //Arrange

            var date = new DateTime(2022, 05, 31);

            var dbOptions = new DbContextOptionsBuilder<TicketingSolutionDbContext>()
                .UseInMemoryDatabase("AvailableTicketTest",x=>x.EnableNullChecks(false))
                .Options;

            using var context = new TicketingSolutionDbContext(options: dbOptions);
            context.Add(new Ticket { Id = 1, Name = "Ticket 1" });
            context.Add(new Ticket { Id = 2, Name = "Ticket 2" });
            context.Add(new Ticket { Id = 3, Name = "Ticket 3" });

            //context.Add(new TicketBooking { TicketId = 1, Name = "T1", Family = "T1",Email = "T1@t1.com", Date = date});
            //context.Add(new TicketBooking { TicketId = 2, Name = "T2", Family = "T2", Email = "T2@t2.com", Date = date.AddDays(-1)});

            context.Add(new TicketBooking { TicketId = 1, Date = date });
            context.Add(new TicketBooking { TicketId = 2, Date = date.AddDays(-1) });

            context.SaveChanges();

            var ticketBookingService = new TicketBookingService(context);

            //Act
            var availableServices = ticketBookingService.GetAvailableTickets(date);

            //Assert
            Assert.Equal(2,availableServices.Count());
            Assert.Contains(availableServices, x => x.Id == 2);
            Assert.Contains(availableServices, x => x.Id == 3);
        }

        [Fact]
        public void Should_Save_Ticket_Booking()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<TicketingSolutionDbContext>()
                .UseInMemoryDatabase("ShouldSaveTest", x => x.EnableNullChecks(false))
                .Options;

            var ticketBooking = new TicketBooking
            {
                TicketId = 1,
                Date = new DateTime(2022, 06, 01)
            };


            //Act
            using var context = new TicketingSolutionDbContext(options: dbOptions);
            var ticketBookingService = new TicketBookingService(context);

            ticketBookingService.Save(ticketBooking);

            //Assert
            var bookings = context.TicketBookings.ToList();
            var booking = Assert.Single(bookings);

            Assert.Equal(ticketBooking.TicketId, booking.TicketId);
            Assert.Equal(ticketBooking.Date, booking.Date);

        }
    }
}
