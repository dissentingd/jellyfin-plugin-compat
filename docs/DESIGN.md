# Design notes

Working design for v0.1 (Jellyfin 12.1+).

## Data available inside the server

- Installed plugins: `IPluginManager` — name, id, version, target ABI, status (`Active`,
  `Disabled`, `NotSupported`, `Malfunctioned`, …), and the folder's `meta.json`.
- Configured repositories: `IServerConfigurationManager` → plugin repository URLs; each serves a
  manifest (JSON) listing packages with `versions[]` → `version`, `targetAbi`, `timestamp`,
  `sourceUrl`, `changelog`.
- `IInstallationManager` already fetches and parses these manifests (`GetAvailablePackages`).
- The running server version is known; the **target** version is user-entered, or taken from the
  latest Jellyfin release.

## Report (per installed plugin)

| Field | From |
|---|---|
| Installed version, target ABI, status | plugin manager / `meta.json` |
| In a repository? which one | manifest match by plugin GUID |
| Newest version for the target ABI | manifest `versions[]` filtered by `targetAbi <= target` |
| Last release date | manifest `timestamp` |
| Verdict | *OK*, *update available*, *likely OK (older ABI, same major)*, *no compatible build*, *sideloaded — unknown* |

"Likely OK" needs care: a plugin built for an earlier minor ABI of the same major often runs, but not
always. Report it as a hint, never as a guarantee.

## Health checks (post-upgrade)

Probe server surfaces that a misbehaving plugin can take down, from inside the server:
the scheduled-task list, API documentation generation, the plugin list itself. Report which fail and,
where possible, which plugin's types appear in the exception.

## Surfaces

- Settings page with the report table, a target-version box, and a **Run check** button.
- A scheduled task (weekly by default) that refreshes the report; optional notification when a new
  Jellyfin release appears and any installed plugin lacks a compatible build.
- API (admin only): `GET /PluginCompat/Report?target=`.

## Chicken-and-egg

This plugin also has to be compatible with the new server. Keep it dependency-light (only
`Jellyfin.Controller`/`Jellyfin.Model`), publish a build per ABI promptly, and make the pre-upgrade
check exportable (download the report before upgrading).

## Open questions

- Is there a public source for *upcoming* ABI versions before release (release candidates)?
- Source-repo activity (last commit, open "doesn't work on X" issues) as an abandonment signal —
  needs GitHub API access; optional, off by default.
