using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
using NetCordSampler.CodeSamples;
using NetCordSampler.CodeSamples.RestObjects;
using NetCordSampler.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddOptions<Configuration>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations();

builder.Services
    .AddOptions<SamplerSettings>()
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

builder.Services.AddSingleton<ICodeSample<EmbedProperties>, EmbedCodeSample>();
builder.Services.AddSingleton<ICodeSample<EmbedAuthorProperties>, EmbedAuthorCodeSample>();
builder.Services.AddSingleton<ICodeSample<EmbedFooterProperties>, EmbedFooterCodeSample>();
builder.Services.AddSingleton<ICodeSample<EmbedFieldProperties>, EmbedFieldCodeSample>();

builder.Services.AddSingleton<Builder>();

var host = builder.Build()
    .AddModules(typeof(Program).Assembly)
    .UseGatewayEventHandlers();

var builderInstance = host.Services.GetRequiredService<Builder>();
var samplerSettings = host.Services.GetRequiredService<IOptions<SamplerSettings>>().Value;

await GenerateExamples(builderInstance);

await host.RunAsync();

// NOTE: Below is just temporary for quick testing purposes
static Task GenerateExamples(Builder builder)
{
    // Simple custom embed code sample (few options used)
    var simpleCustomEmbedCode = builder.CustomBuild<EmbedProperties>(embed =>
    {
        embed.Title = "Simple custom embed title";
        embed.Description = "Simple custom embed code sample description text";
    });
    Console.WriteLine("Simple Embed Output:");
    Console.WriteLine(simpleCustomEmbedCode);

    // Full custom embed code sample
    var fullCustomEmbedCode = builder.CustomBuild<EmbedProperties>(embed =>
    {
        embed.Title = "Full custom embed title";
        embed.Description = "Full custom embed code sample description text";
        embed.Color = new(0xFF0000);
        embed.Timestamp = DateTimeOffset.UtcNow;
        embed.Image = "https://example.com/image.png";
        embed.Thumbnail = "https://example.com/thumbnail.png";
        embed.Url = "https://example.com";
        embed.Author = new EmbedAuthorProperties
        {
            Name = "Author Name",
            IconUrl = "https://example.com/icon.png",
            Url = "https://example.com"
        };
        embed.Footer = new EmbedFooterProperties
        {
            Text = "Footer Text",
            IconUrl = "https://example.com/icon.png"
        };
        embed.Fields =
        [
            new EmbedFieldProperties
            {
                Name = "Field One",
                Value = "Value One",
                Inline = true
            },
            new EmbedFieldProperties
            {
                Name = "Field Two",
                Value = "Value Two"
                // Inline defaults to false
            }
        ];
    });
    Console.WriteLine("\nFull Embed Output:");
    Console.WriteLine(fullCustomEmbedCode);

    // QuickBuild (full auto code samples)
    Console.WriteLine("\nQuickBuild Embed Output:");
    var quickBuildEmbedCode = builder.QuickBuild<EmbedProperties>();
    Console.WriteLine(quickBuildEmbedCode);

    return Task.CompletedTask;
}
