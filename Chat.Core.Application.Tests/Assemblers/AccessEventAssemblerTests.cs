using Chat.Core.Application.Domain;
using Chat.Core.Application.Domain.AccessEvents.Assemblers;
using Chat.Core.Application.Domain.Enums;
using Chat.Core.Application.Dtos;

namespace Chat.Core.Application.Tests.Assemblers
{
    // Juan enters the chat,
    // Pepe enters the chat after 10 minutes
    // David enters the chat after 10 hours

    public class AccessEventAssemblerTests
    {
        private readonly string ExpectedUser1 = "Juan";
        private readonly string ExpectedUser2 = "Pepe";
        private readonly string ExpectedUser3 = "David";
        private readonly DateTime ExpectedDate = new DateTime(2023, 01, 16, 10, 20, 30, DateTimeKind.Utc);

        private IList<AccessRoomEvent> _accessEvents;

        public AccessEventAssemblerTests()
        {

            _accessEvents = new List<AccessRoomEvent>()
            {
                new AccessRoomEvent(ExpectedDate, ExpectedUser1, EventType.EnterRoom),
                new AccessRoomEvent(ExpectedDate.AddMinutes(10), ExpectedUser2, EventType.EnterRoom),
                new AccessRoomEvent(ExpectedDate.AddHours(12), ExpectedUser3, EventType.EnterRoom),
            };
        }

        [Fact]
        public void When_AssembleChatEventDtosFrom_WithAddUserAccessEvents_AndSecondsView_ThenMapDtoToDefaultDisplayViewSuccesfully()
        {
            //Arrange
            var expectedDtos = new List<ChatEventDto> {
                new ChatEventDto("10:20:30 AM", $"{ExpectedUser1} enters the room"),
                new ChatEventDto("10:30:30 AM", $"{ExpectedUser2} enters the room"),
                new ChatEventDto("10:20:30 PM", $"{ExpectedUser3} enters the room"),
            };

            //Act
            var accessEventAssembler = new AccessEventAssembler();
            var actualChatEventDtos = accessEventAssembler.AssembleChatEventDtosFrom(_accessEvents, TimeGranularity.Seconds).ToList();


            //Assert
            Assert.NotNull(actualChatEventDtos);
            Assert.Equal(3, actualChatEventDtos.Count());
            AssertDtoItems(expectedDtos, actualChatEventDtos);
        }

        [Fact]
        public void When_AssembleChatEventDtosFrom_WithAddUserAccessEvents_AndMinutesView_ThenMapDtoToDefaultDisplayViewSuccesfully()
        {
            //Arrange
            var expectedDtos = new List<ChatEventDto> {
                new ChatEventDto("10:20 AM", $"{ExpectedUser1} enters the room"),
                new ChatEventDto("10:30 AM", $"{ExpectedUser2} enters the room"),
                new ChatEventDto("10:20 PM", $"{ExpectedUser3} enters the room"),
            };

            //Act
            var accessEventAssembler = new AccessEventAssembler();
            var actualChatEventDtos = accessEventAssembler.AssembleChatEventDtosFrom(_accessEvents, TimeGranularity.Minutes).ToList();


            //Assert
            Assert.NotNull(actualChatEventDtos);
            Assert.Equal(3, actualChatEventDtos.Count());
            AssertDtoItems(expectedDtos, actualChatEventDtos);
        }

        [Fact]
        public void When_AssembleChatEventDtosFrom_WithAddUserAccessEvents_AndHoursView_ThenMapDtoToAgreggatedDisplayViewSuccesfully()
        {
            //Arrange
            var expectedDtos = new List<ChatEventDto> {
                new ChatEventDto("10 AM", "2 people entered"),
                new ChatEventDto("10 PM", "1 person entered"),
            };

            //Act
            var accessEventAssembler = new AccessEventAssembler();
            var actualChatEventDtos = accessEventAssembler.AssembleChatEventDtosFrom(_accessEvents, TimeGranularity.Hours).ToList();


            //Assert
            Assert.NotNull(actualChatEventDtos);
            Assert.Equal(2, actualChatEventDtos.Count());
            AssertDtoItems(expectedDtos, actualChatEventDtos);
        }

        [Fact]
        public void When_AssembleChatEventDtosFrom_WithAddUserAccessEvents_AndDaysView_ThenMapDtoToAgreggatedDisplayViewSuccesfully()
        {
            //Arrange
            var expectedDtos = new List<ChatEventDto> {
                new ChatEventDto("1/16/23", "3 people entered")
            };

            //Act
            var accessEventAssembler = new AccessEventAssembler();
            var actualChatEventDtos = accessEventAssembler.AssembleChatEventDtosFrom(_accessEvents, TimeGranularity.Days).ToList();


            //Assert
            Assert.NotNull(actualChatEventDtos);
            Assert.Single(actualChatEventDtos);
            AssertDtoItems(expectedDtos, actualChatEventDtos);
        }

        private void AssertDtoItems(IList<ChatEventDto> expectedDtos, IList<ChatEventDto> actualChatEventDtos)
        {
            for (int i = 0; i < expectedDtos.Count(); i++)
            {
                Assert.Equal(expectedDtos[i].TimeScope, actualChatEventDtos[i].TimeScope);
                Assert.Equal(expectedDtos[i].Message, actualChatEventDtos[i].Message);
            }
        }

    }
}