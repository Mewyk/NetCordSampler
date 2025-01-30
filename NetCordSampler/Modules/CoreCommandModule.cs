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
        IntegrationTypes = [ApplicationIntegrationType.GuildInstall])]
    public InteractionMessageProperties SearchSamples(
        [SlashCommandParameter(
            Description = "Description",
            MaxLength = 90,
            AutocompleteProviderType = typeof(SearchSamplesAutocompleteProvider))]
        string sample)
    {
        return _settings == null
            ? throw new ArgumentNullException(nameof(sample), "SamplerSettings cannot be null")
            : SampleHelper.CreateQuickBuildMessage(sample, _builder, _settings);
    }

    public class SearchSamplesAutocompleteProvider : IAutocompleteProvider<AutocompleteInteractionContext>
    {
        public ValueTask<IEnumerable<ApplicationCommandOptionChoiceProperties>?> GetChoicesAsync(
            ApplicationCommandInteractionDataOption option,
            AutocompleteInteractionContext context)
        {
            Console.WriteLine(context.Interaction.Data.Id);
            Console.WriteLine(option.Options?.Count);
            Console.WriteLine(option.Value?.ToString());

            var searchTerm = option.Value?.ToString() ?? string.Empty;
            var sampleNames = SampleHelper.FindSamples(searchTerm, 0, 25, out var total);

            Console.WriteLine($"Found {total} results.");

            var choices = sampleNames.Select(name =>
            {
                string displayName = name.Length > 90 ? name[..90] : name;
                return new ApplicationCommandOptionChoiceProperties(displayName, name);
            });

            return new ValueTask<IEnumerable<ApplicationCommandOptionChoiceProperties>?>(choices);
        }
    }
}
