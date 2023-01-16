using Chat.Core.Application.Domain;
using Chat.Core.Application.Domain.Base.Assemblers;
using Chat.Core.Application.Domain.Enums;
using Chat.Core.Application.Dtos;
using Chat.Core.Application.Infrastructure.Persistence;
using Moq;

namespace Chat.Core.Application.Tests.Assemblers
{
    public class ChatEventServiceTests
    {
        private readonly Mock<IRepository<AccessRoomEvent>> _mockAccessEventRepository;
        private readonly Mock<IRepository<ChatMessage>> _mockChatMessageRepository;
        private readonly Mock<IRepository<ChatHighFive>> _mockChatHighFiveRepository;
        private readonly Mock<IChatEventAssembler<AccessRoomEvent>> _mockAccessEventAssembler;
        private readonly Mock<IChatEventAssembler<ChatMessage>> _mockChatMessageAssembler;
        private readonly Mock<IChatEventAssembler<ChatHighFive>> _mockChatHighFiveAssembler;

        private readonly DateTime ExpectedDefaultDate = new DateTime(2023, 01, 16, 10, 10, 10, DateTimeKind.Utc);

        public ChatEventServiceTests()
        {
            _mockAccessEventRepository = new Mock<IRepository<AccessRoomEvent>>();
            _mockChatMessageRepository = new Mock<IRepository<ChatMessage>>();
            _mockChatHighFiveRepository = new Mock<IRepository<ChatHighFive>>();
            _mockAccessEventAssembler = new Mock<IChatEventAssembler<AccessRoomEvent>>();
            _mockChatMessageAssembler = new Mock<IChatEventAssembler<ChatMessage>>();
            _mockChatHighFiveAssembler = new Mock<IChatEventAssembler<ChatHighFive>>();

        }

        [Fact]
        public async Task When_GetAllEventsActivityAsync_WithNoActivity_ThenSucceed()
        {
            //Arrange
            var expectedTimeView = TimeGranularity.Seconds;
            var expectedAccessRoomEvents = new List<AccessRoomEvent>();
            var expectedChatMessageEvents = new List<ChatMessage>();
            var expectedChatHighFiveEvents = new List<ChatHighFive>();

            _mockAccessEventRepository.Setup(x => x.GetAllAsync<AccessRoomEvent>()).ReturnsAsync(expectedAccessRoomEvents);
            _mockChatMessageRepository.Setup(x => x.GetAllAsync<ChatMessage>()).ReturnsAsync(expectedChatMessageEvents);
            _mockChatHighFiveRepository.Setup(x => x.GetAllAsync<ChatHighFive>()).ReturnsAsync(expectedChatHighFiveEvents);

            _mockAccessEventAssembler.Setup(x => x.AssembleChatEventDtosFrom(expectedAccessRoomEvents, expectedTimeView)).Returns(new List<ChatEventDto>());
            _mockChatMessageAssembler.Setup(x => x.AssembleChatEventDtosFrom(expectedChatMessageEvents, expectedTimeView)).Returns(new List<ChatEventDto>());
            _mockChatHighFiveAssembler.Setup(x => x.AssembleChatEventDtosFrom(expectedChatHighFiveEvents, expectedTimeView)).Returns(new List<ChatEventDto>());

            //Act
            var sut = CreateSut();
            var actualChatEventDtos = await sut.GetAllEventsActivityAsync(expectedTimeView.ToString());


            //Assert
            Assert.NotNull(actualChatEventDtos);
            Assert.Empty(actualChatEventDtos);

            _mockAccessEventRepository.Verify(x => x.GetAllAsync<AccessRoomEvent>(), Times.Once);
            _mockChatMessageRepository.Verify(x => x.GetAllAsync<ChatMessage>(), Times.Once);
            _mockChatHighFiveRepository.Verify(x => x.GetAllAsync<ChatHighFive>(), Times.Once);

            _mockAccessEventAssembler.Verify(x => x.AssembleChatEventDtosFrom(expectedAccessRoomEvents, expectedTimeView), Times.Never);
            _mockChatMessageAssembler.Verify(x => x.AssembleChatEventDtosFrom(expectedChatMessageEvents, expectedTimeView), Times.Never);
            _mockChatHighFiveAssembler.Verify(x => x.AssembleChatEventDtosFrom(expectedChatHighFiveEvents, expectedTimeView), Times.Never);
        }

