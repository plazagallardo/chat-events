using Chat.Core.Application.Domain;
using Chat.Core.Application.Domain.AccessEvents.Assemblers;
using Chat.Core.Application.Domain.Base.Assemblers;
using Chat.Core.Application.Domain.ChatMessages.Assemblers;
using Chat.Core.Application.Domain.HighFives.Assemblers;
using Chat.Core.Application.Infrastructure.Persistence;
using Chat.Persistence.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IChatEventAssembler<AccessRoomEvent>, AccessEventAssembler>();
builder.Services.AddScoped<IChatEventAssembler<ChatMessage>, ChatMessageAssembler>();
builder.Services.AddScoped<IChatEventAssembler<ChatHighFive>, ChatHighFiveAssembler>();
builder.Services.AddScoped<IChatEventService, ChatEventService>();
builder.Services.AddSingleton<IRepository<AccessRoomEvent>, InMemoryChatAccessRoomEventMessageRepository>();
builder.Services.AddSingleton<IRepository<ChatHighFive>, InMemoryChatHighFiveMessageRepository>();
builder.Services.AddSingleton<IRepository<ChatMessage>, InMemoryChatMessageRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
