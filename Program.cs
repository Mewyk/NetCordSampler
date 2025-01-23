using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Rest;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.ComponentInteractions;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.ComponentInteractions;

using NetCordSampler;
using NetCordSampler.CodeSamples.Rest;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddOptions<Configuration>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations();

builder.Services
    .AddHttpClient();

builder.Services
    .AddApplicationCommands<ApplicationCommandInteraction, ApplicationCommandContext>()
    .AddComponentInteractions<ButtonInteraction, ButtonInteractionContext>()
    .AddComponentInteractions<StringMenuInteraction, StringMenuInteractionContext>();

builder.Services
    .AddDiscordRest()
    .AddDiscordGateway(options =>
    {
        options.Intents =
            GatewayIntents.GuildMessages |
            GatewayIntents.GuildMessageReactions |
            GatewayIntents.MessageContent;
    });

builder.Services
    .AddLogging(configure => configure
        .AddConsole()
        .SetMinimumLevel(LogLevel.Debug));

var host = builder.Build()
    .AddModules(typeof(Program).Assembly)
    .UseGatewayEventHandlers();

await host.RunAsync();