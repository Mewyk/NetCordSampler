using NetCordSampler.Builder;

var footer = new EmbedSample.FooterSample
{
    Text = "Text",
    IconUrl = "https://example.com/embed/footer.png",
};
var footerCode = footer.Build();
Console.WriteLine($"var footer = {footerCode}");

var author = new EmbedSample.AuthorSample
{
    Name = "Name",
    Url = "https://example.com/author/url",
    IconUrl = "https://example.com/author/icon.png",
};
var authorCode = author.Build();
Console.WriteLine($"var author = {authorCode}");

var field = new EmbedSample.FieldSample();
var fieldCode = field.Build();
Console.WriteLine($"var field = {fieldCode}");

var embed = new EmbedSample
{
    Title = "Title of the embed",
    Description = "Description of the embed",
    Url = "https://example.com",
    Color = 0xFF0000,
    ImageUrl = "https://example.com/embed/image.png",
    ThumbnailUrl = "https://example.com/embed/thumbnail.png",
    Timestamp = "DateTimeOffset.UtcNow",
    Author = new EmbedSample.AuthorSample
    {
        Name = "Name",
        Url = "https://example.com/author/url",
        IconUrl = "https://example.com/embed/author/icon.png",
    },
    Footer = new EmbedSample.FooterSample
    {
        Text = "Text",
        IconUrl = "https://example.com/embed/footer.png",
    }
};

embed.AddField("Field1", "Value1", true)
     .AddField("Field2", "Value2");

var embedCode = embed.Build();
Console.WriteLine($"var embed = {embedCode}");
Console.WriteLine($"var embed = {EmbedSample.QuickBuild()}");