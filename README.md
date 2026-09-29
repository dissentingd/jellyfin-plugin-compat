# jellyfin-plugin-compat

> **Status: pre-alpha — design stage, not functional yet.** See [docs/DESIGN.md](docs/DESIGN.md).

Before you upgrade Jellyfin, find out which of your plugins will break — and after, how to fix the ones that did.

A Jellyfin plugin for Jellyfin **12.1+**. Addresses the feature request no single feature request; a recurring pain on every major Jellyfin release.

## The problem

Every major Jellyfin release changes the plugin ABI, and plugins without a matching
build are disabled on first start. There's no way to see that coming: you upgrade, then discover
that some plugins were switched off — some just need an update that already exists, some run fine
if re-enabled, some have no compatible build at all, and sideloaded ones were never tracked by any
repository to begin with.

## Planned

- **Pre-upgrade check:** pick a target version (e.g. the next release); for each installed
  plugin, report whether a compatible build exists in your configured repositories, the newest
  version and its target ABI, when it was last released, and whether it's sideloaded.
- **Post-upgrade triage:** list plugins that were disabled or failed to load, with the likely fix —
  *update available*, *try re-enabling* (targets an older ABI of the same major version),
  *no compatible build yet*, *sideloaded: rebuild needed*.
- **Health checks** that plugins commonly break after an upgrade (e.g. the Scheduled Tasks page,
  API documentation generation).

## Develop

```bash
dotnet build jellyfin-plugin-compat.sln
dotnet test jellyfin-plugin-compat.sln
dotnet publish Jellyfin.Plugin.PluginCompat -c Release -o out
```

To try a build by hand: copy `out/Jellyfin.Plugin.PluginCompat.dll` into a new folder in Jellyfin's plugin directory
(e.g. `<config>/data/plugins/Plugin Compatibility Check_0.0.1.0/`) and restart Jellyfin.

## License

[MIT](LICENSE)
