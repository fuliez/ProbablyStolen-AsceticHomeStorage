# Hardship Rewarded - Ascetic Storage Restored After Home Purchase

**苦尽甘来-苦行僧购房后仓库恢复**

[简体中文](README.md) | [English](README.en.md)

An independent Probably Stolen mod by **fuliezi**. Only when an ascetic character has fully paid for the property, restore main storage from **16×10 (160 cells) to the normal 24×10 (240 cells)**.

- Applies immediately after an outright purchase or mortgage payoff, and when loading an existing fully paid save.
- Keeps the ascetic restriction before purchase, after a down payment, and while paying a mortgage.
- Retains the ascetic perk, its -4 point cost, perk selection and other effects. Minimalist characters, characters without ascetic, other containers, cash, rent and mortgage behavior remain vanilla.
- Does not remove perks, overwrite game DLLs, directly read/write saves or add mod save fields. No prerequisite mods or additional capabilities.
- Independent folder, Mod ID, assembly, Git history and repository from Drag-Aware Hotkeys; no keyboard patches are included.

Version **0.1.1**: the owner tested and accepted restoring normal storage after an ascetic character purchases the property, and tested item behavior when disabling and re-enabling the mod, as described below. These are owner-reported results; individual branches not explicitly reported remain on the checklist. Local target: 100-Rev13 / Steam build 26525552, menu identifier 0.46D. Other versions are untested. Normal storage is 24×10; the 28×10 `SetBigInv()` method is a separate expansion.

## Build and install

Subscribe, update and provide feedback through [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3838428231). After subscribing, enable the mod in-game and restart. The following commands are for building and installing locally.

Requires a .NET SDK (currently 6.0.410), PowerShell and a legitimate game installation. SDK 6 is out of support; it is a development tool and is not distributed with this mod.

```powershell
$env:PROBABLY_STOLEN_GAME_DIR = (Resolve-Path '..').Path # when the project is inside the game folder
./build.ps1
./install-local.ps1
./package.ps1
```

Alternatively pass `-GameDir`, or copy `config.example.json` to ignored `config.local.json`. Building and packaging do not install automatically. The installer requires the game to be closed and writes only to `Mods/ascetic_home_storage/`.

The package is `artifacts/ProbablyStolen-AsceticHomeStorage-0.1.1.zip`, containing only `manifest.xml`, `ProbablyStolen.AsceticHomeStorage.dll` and `preview.png` under `ascetic_home_storage/`. Extract into the game's `Mods/` folder, enable in the Mods menu, inspect permissions and restart. An original in-game cover is included. Do not copy the source repository into Mods.

## Disable and save considerations

The mod does not directly edit saves, but items in the added area are saved by the game normally. **Before disabling or uninstalling, move items from columns 17–24 into the original 16×10 area or other containers, then save and exit.** Back up saves before updating the game or changing mods.

Owner-reported test on 2026-11-12: after disabling, items entirely in the added area disappeared from view. Partially overlapping items remained at the vanilla boundary and could be retrieved, but could not be placed back outside the vanilla grid. After re-enabling, the tested items were still in the added area. This does not establish permanent deletion or guarantee recovery across future game versions or every save operation performed while disabled. Avoid overwriting your only save if storage behaves abnormally.

Disable and restart, or close the game and remove `Mods/ascetic_home_storage/`, to restore vanilla size rules. Disabling during play does not forcibly shrink populated storage; restart to reconstruct vanilla dimensions.

## Checks and implementation

```powershell
dotnet run --project tests/Contracts/Contracts.csproj -c Release
dotnet run --project tests/Contracts/Contracts.csproj -c Debug
./scripts/check.ps1
./scripts/check-game.ps1
./tests/Workshop.ps1
```

Only two game methods are patched: a conditional prefix on `EmporiumEntry.SetSmallInv` prevents shrinking fully paid ascetic storage before item decoding; a postfix on `PlayerStore.OnStoreOwned` expands the original 16×10 grid at ownership completion. Only an exact 16×10 grid is expanded to 24×10; larger layouts are never reduced. No per-frame polling or cross-save unlock cache.

See [investigation](docs/API-INVESTIGATION.md), [acceptance checklist](docs/TESTING.md), [release procedure](docs/RELEASING.md) and [handoff](agent.md). Public CI only checks behavior with handwritten doubles and repository policy. Real builds and scanning require local game references. Automated checks do not replace gameplay acceptance.

## Source and license

[GitHub source](https://github.com/fuliez/ProbablyStolen-AsceticHomeStorage), MIT license. Game assemblies, decompiled source, saves, logs and machine configuration are excluded from the repository and package. Packages are available through [GitHub Releases](https://github.com/fuliez/ProbablyStolen-AsceticHomeStorage/releases); subscriptions and feedback use [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3838428231).
