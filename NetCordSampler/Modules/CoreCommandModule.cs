using Microsoft.Extensions.Options;

using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace NetCordSampler.Modules;

public class CoreCommandModule(IOptions<Configuration> options) : ApplicationCommandModule<SlashCommandContext>
{
    [SlashCommand("search", "Search and customize NetCord samples", 
        IntegrationTypes = [
            ApplicationIntegrationType.GuildInstall, 
            ApplicationIntegrationType.UserInstall])]
    public InteractionMessageProperties SearchSamples(
        [SlashCommandParameter(
            Description = "",
            MaxLength = 90,
            AutocompleteProviderType = typeof(SearchSamplesAutocompleteProvider))]
        string sampleSelection)
    {
        var settings = options?.Value?.Settings;
        return settings == null
            ? throw new ArgumentNullException(nameof(sampleSelection), "SamplerSettings cannot be null")
            : SampleHelper.CreateQuickBuildMessage(sampleSelection, settings, Context);
    }

    private class SearchSamplesAutocompleteProvider : IAutocompleteProvider<AutocompleteInteractionContext>
    {
        public ValueTask<IEnumerable<ApplicationCommandOptionChoiceProperties>?> GetChoicesAsync(
            ApplicationCommandInteractionDataOption option, 
            AutocompleteInteractionContext context)
        {
            return new(SampleHelper.FindSamples(option.Value?.ToString() 
                ?? string.Empty, 0, 25, out _)
                .Select(source =>
                {
                    string name = source.Name;

                    if (name.Length > 90)
                        name = name[..90];

                    return new ApplicationCommandOptionChoiceProperties(name, name);
                }));
        }
    }
}
