/* Usage
// Quick Build
string quickEmbedCode = EmbedSample.QuickBuild();
Console.WriteLine(quickEmbedCode);

// Custom Build
var embedSample = new EmbedSample
{
    Title = "Custom Title",
    Description = "Custom Description"
};
embedSample.AddField("Field 1", "Value 1", inline: true);
string customEmbedCode = embedSample.BuildEmbedCode();
Console.WriteLine(customEmbedCode);
*/

using System.Reflection;
using System.Text;
using NetCord;
using NetCord.Rest;

namespace NetCordSampler.CodeSamples.Rest;

// TODO: Add attributes, interfaces, configuration, etc
public class EmbedSample
{
    // Backing fields
    private string? _title;
    private string? _description;
    private string? _url;
    private DateTimeOffset? _timestamp;
    private Color? _color;
    private string? _imageUrl;
    private string? _thumbnailUrl;
    private EmbedAuthorProperties? _author;
    private EmbedFooterProperties? _footer;
    private List<EmbedFieldProperties>? _fields;

    public string Title
    {
        get => _title ?? "Title of the embed";
        set => _title = value;
    }

    public string Description
    {
        get => _description ?? "Description of the embed";
        set => _description = value;
    }

    public string Url
    {
        get => _url ?? "https://example.com";
        set => _url = value;
    }

    public DateTimeOffset Timestamp
    {
        get => _timestamp ?? DateTimeOffset.UtcNow;
        set => _timestamp = value;
    }

    public Color Color
    {
        get => _color ?? new Color(0xFFA500);
        set => _color = value;
    }

    public string ImageUrl
    {
        get => _imageUrl ?? "https://example.com/embed/image.png";
        set => _imageUrl = value;
    }

    public string ThumbnailUrl
    {
        get => _thumbnailUrl ?? "https://example.com/embed/thumbnail.png";
        set => _thumbnailUrl = value;
    }

    public EmbedAuthorProperties? Author
    {
        get => _author;
        set => _author = value;
    }

    public EmbedFooterProperties? Footer
    {
        get => _footer;
        set => _footer = value;
    }

    public List<EmbedFieldProperties> Fields
    {
        get => _fields ??= [];
        set => _fields = value;
    }

    // TODO: This can be handled better
    //       Revisit after attributes are added
    public EmbedSample AddField(string name = "Default Field Name", string value = "Default Field Value", bool inline = false)
    {
        if (Fields.Count < 10)
        {
            Fields.Add(new EmbedFieldProperties
            {
                Name = name,
                Value = value,
                Inline = inline
            });
        }
        return this;
    }

    public string BuildEmbedCode()
    {
        var embedProperties = typeof(EmbedProperties);
        var properties = embedProperties.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var codeBuilder = new StringBuilder();
        codeBuilder.AppendLine("new EmbedProperties()");
        codeBuilder.AppendLine("{");

        foreach (var property in properties)
        {
            var value = GetPropertyValue(property.Name);
            if (value != null)
            {
                var valueStr = FormatProperty(value);
                if (!string.IsNullOrEmpty(valueStr))
                {
                    codeBuilder.AppendLine($"\t{property.Name} = {valueStr},");
                }
            }
        }

        codeBuilder.AppendLine("};");
        return codeBuilder.ToString();
    }

    public static string QuickBuild()
    {
        var embedSample = new EmbedSample
        {
            Title = "Title of the embed",
            Description = "Description of the embed",
            Url = "https://example.com",
            Timestamp = DateTimeOffset.UtcNow,
            Color = new Color(0xFFA500),
            ImageUrl = "https://example.com/embed/image.png",
            ThumbnailUrl = "https://example.com/embed/thumbnail.png",
            Author = new EmbedAuthorProperties
            {
                Name = "Author Name",
                Url = "https://example.com/embed/author/url",
                IconUrl = "https://example.com/embed/author/icon.png"
            },
            Footer = new EmbedFooterProperties
            {
                Text = "Footer Text",
                IconUrl = "https://example.com/embed/footer/icon.png"
            }
        };

        // Add sample fields
        embedSample.AddField();
        embedSample.AddField();
        embedSample.AddField(inline: true);

        return embedSample.BuildEmbedCode();
    }

    private object? GetPropertyValue(string propertyName)
    {
        var fieldName = $"_{char.ToLower(propertyName[0])}{propertyName[1..]}";
        var fieldInfo = typeof(EmbedSample).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

        if (fieldInfo != null)
        {
            var value = fieldInfo.GetValue(this);

            if (propertyName == nameof(Fields))
                return value is List<EmbedFieldProperties> fields && fields.Count > 0 ? fields : null;

            return value;
        }

        return null;
    }

    private static string? FormatProperty(object value)
    {
        return value switch
        {
            string valueString => $"\"{EscapeString(valueString)}\"",
            DateTimeOffset offset => $"DateTimeOffset.Parse(\"{offset.ToUniversalTime():u}\")",
            Color color => $"new Color(0x{color.RawValue:X6})",
            EmbedAuthorProperties author => BuildAuthorCode(author),
            EmbedFooterProperties footer => BuildFooterCode(footer),
            IEnumerable<EmbedFieldProperties> fields => BuildFieldsCode(fields),
            _ => null
        };
    }

    private static string BuildAuthorCode(EmbedAuthorProperties author)
    {
        var builder = new StringBuilder();
        builder.AppendLine("new EmbedAuthorProperties");
        builder.AppendLine("{");
        AppendProperty(builder, nameof(author.Name), author.Name);
        AppendProperty(builder, nameof(author.Url), author.Url);
        AppendProperty(builder, nameof(author.IconUrl), author.IconUrl, isLast: true);
        builder.Append('}');
        return builder.ToString();
    }

    private static string BuildFooterCode(EmbedFooterProperties footer)
    {
        var builder = new StringBuilder();
        builder.AppendLine("new EmbedFooterProperties");
        builder.AppendLine("{");
        AppendProperty(builder, nameof(footer.Text), footer.Text);
        AppendProperty(builder, nameof(footer.IconUrl), footer.IconUrl, isLast: true);
        builder.Append('}');
        return builder.ToString();
    }

    private static string BuildFieldsCode(IEnumerable<EmbedFieldProperties> fields)
    {
        var builder = new StringBuilder();
        builder.AppendLine("new List<EmbedFieldProperties>");
        builder.AppendLine("{");

        foreach (var field in fields)
        {
            builder.AppendLine("\tnew EmbedFieldProperties");
            builder.AppendLine("\t{");
            AppendProperty(builder, "Name", field.Name);
            AppendProperty(builder, "Value", field.Value);
            builder.AppendLine($"\t\tInline = {field.Inline.ToString().ToLowerInvariant()}");
            builder.AppendLine("\t},");
        }

        builder.AppendLine("}");
        return builder.ToString();
    }

    private static void AppendProperty(StringBuilder builder, string propertyName, string? value, bool isLast = false)
    {
        if (!string.IsNullOrEmpty(value))
        {
            builder.AppendLine($"\t{propertyName} = \"{EscapeString(value)}\"{(isLast ? "" : ",")}");
        }
    }

    // TODO: Move to common and add more/better cleaning functionalities
    private static string EscapeString(string input) => input.Replace("\\", "\\\\").Replace("\"", "\\\"");
}

