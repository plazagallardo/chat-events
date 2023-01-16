// See https://aka.ms/new-console-template for more information
using Chat.Core.Application.Domain;
using Chat.Core.Application.Domain.AccessEvents.Assemblers;
using Chat.Core.Application.Domain.Base.Assemblers;
using Chat.Core.Application.Domain.ChatMessages.Assemblers;
using Chat.Core.Application.Domain.Enums;
using Chat.Core.Application.Domain.HighFives.Assemblers;
using Chat.Core.Application.Infrastructure.Persistence;
using Chat.Persistence.InMemory;
using Microsoft.Extensions.DependencyInjection;

//setup our DI
var serviceProvider = new ServiceCollection()
    .AddScoped<IChatEventAssembler<AccessRoomEvent>, AccessEventAssembler>()
    .AddScoped<IChatEventAssembler<ChatMessage>, ChatMessageAssembler>()
    .AddScoped<IChatEventAssembler<ChatHighFive>, ChatHighFiveAssembler>()
    .AddScoped<IChatEventService, ChatEventService>()
    .AddSingleton<IRepository<AccessRoomEvent>, InMemoryChatAccessRoomEventMessageRepository>()
    .AddSingleton<IRepository<ChatHighFive>, InMemoryChatHighFiveMessageRepository>()
    .AddSingleton<IRepository<ChatMessage>, InMemoryChatMessageRepository>()
    .BuildServiceProvider();

bool continuee = true;

while (continuee)
{
    Console.WriteLine($"\n\n");
    Console.WriteLine($"What do you want to do?");
    Console.WriteLine($"1. Enter chat");
    Console.WriteLine($"2. Leave chat");
    Console.WriteLine($"3. Add comment");
    Console.WriteLine($"4. Send High Five");
    Console.WriteLine($"5. Show all past activity\n\n");

    string menu = Console.ReadLine();

    switch (menu)
    {
        case "1":
            Console.Write($"Add person name which will enter the room:");
            string personNameToEnter = Console.ReadLine();
            await AccessAsync(EventType.EnterRoom, personNameToEnter, serviceProvider);
            break;

        case "2":
            Console.Write($"Add person name which will leave the room:");
            string personNameToLeave = Console.ReadLine();
            await AccessAsync(EventType.LeaveRoom, personNameToLeave, serviceProvider);
            break;

        case "3":
            Console.Write($"Add person name who comments:");
            string commentBy = Console.ReadLine();
            Console.Write($"Add the comment: ");
            string comment = Console.ReadLine();
            await SendCommentAsync(commentBy, comment, serviceProvider);
            break;

        case "4":
            Console.Write($"Add person who sends the high five:");
            string sendedBy = Console.ReadLine();
            Console.Write($"Add person who should receive the high five:");
            string receivedBy = Console.ReadLine();
            await SendHighFiveAsync(sendedBy, receivedBy, serviceProvider);
            break;

        case "5":
            Console.Write($"Define the time scope you want to visualize the chat activity.\n" +
                $"Pick from -> Seconds, Minutes, Hours, Days\n");
            string timeGranularity = Console.ReadLine();
            await DisplayActivityAsync(timeGranularity, serviceProvider);

            break;

        case "e":
        default:
            continuee = false;
            break;
    }
}
async Task AccessAsync(EventType eventType, string personName, IServiceProvider serviceProvider)
{
    var eventRepository = serviceProvider.GetService<IRepository<AccessRoomEvent>>();
    if (eventType == EventType.EnterRoom)
    {
        await eventRepository.AddAsync(new AccessRoomEvent(DateTime.UtcNow, personName, EventType.EnterRoom));
        Console.WriteLine(personName + " enters the room");
    }
    else
    {
        await eventRepository.AddAsync(new AccessRoomEvent(DateTime.UtcNow, personName, EventType.LeaveRoom));
        Console.WriteLine(personName + " leaves the room");
    }
}

async Task SendHighFiveAsync(string sender, string receiver, IServiceProvider serviceProvider)
{
    var chatEventService = serviceProvider.GetService<IChatEventService>();
    await chatEventService.SendHighFive(DateTime.UtcNow, sender, receiver);
    Console.WriteLine(sender + " high-fives " + receiver);
}

async Task SendCommentAsync(string sender, string comment, IServiceProvider serviceProvider)
{
    var chatEventService = serviceProvider.GetService<IChatEventService>();
    await chatEventService.SendTextMessage(DateTime.UtcNow, sender, comment);
    Console.WriteLine(sender + " comments: " + comment);

}

async Task DisplayActivityAsync(string timeGranularity, IServiceProvider serviceProvider)
{
    var chatEventService = serviceProvider.GetService<IChatEventService>();
    var chatEvents = await chatEventService.GetAllEventsActivityAsync(timeGranularity);
    var chatEventGroupedList = chatEvents.GroupBy(x => x.TimeScope).ToList();

    Console.WriteLine("Granularity: " + timeGranularity + "\n\n");
    foreach (var chatEventGroupedItem in chatEventGroupedList)
    {
        Console.WriteLine($"{chatEventGroupedItem.Key}:");
        //iterating through values
        foreach (var chatEventItem in chatEventGroupedItem)
        {
            Console.WriteLine($"\t\t{chatEventItem.Message}\n");
        }
    }
}
