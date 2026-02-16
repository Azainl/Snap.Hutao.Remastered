namespace Snap.Hutao.Remastered.API.Model;

public class PluginVersion
{
    [JsonPropertyName("major")]
    public int Major { get; set; }

    [JsonPropertyName("minor")]
    public int Minor { get; set; }

    [JsonPropertyName("patch")]
    public int Patch { get; set; }

    public PluginVersion(int major, int minor, int patch)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
    }

    public static PluginVersion Parse(string version)
    {
        string[] parts = version.Split('.');
        if (parts.Length != 3)
        {
            throw new FormatException("Invalid version format. Expected format: Major.Minor.Patch");
        }
        return new PluginVersion(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
    }
    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }

    public static bool operator >(PluginVersion v1, PluginVersion v2)
    {
        if (v1.Major != v2.Major)
            return v1.Major > v2.Major;
        if (v1.Minor != v2.Minor)
            return v1.Minor > v2.Minor;
        return v1.Patch > v2.Patch;
    }

    public static bool operator <(PluginVersion v1, PluginVersion v2)
    {
        if (v1.Major != v2.Major)
            return v1.Major < v2.Major;
        if (v1.Minor != v2.Minor)
            return v1.Minor < v2.Minor;
        return v1.Patch < v2.Patch;
    }

    public static bool operator >=(PluginVersion v1, PluginVersion v2)
    {
        return !(v1 < v2);
    }

    public static bool operator <=(PluginVersion v1, PluginVersion v2)
    {
        return !(v1 > v2);
    }

    public override bool Equals(object? obj)
    {
        if (obj is PluginVersion other)
        {
            return Major == other.Major && Minor == other.Minor && Patch == other.Patch;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Major, Minor, Patch);
    }

    public static bool operator ==(PluginVersion v1, PluginVersion v2)
    {
        return v1.Equals(v2);
    }

    public static bool operator !=(PluginVersion v1, PluginVersion v2)
    {
        return !v1.Equals(v2);
    }
}
