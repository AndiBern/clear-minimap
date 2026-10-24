# Building

Requires the .NET 8 SDK, Rust, Schedule I with MelonLoader, and ModSettings
0.3.0. The mod targets .NET 6. Tests run on .NET 8.

Launch the game with MelonLoader once to generate references, then set:

```sh
export MelonLoaderDir='/path/to/profile/MelonLoader'
export ModSettingsDll='/path/to/ModSettings.dll'
```

The loader directory must contain `net6/` and `Il2CppAssemblies/`. In
PowerShell, set `$env:MelonLoaderDir` and `$env:ModSettingsDll` instead. On Nix,
enter `nix-shell -p dotnet-sdk_8 cargo rustc` first.

## Build and test

```console
dotnet build ClearMinimap.csproj --configuration Release
dotnet run --project tests/Tests.csproj
cargo test --locked
```

Copy `bin/Release/net6.0/ClearMinimap.dll` into your test profile's `Mods/`
directory, then restart the game. Install ModSettings separately.

The standalone tests cover projection, zoom, placement, clipping, heading, and
motion. Check these in-game:

- Compare landmarks and NPC icons with the phone map, including moving NPCs and
  hidden icons. Test before and after opening the phone map.
- Change zoom, size, corner, and margins. Check map and marker alignment,
  teammate edge pointers, and room for the clock.
- Rebind and disable zoom shortcuts. Check persistence after restarting and
  ensure shortcuts do nothing while settings, the phone, or pause menu is open.
- Check markers and visibility through multiplayer joins and scene changes.

Game assemblies and assets must not be distributed.

## Format and verify interop

With both environment variables set:

```console
dotnet format ClearMinimap.csproj
dotnet format tests/Tests.csproj
cargo fmt
```

After building, run the native API check. In PowerShell, use
`$env:MelonLoaderDir` in these arguments:

```sh
dotnet run --project verification/InteropCheck.csproj "-p:MelonLoaderDir=$MelonLoaderDir" -- bin/Release/net6.0/ClearMinimap.dll "$MelonLoaderDir/Il2CppAssemblies"
```

## Package and release

Copy the Release DLL to `package/ClearMinimap.dll`, then run:

```console
cargo run --locked -- metadata
dotnet tool restore
dotnet tool run tcli build --config-path dist/thunderstore.toml
cargo run --locked -- package
```

The validated ZIP is written to `dist/holyfurries-ClearMinimap-<version>.zip`.
The studio icon ships with `icon-LICENSE`.

Update the changelog and align versions in `ClearMinimap.csproj`,
`package/manifest.json`, and `MelonInfo`. Commit source, metadata, and the
refreshed DLL together.

**Pushing a matching `vX.Y.Z` tag publishes to Thunderstore automatically.**
Main-branch pushes produce package artifacts. CI packages the committed DLL
without compiling against game references. Publishing requires the
`TCLI_AUTH_TOKEN` Actions secret.