        [Fact]
        public async Task When_GetAllEventsActivityAsync_WithActivity_ThenSucceed()
        {
            //Arrange
            var expectedTimeView = TimeGranularity.Seconds;
            var expectedAccessRoomEvents = new List<AccessRoomEvent>()
            {
                new AccessRoomEvent(ExpectedDefaultDate, "Bob", EventType.EnterRoom)
            };
            var expectedAccessRoomDtos = new List<ChatEventDto>
            {
                new ChatEventDto("10:10:10 AM", "Bob entered the room")
            };
            var expectedChatMessageEvents = new List<ChatMessage>()
            {
                new ChatMessage(DateTime.Now.AddMinutes(1), "Mary", EventType.SendMessage, "Hi Bob"),
            };
            var expectedMessageDtos = new List<ChatEventDto>
            {
                new ChatEventDto("10:11:10 AM", "Bob comments:'Hi'")
            };
            var expectedChatHighFiveEvents = new List<ChatHighFive>();

            _mockAccessEventRepository.Setup(x => x.GetAllAsync<AccessRoomEvent>()).ReturnsAsync(expectedAccessRoomEvents);
            _mockChatMessageRepository.Setup(x => x.GetAllAsync<ChatMessage>()).ReturnsAsync(expectedChatMessageEvents);
            _mockChatHighFiveRepository.Setup(x => x.GetAllAsync<ChatHighFive>()).ReturnsAsync(expectedChatHighFiveEvents);

            _mockAccessEventAssembler.Setup(x => x.AssembleChatEventDtosFrom(expectedAccessRoomEvents, expectedTimeView)).Returns(expectedAccessRoomDtos);
            _mockChatMessageAssembler.Setup(x => x.AssembleChatEventDtosFrom(expectedChatMessageEvents, expectedTimeView)).Returns(expectedMessageDtos);
            _mockChatHighFiveAssembler.Setup(x => x.AssembleChatEventDtosFrom(expectedChatHighFiveEvents, expectedTimeView)).Returns(new List<ChatEventDto>());

            //Act
            var sut = CreateSut();
            var actualChatEventDtos = await sut.GetAllEventsActivityAsync(expectedTimeView.ToString());


            //Assert
            Assert.NotNull(actualChatEventDtos);
            Assert.Equal(2, actualChatEventDtos.Count());
            Assert.Equal("Bob entered the room", actualChatEventDtos.First().Message);
            Assert.Equal("Bob comments:'Hi'", actualChatEventDtos.Last().Message);


            _mockAccessEventRepository.Verify(x => x.GetAllAsync<AccessRoomEvent>(), Times.Once);
            _mockChatMessageRepository.Verify(x => x.GetAllAsync<ChatMessage>(), Times.Once);
            _mockChatHighFiveRepository.Verify(x => x.GetAllAsync<ChatHighFive>(), Times.Once);

            _mockAccessEventAssembler.Verify(x => x.AssembleChatEventDtosFrom(expectedAccessRoomEvents, expectedTimeView), Times.Once);
            _mockChatMessageAssembler.Verify(x => x.AssembleChatEventDtosFrom(expectedChatMessageEvents, expectedTimeView), Times.Once);
            _mockChatHighFiveAssembler.Verify(x => x.AssembleChatEventDtosFrom(expectedChatHighFiveEvents, expectedTimeView), Times.Never);
        }

        private ChatEventService CreateSut()
        {
            return new ChatEventService(
                _mockAccessEventRepository.Object,
                _mockChatMessageRepository.Object,
                _mockChatHighFiveRepository.Object,
                _mockAccessEventAssembler.Object,
                _mockChatMessageAssembler.Object,
                _mockChatHighFiveAssembler.Object
                );
        }


    }
}