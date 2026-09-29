using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.PluginCompat.Configuration;

/// <summary>Plugin settings. Placeholder until v0.1 settles what's configurable.</summary>
public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>Gets or sets a value indicating whether the plugin is enabled.</summary>
    public bool Enabled { get; set; } = true;
}
