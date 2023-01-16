using Chat.Core.Application.Domain;
using Chat.Core.Application.Domain.ChatMessages.Assemblers;
using Chat.Core.Application.Domain.Enums;
using Chat.Core.Application.Dtos;

namespace Chat.Core.Application.Tests.Assemblers
{
    // Juan sends sends message,
    // Pepe sends message after 12 hours

    public class ChatMessageAssemblerTests
    {
        private readonly string ExpectedUser1 = "Juan";
        private readonly string ExpectedUser2 = "Pepe";
        private readonly string ExpectedMsg1 = "Hi";
        private readonly string ExpectedMsg2 = "Bye";
        private readonly DateTime ExpectedDate = new DateTime(2023, 01, 16, 10, 11, 12, DateTimeKind.Utc);

        private IList<ChatMessage> _messageEvents;

        public ChatMessageAssemblerTests()
        {

            _messageEvents = new List<ChatMessage>()
            {
                new ChatMessage(ExpectedDate, ExpectedUser1, EventType.SendMessage, ExpectedMsg1),
                new ChatMessage(ExpectedDate.AddHours(12), ExpectedUser2, EventType.SendMessage, ExpectedMsg2),
            };
        }

        [Fact]
        public void When_AssembleChatEventDtosFrom_WithChatMessageAccessEvents_AndSecondsView_ThenMapDtoToDefaultDisplayViewSuccesfully()
        {
            //Arrange
            var expectedDtos = new List<ChatEventDto> {
                new ChatEventDto("10:11:12 AM", $"{ExpectedUser1} comments: \"{ExpectedMsg1}\""),
                new ChatEventDto("10:11:12 PM", $"{ExpectedUser2} comments: \"{ExpectedMsg2}\""),
            };

            //Act
            var chatMessageAssembler = new ChatMessageAssembler();
            var actualChatEventDtos = chatMessageAssembler.AssembleChatEventDtosFrom(_messageEvents, TimeGranularity.Seconds).ToList();


            //Assert
            Assert.NotNull(actualChatEventDtos);
            Assert.Equal(2, actualChatEventDtos.Count());
            AssertDtoItems(expectedDtos, actualChatEventDtos);
        }

        [Fact]
        public void When_AssembleChatEventDtosFrom_WithChatMessageAccessEvents_AndMinutesView_ThenMapDtoToDefaultDisplayViewSuccesfully()
        {
            //Arrange
            var expectedDtos = new List<ChatEventDto> {
                new ChatEventDto("10:11 AM", $"{ExpectedUser1} comments: \"{ExpectedMsg1}\""),
                new ChatEventDto("10:11 PM", $"{ExpectedUser2} comments: \"{ExpectedMsg2}\""),
            };

            //Act
            var chatMessageAssembler = new ChatMessageAssembler();
            var actualChatEventDtos = chatMessageAssembler.AssembleChatEventDtosFrom(_messageEvents, TimeGranularity.Minutes).ToList();


            //Assert
            Assert.NotNull(actualChatEventDtos);
            Assert.Equal(2, actualChatEventDtos.Count());
            AssertDtoItems(expectedDtos, actualChatEventDtos);
        }

        [Fact]
        public void When_AssembleChatEventDtosFrom_WithChatMessageAccessEvents_AndHoursView_ThenMapDtoToDefaultDisplayViewSuccesfully()
        {
            //Arrange
            var expectedDtos = new List<ChatEventDto> {
                new ChatEventDto("10 AM", $"1 comment"),
                new ChatEventDto("10 PM", $"1 comment"),
            };

            //Act
            var chatMessageAssembler = new ChatMessageAssembler();
            var actualChatEventDtos = chatMessageAssembler.AssembleChatEventDtosFrom(_messageEvents, TimeGranularity.Hours).ToList();


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
                new ChatEventDto("1/16/23", "2 comments")
            };

            //Act
            var chatMessageAssembler = new ChatMessageAssembler();
            var actualChatEventDtos = chatMessageAssembler.AssembleChatEventDtosFrom(_messageEvents, TimeGranularity.Days).ToList();


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