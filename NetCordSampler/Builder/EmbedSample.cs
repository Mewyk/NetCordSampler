using System.Text;

namespace NetCordSampler.Builder;

public class EmbedSample
{
    private AuthorSample? _author;
    private FooterSample? _footer;
    private readonly List<FieldSample> _fields = [];

    private string? _title;
    private string? _description;
    private string? _url;
    private string? _timestamp;
    private int? _color;
    private string? _imageUrl;
    private string? _thumbnailUrl;

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

    public string Timestamp
    {
        get => _timestamp ?? "DateTimeOffset.UtcNow";
        set => _timestamp = value;
    }

    public int Color
    {
        get => _color ?? 0xFFA500;
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

    public AuthorSample? Author
    {
        get => _author;
        set => _author = value;
    }

    public FooterSample? Footer
    {
        get => _footer;
        set => _footer = value;
    }

    // TODO: Refactor fields so that it is not implemented like garbage
    public EmbedSample AddField(string name = "", string value = "", bool inline = false)
    {
        if (_fields.Count < 10)
        {
            _fields.Add(new FieldSample
            {
                Name = name,
                Value = value,
                Inline = inline
            });
        }
        return this;
    }

    public string Build(bool isQuick = false)
    {
        // Start
        var embedString = new StringBuilder();
        embedString.AppendLine("new()");
        embedString.AppendLine("{");

        // Properties
        if (_title != null || isQuick)
            embedString.AppendLine($"\tTitle = \"{EscapeString(_title ?? Title)}\",");

        if (_description != null || isQuick)
            embedString.AppendLine($"\tDescription = \"{EscapeString(_description ?? Description)}\",");

        if (_url != null || isQuick)
            embedString.AppendLine($"\tUrl = \"{EscapeString(_url ?? Url)}\",");

        if (_timestamp != null || isQuick)
            embedString.AppendLine($"\tTimestamp = {(_timestamp ?? Timestamp)},");

        if (_color.HasValue || isQuick)
            embedString.AppendLine($"\tColor = new Color(0x{(_color ?? Color):X6}),");

        if (_footer != null || isQuick)
            embedString.AppendLine($"\tFooter = {(_footer?.Build("\t") ?? new FooterSample().Build("\t"))},");

        if (_imageUrl != null || isQuick)
            embedString.AppendLine($"\tImage = \"{EscapeString(_imageUrl ?? ImageUrl)}\",");

        if (_thumbnailUrl != null || isQuick)
            embedString.AppendLine($"\tThumbnail = \"{EscapeString(_thumbnailUrl ?? ThumbnailUrl)}\",");

        if (_author != null || isQuick)
            embedString.AppendLine($"\tAuthor = {(_author?.Build("\t") ?? new AuthorSample().Build("\t"))},");

        if (isQuick)
        {
            _fields.Add(new FieldSample());
            _fields.Add(new FieldSample());
            _fields.Add(new FieldSample{ Inline = true });
        }

        if (_fields.Count != 0)
        {
            embedString.AppendLine("\tFields =");
            embedString.AppendLine("\t[");
            embedString.AppendLine(string.Join(",\n", _fields.Select(field => $"\t\t{field.Build("\t\t")}")));
            embedString.AppendLine("\t]");
        }

        // End
        embedString.AppendLine("};");
        return embedString.ToString();
    }

    // TODO: Refactor this so that it fully cleans strings
    private static string EscapeString(string input)
    {
        return input.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

    public static string QuickBuild() => new EmbedSample().Build(true);

    public class FooterSample
    {
        private string? _text;
        private string? _iconUrl;

        public string Text
        {
            get => _text ?? "Footer Text";
            set { if (!string.IsNullOrEmpty(value)) _text = value; }
        }

        public string IconUrl
        {
            get => _iconUrl ?? "https://example.com/embed/footer/icon.png";
            set { if (!string.IsNullOrEmpty(value)) _iconUrl = value; }
        }

        public StringBuilder Build(string indentation = "")
        {
            // Start
            var footerBuilder = new StringBuilder();
            footerBuilder.AppendLine("new()");
            footerBuilder.AppendLine($"{indentation}" + "{");

            // Properties
            footerBuilder.AppendLine($"\t{indentation}Text = \"{EscapeString(_text ?? Text)}\",");
            footerBuilder.AppendLine($"\t{indentation}IconUrl = \"{EscapeString(_iconUrl ?? IconUrl)}\"");

            // End
            footerBuilder.Append($"{indentation}" + "}");
            return footerBuilder;
        }
    }

    public class AuthorSample
    {
        private string? _name;
        private string? _url;
        private string? _iconUrl;

        public string Name
        {
            get => _name ?? "Author Name";
            set { if (!string.IsNullOrEmpty(value)) _name = value; }
        }

        public string Url
        {
            get => _url ?? "https://example.com/embed/author/url";
            set { if (!string.IsNullOrEmpty(value)) _url = value; }
        }

        public string IconUrl
        {
            get => _iconUrl ?? "https://example.com/embed/author/icon.png";
            set { if (!string.IsNullOrEmpty(value)) _iconUrl = value; }
        }

        public StringBuilder Build(string indentation = "")
        {
            // Start
            var authorBuilder = new StringBuilder();
            authorBuilder.AppendLine("new()");
            authorBuilder.AppendLine($"{indentation}" + "{");

            // Properties
            authorBuilder.AppendLine($"\t{indentation}Name = \"{EscapeString(_name ?? Name)}\",");
            authorBuilder.AppendLine($"\t{indentation}Url = \"{EscapeString(_url ?? Url)}\",");
            authorBuilder.AppendLine($"\t{indentation}IconUrl = \"{EscapeString(_iconUrl ?? IconUrl)}\"");

            // End
            authorBuilder.Append($"{indentation}" + "}");
            return authorBuilder;
        }
    }

    public class FieldSample
    {
        private string? _name;
        private string? _value;
        private bool _inline;

        public string Name
        {
            get => _name ?? "Default Field Name";
            set { if (!string.IsNullOrEmpty(value)) _name = value; }
        }

        public string Value
        {
            get => _value ?? "Default Field Value";
            set { if (!string.IsNullOrEmpty(value)) _value = value; }
        }

        public bool Inline
        {
            get => _inline;
            set => _inline = value;
        }

        public StringBuilder Build(string indentation = "")
        {
            // Start
            var fieldBuilder = new StringBuilder();
            fieldBuilder.AppendLine("new()");
            fieldBuilder.AppendLine($"{indentation}" + "{");

            // Properties
            fieldBuilder.AppendLine($"\t{indentation}Name = \"{EscapeString(_name ?? Name)}\",");
            fieldBuilder.AppendLine($"\t{indentation}Value = \"{EscapeString(_value ?? Value)}\",");
            fieldBuilder.AppendLine($"\t{indentation}Inline = {Inline.ToString().ToLower()}");

            // End
            fieldBuilder.Append($"{indentation}" + "}");
            return fieldBuilder;
        }
    }
}