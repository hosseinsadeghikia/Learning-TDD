using Moq;
using Shouldly;
using TicketingSolution.Core.DataServices;
using TicketingSolution.Core.Domain;
using TicketingSolution.Core.Handler;
using TicketingSolution.Core.Model;

namespace TicketingSolution.Core
{
    public class Ticket_Booking_Request_Handler_Test
    {
        private readonly TicketBookingRequestHandler _handler;
        private readonly TicketBookingRequest _request;
        private readonly Mock<ITicketBookingService> _ticketBookingServiceMock;

        public Ticket_Booking_Request_Handler_Test()
        {
            //Arrange
            _request = new TicketBookingRequest
            {
                Name = "Test Name",
                Family = "Test Family",
                Email = "Test Email"
            };

            _ticketBookingServiceMock = new Mock<ITicketBookingService>();

            _handler = new TicketBookingRequestHandler(_ticketBookingServiceMock.Object);
        }

        [Fact]
        public void Should_Return_Ticket_Booking_Response_With_Request_Values()
        {

            //Act
            ServiceBookingResult result = _handler.BookService(_request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name, _request.Name);
            Assert.Equal(result.Family, _request.Family);
            Assert.Equal(result.Email, _request.Email);

            //Assert By Shouldly
            result.ShouldNotBeNull();
            result.Name.ShouldBe(_request.Name);
            result.Family.ShouldBe(_request.Family);
            result.Email.ShouldBe(_request.Email);
        }

        [Fact]
        public void Should_Throw_Exception_For_Null_Request()
        {
            var exception = Should.Throw<ArgumentNullException>((() => _handler.BookService(null)));
            exception.ParamName.ShouldBe("bookingRequest");

            //Assert.Throws<ArgumentNullException>(() => _handler.BookService(null));
        }

        [Fact]
        public void Should_Save_Ticket_Booking_Request()
        {
            TicketBooking savedBooking = null;
            _ticketBookingServiceMock.Setup(x =>
                x.Save(It.IsAny<TicketBooking>()))
                .Callback<TicketBooking>(booking =>
                {
                    savedBooking = booking;
                });

            _handler.BookService(_request);

            _ticketBookingServiceMock.Verify(x=>
                x.Save(It.IsAny<TicketBooking>()),Times.Once);

            savedBooking.ShouldNotBeNull();
            savedBooking.Name.ShouldBe(_request.Name);
            savedBooking.Family.ShouldBe(_request.Family);
            savedBooking.Email.ShouldBe(_request.Email);
        }
    }
}
