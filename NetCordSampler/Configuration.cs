using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace NetCordSampler;

public class Configuration
{
    [Required]
    public required DiscordSettings? Discord { get; set; }

    [Required]
    public required CodeSamplesSettings? CodeSamples { get; set; }
}

// Discord settings
public class DiscordSettings
{
    [Required]
    public required string Token { get; set; }

    [Required]
    public required string[] Intents { get; set; }
}

// CodeSamples settings
public class CodeSamplesSettings
{
    [Required]
    public required DefaultValues.Url? DefaultUrls { get; set; }

    [Required]
    public required UserInterface.Search SearchInterface { get; set; }

    [Required]
    public required UserInterface.Browse BrowseInterface { get; set; }

    [Required]
    public required UserInterface.Builder BuilderInterface { get; set; }

    public class UserInterface
    {
        public class Search
        {
            public required int EmbedColor { get; set; }
        }

        public class Browse
        {
            public required int EmbedColor { get; set; }
        }

        public class Builder
        {
            public required int EmbedColor { get; set; }
        }
    }

    public class DefaultValues
    {
        [Required]
        public required string MissingDescription { get; set; }

        public class Url
        {
            [Required]
            public required string Thumbnail { get; set; }

            [Required]
            public required string Image { get; set; }

            [Required]
            public required string Video { get; set; }

            [Required]
            public required string Audio { get; set; }
        }
    }
}
