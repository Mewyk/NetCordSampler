using System.ComponentModel.DataAnnotations;

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

    public class DefaultValues
    {
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
