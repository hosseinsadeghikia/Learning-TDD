using Moq;
using Shouldly;
using TicketingSolution.Core.DataServices;
using TicketingSolution.Core.Enums;
using TicketingSolution.Core.Handler;
using TicketingSolution.Core.Model;
using TicketingSolution.Domain.Domain;

namespace TicketingSolution.Core
{
    public class Ticket_Booking_Request_Handler_Test
    {
        private readonly TicketBookingRequestHandler _handler;
        private readonly TicketBookingRequest _request;
        private readonly Mock<ITicketBookingService> _ticketBookingServiceMock;
        private List<Ticket> _availableTicket;

        public Ticket_Booking_Request_Handler_Test()
        {
            //Arrange
            _request = new TicketBookingRequest
            {
                Name = "Test Name",
                Family = "Test Family",
                Email = "Test Email",
                Date = DateTime.Now
            };

            _availableTicket = new List<Ticket> { new Ticket() { Id = 1 } };
            _ticketBookingServiceMock = new Mock<ITicketBookingService>();
            _ticketBookingServiceMock.Setup(x =>
                    x.GetAvailableTickets(_request.Date)).Returns(_availableTicket);

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

            _ticketBookingServiceMock.Verify(x =>
                x.Save(It.IsAny<TicketBooking>()), Times.Once);

            savedBooking.ShouldNotBeNull();
            savedBooking.Name.ShouldBe(_request.Name);
            savedBooking.Family.ShouldBe(_request.Family);
            savedBooking.Email.ShouldBe(_request.Email);
            savedBooking.TicketId.ShouldBe(_availableTicket.First().Id);

        }

        [Fact]
        public void Should_Not_Save_Ticket_Booking_Request_If_None_Available()
        {
            _availableTicket.Clear();
            _handler.BookService(_request);
            _ticketBookingServiceMock.Verify(x =>
                x.Save(It.IsAny<TicketBooking>()), Times.Never);
        }

        [Theory]
        [InlineData(BookingResultFlag.Failure, false)]
        [InlineData(BookingResultFlag.Success, true)]
        public void Should_Return_SuccessOrContractFailure_Flag_In_Result(BookingResultFlag bookingSuccessFlag, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableTicket.Clear();
            }

            var result = _handler.BookService(_request);

            bookingSuccessFlag.ShouldBe(result.Flag);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(null, false)]
        public void Should_Return_TicketBookingId_In_Result(int? ticketBookingId, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableTicket.Clear();
            }
            else
            {
                _ticketBookingServiceMock.Setup(x =>
                        x.Save(It.IsAny<TicketBooking>()))
                    .Callback<TicketBooking>(booking =>
                    {
                        booking.Id = ticketBookingId.Value;
                    });
            }
        }
    }
}
