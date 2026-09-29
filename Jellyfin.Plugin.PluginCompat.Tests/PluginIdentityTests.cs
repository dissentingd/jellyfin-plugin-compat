namespace Jellyfin.Plugin.PluginCompat.Tests;

public class PluginIdentityTests
{
    [Fact]
    public void GuidIsValidAndMatchesBuildYaml()
    {
        var guid = Guid.Parse(Jellyfin.Plugin.PluginCompat.Plugin.PluginGuid);
        Assert.NotEqual(Guid.Empty, guid);

        var buildYaml = File.ReadAllText(Path.Combine(RepoRoot(), "build.yaml"));
        Assert.Contains($"guid: \"{guid}\"", buildYaml, StringComparison.Ordinal);
    }

    private static string RepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "build.yaml")))
        {
            dir = dir.Parent;
        }

        return dir?.FullName ?? throw new InvalidOperationException("build.yaml not found");
    }
}
