using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace NetCordSampler;

public class Configuration
{
    [Required]
    public required DiscordSettings Discord { get; set; }

    [Required]
    public required SamplerSettings Sampler { get; set; }
}

// Discord settings
public class DiscordSettings
{
    [Required]
    public required string Token { get; set; }

    [Required]
    public required string[] Intents { get; set; }
}

// Sampler settings
public class SamplerSettings
{
    [Required]
    public required Defaults DefaultValues { get; set; }

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
            [Required]
            public required int EmbedColor { get; set; }
        }

        public class Browse
        {
            [Required]
            public required int EmbedColor { get; set; }
        }

        public class Builder
        {
            [Required]
            public required int EmbedColor { get; set; }
        }
    }

    public class Defaults
    {
        [Required]
        public required string MissingTitle { get; set; }

        [Required]
        public required string MissingDescription { get; set; }

        [Required]
        public required Url Urls { get; set; }

        public class Url
        {
            [Required]
            public required string Website { get; set; }

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
