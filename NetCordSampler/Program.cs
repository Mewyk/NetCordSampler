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

var host = builder.Build()
    .AddModules(typeof(Program).Assembly)
    .UseGatewayEventHandlers();

var embedCodeSample = host.Services.GetRequiredService<ICodeSample<EmbedProperties>>();
await GenerateExamples(embedCodeSample);

await host.RunAsync();

// NOTE: Below is just temporary for quick testing purposes
static async Task GenerateExamples(ICodeSample<EmbedProperties> codeSample)
{
    // Simple custom embed code sample (few options used)
    var simpleCustomEmbed = EmbedCodeSample.CreateCustom(embed =>
    {
        embed.Title = "Simple custom embed title";
        embed.Description = "Simple custom embed code sample description text";
    });
    Console.WriteLine("Simple Embed Output:");
    Console.WriteLine(await Task.Run(() => codeSample.BuildCodeSample(simpleCustomEmbed)));

    // Full custom embed code sample 
    var fullCustomEmbed = EmbedCodeSample.CreateCustom(embed =>
    {
        embed.Title = "Full custom embed title";
        embed.Description = "Full custom embed code sample description text";
        embed.Color = new(0xFF0000);
        embed.Timestamp = DateTimeOffset.UtcNow;
        embed.Image = "https://example.com/image.png";
        embed.Thumbnail = "https://example.com/thumbnail.png";
        embed.Url = "https://example.com";
        embed.Author = EmbedAuthorCodeSample.CreateCustom(author =>
        {
            author.Name = "Author Name";
            author.IconUrl = "https://example.com/icon.png";
            author.Url = "https://example.com";
        });
        embed.Footer = EmbedFooterCodeSample.CreateCustom(footer =>
        {
            footer.Text = "Footer Text";
            footer.IconUrl = "https://example.com/icon.png";
        });
        embed.Fields =
        [
            EmbedFieldCodeSample.CreateCustom(field =>
            {
                field.Name = "Field One";
                field.Value = "Value One";
                field.Inline = true;
            }),
            EmbedFieldCodeSample.CreateCustom(field =>
            {
                field.Name = "Field Two";
                field.Value = "Value Two";
                // Inline defaults to false
            })
        ];
    });
    Console.WriteLine("\nFull Embed Output:");
    Console.WriteLine(await Task.Run(() => codeSample.BuildCodeSample(fullCustomEmbed)));

    // QuickBuild (full auto code samples)
    // Uses default values and does not allow any custom values
    Console.WriteLine("\nQuickBuild Embed Output:");
    Console.WriteLine(await Task.Run(() => codeSample.QuickBuild()));
}
