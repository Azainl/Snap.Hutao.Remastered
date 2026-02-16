namespace Snap.Hutao.Remastered.API.Model;

public class PluginManifest
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("version")]
    public PluginVersion Version { get; set; } = new PluginVersion(1, 0, 0);

    [JsonPropertyName("author")]
    public List<string>? Author { get; set; } = new();

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    public string? IconPath { get; set; }
}
