using Microsoft.Extensions.Options;

using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

using NetCordSampler.CodeSamples;

namespace NetCordSampler.Modules;

public class CoreCommandModule(
    IOptions<Configuration> settings, Builder builder) 
    : ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly SamplerSettings _settings = settings.Value.Sampler;
    private readonly Builder _builder = builder;

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
        return _settings == null
            ? throw new ArgumentNullException(nameof(sampleSelection), "SamplerSettings cannot be null")
            : SampleHelper.CreateQuickBuildMessage(sampleSelection, _settings, Context, _builder);
    }

    private class SearchSamplesAutocompleteProvider : IAutocompleteProvider<AutocompleteInteractionContext>
    {
        public ValueTask<IEnumerable<ApplicationCommandOptionChoiceProperties>?> GetChoicesAsync(
            ApplicationCommandInteractionDataOption option,
            AutocompleteInteractionContext context)
        {
            var searchTerm = option.Value?.ToString() ?? string.Empty;
            var samples = SampleHelper.FindSamples(searchTerm, 0, 25, out _);

            var choices = samples.Select(source =>
            {
                string name = source.Name.Length > 90 ? source.Name[..90] : source.Name;
                return new ApplicationCommandOptionChoiceProperties(name, name);
            });

            return new ValueTask<IEnumerable<ApplicationCommandOptionChoiceProperties>?>(choices);
        }
    }
}
