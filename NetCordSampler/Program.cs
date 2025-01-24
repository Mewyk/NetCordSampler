using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.ComponentInteractions;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.ComponentInteractions;

using NetCordSampler;
using NetCordSampler.CodeSamples.Rest;
using NetCordSampler.Interfaces;

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

builder.Services.AddSingleton<EmbedCodeSample>();
builder.Services.AddSingleton<EmbedAuthorCodeSample>();
builder.Services.AddSingleton<EmbedFooterCodeSample>();
builder.Services.AddSingleton<EmbedFieldCodeSample>();

var host = builder.Build()
    .AddModules(typeof(Program).Assembly)
    .UseGatewayEventHandlers();

var sampleBuilder = host.Services.GetRequiredService<EmbedCodeSample>();
await GenerateExamples(sampleBuilder);

await host.RunAsync();

// NOTE: Below is just temporary for quick testing purposes
static async Task GenerateExamples(ICodeSample codeSample)
{
    // Simple custom embed code sample (few options used)
    var simpleCustomEmbed = new EmbedProperties
    {
        Title = "Simple custom embed title",
        Description = "Simple custom embed code sample description text"
    };
    Console.WriteLine("Simple Embed Output:");
    Console.WriteLine(await Task.Run(() => codeSample.BuildCodeSample(simpleCustomEmbed)));

    // Full custom embed code sample 
    var fullCustomEmbed = new EmbedProperties
    {
        Title = "Full custom embed title",
        Description = "Full custom embed code sample description text",
        Color = new(0xFF0000),
        Timestamp = DateTimeOffset.UtcNow,
        Image = "https://example.com/image.png",
        Thumbnail = "https://example.com/thumbnail.png",
        Url = "https://example.com",
        Author = new EmbedAuthorProperties
        {
            Name = "Author Name",
            IconUrl = "https://example.com/icon.png",
            Url = "https://example.com"
        },
        Footer = new EmbedFooterProperties
        {
            Text = "Footer Text",
            IconUrl = "https://example.com/icon.png"
        },
        Fields =
        [
            new() { Name = "Field One", Value = "Value One", Inline = true },
            new() { Name = "Field Two", Value = "Value Two"}
        ]
    };
    Console.WriteLine("\nFull Embed Output:");
    Console.WriteLine(await Task.Run(() => codeSample.BuildCodeSample(fullCustomEmbed)));

    // QuickBuild (full auto code samples)
    // Uses default values and does not allow any custom values
    Console.WriteLine("\nQuickBuild Embed Output:");
    Console.WriteLine(await Task.Run(() => codeSample.QuickBuild(typeof(EmbedProperties))));
}
