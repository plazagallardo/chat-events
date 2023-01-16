using Chat.Core.Application.Domain;
using Chat.Core.Application.Domain.Enums;
using Chat.Core.Application.Domain.HighFives.Assemblers;
using Chat.Core.Application.Dtos;

namespace Chat.Core.Application.Tests.Assemblers
{
    // Juan sends high five to Tomas,
    // Juan sends high five to David after 1 second,
    // Juan sends high five to Marc after 2 seconds,
    // Pepe sends high five to David after 10 minutes
    // Pepe sends high five to Marc after 12 hours

    public class ChatHighFiveAssemblerTests
    {
        private readonly string ExpectedUser1 = "Juan";
        private readonly string ExpectedUser2 = "Pepe";
        private readonly string ExpectedHighFiveRecipient1 = "Tomas";
        private readonly string ExpectedHighFiveRecipient2 = "David";
        private readonly string ExpectedHighFiveRecipient3 = "Marc";
        private readonly DateTime ExpectedDate = new DateTime(2023, 01, 16, 10, 15, 30, DateTimeKind.Utc);

        private IList<ChatHighFive> _highFiveEvents;

        public ChatHighFiveAssemblerTests()
        {

            _highFiveEvents = new List<ChatHighFive>()
            {
                new ChatHighFive(ExpectedDate, ExpectedUser1, ExpectedHighFiveRecipient1, EventType.SendHiFive),
                new ChatHighFive(ExpectedDate.AddSeconds(1), ExpectedUser1, ExpectedHighFiveRecipient2, EventType.SendHiFive),
                new ChatHighFive(ExpectedDate.AddSeconds(2), ExpectedUser1, ExpectedHighFiveRecipient3, EventType.SendHiFive),
                new ChatHighFive(ExpectedDate.AddMinutes(10), ExpectedUser2, ExpectedHighFiveRecipient2, EventType.SendHiFive),
                new ChatHighFive(ExpectedDate.AddHours(12), ExpectedUser2, ExpectedHighFiveRecipient3, EventType.SendHiFive),
            };
        }

        [Fact]
        public void When_AssembleChatEventDtosFrom_WithChatHighFiveAccessEvents_AndSecondsView_ThenMapDtoToDefaultDisplayViewSuccesfully()
        {
            //Arrange
            var expectedDtos = new List<ChatEventDto> {
                new ChatEventDto("10:15:30 AM", $"{ExpectedUser1} high-fives {ExpectedHighFiveRecipient1}"),
                new ChatEventDto("10:15:31 AM", $"{ExpectedUser1} high-fives {ExpectedHighFiveRecipient2}"),
                new ChatEventDto("10:15:32 AM", $"{ExpectedUser1} high-fives {ExpectedHighFiveRecipient3}"),
                new ChatEventDto("10:25:30 AM", $"{ExpectedUser2} high-fives {ExpectedHighFiveRecipient2}"),
                new ChatEventDto("10:15:30 PM", $"{ExpectedUser2} high-fives {ExpectedHighFiveRecipient3}"),
            };

            //Act
            var chatHighFiveAssembler = new ChatHighFiveAssembler();
            var actualChatEventDtos = chatHighFiveAssembler.AssembleChatEventDtosFrom(_highFiveEvents, TimeGranularity.Seconds).ToList();


            //Assert
            Assert.NotNull(actualChatEventDtos);
            Assert.Equal(5, actualChatEventDtos.Count());
            AssertDtoItems(expectedDtos, actualChatEventDtos);
        }

        [Fact]
        public void When_AssembleChatEventDtosFrom_WithChatHighFiveAccessEvents_AndMinutesView_ThenMapDtoToDefaultDisplayViewSuccesfully()
        {
            //Arrange
            var expectedDtos = new List<ChatEventDto> {
                new ChatEventDto("10:15 AM", $"{ExpectedUser1} high-fives {ExpectedHighFiveRecipient1}"),
                new ChatEventDto("10:15 AM", $"{ExpectedUser1} high-fives {ExpectedHighFiveRecipient2}"),
                new ChatEventDto("10:15 AM", $"{ExpectedUser1} high-fives {ExpectedHighFiveRecipient3}"),
                new ChatEventDto("10:25 AM", $"{ExpectedUser2} high-fives {ExpectedHighFiveRecipient2}"),
                new ChatEventDto("10:15 PM", $"{ExpectedUser2} high-fives {ExpectedHighFiveRecipient3}"),
            };

            //Act
            var chatHighFiveAssembler = new ChatHighFiveAssembler();
            var actualChatEventDtos = chatHighFiveAssembler.AssembleChatEventDtosFrom(_highFiveEvents, TimeGranularity.Minutes).ToList();


            //Assert
            Assert.NotNull(actualChatEventDtos);
            Assert.Equal(5, actualChatEventDtos.Count());
            AssertDtoItems(expectedDtos, actualChatEventDtos);
        }

        [Fact]
        public void When_AssembleChatEventDtosFrom_WithChatHighFiveAccessEvents_AndHoursView_ThenMapDtoToDefaultDisplayViewSuccesfully()
        {
            //Arrange
            var expectedDtos = new List<ChatEventDto> {
                new ChatEventDto("10 AM", $"2 people high-fived 3 other people"),
                new ChatEventDto("10 PM", $"1 person high-fived 1 other person"),
            };

            //Act
            var chatHighFiveAssembler = new ChatHighFiveAssembler();
            var actualChatEventDtos = chatHighFiveAssembler.AssembleChatEventDtosFrom(_highFiveEvents, TimeGranularity.Hours).ToList();


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
                new ChatEventDto("1/16/23", "2 people high-fived 3 other people")
            };

            //Act
            var chatHighFiveAssembler = new ChatHighFiveAssembler();
            var actualChatEventDtos = chatHighFiveAssembler.AssembleChatEventDtosFrom(_highFiveEvents, TimeGranularity.Days).ToList();


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