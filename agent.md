# Handoff — 2026-11-12

## User intent and project separation

Restore only the ascetic main-storage restriction after the property is fully paid. No perk/points/other-container changes. Reviewed previous my_mod/docs records as workflow references; old runtime/project left unchanged. User chose a public GitHub repository named ProbablyStolen-AsceticHomeStorage. Author fuliezi, owner fuliez.

## Decisions

- Verified installed game SHA matches old investigation; target build 26525552 / 100-Rev13, menu identifier 0.46D.
- Initial investigation briefly mistook SetBigInv(28x10) for normal; corrected after checking initialization and mind-reprogrammer baseline: normal is 24x10. The user was informed before implementation.
- Two narrow patches: SetSmallInv prefix for paid ascetic loads (before item decode); OnStoreOwned postfix for cash purchase and mortgage payoff. Down payment does not qualify.
- Do not change IsAscetic, StartingPerkList, points, money, mortgage, saves or other containers. Never cache an unlock across saves.
- Only expand exact 16x10; leave existing normal/larger dimensions intact. OnDisable does not shrink populated grids; restart restores vanilla rules.
- Explicit uninstall instructions: empty columns 17–24 before disabling. Initial inference of automatic out-of-bounds deletion was unsupported; see the later owner report below.
- Initial independent two-file preview package; superseded by the three-file 0.1.1 package below. No reused keyboard code, Workshop ID, old configs or old test results.

## Verified so far

- Actual-reference Release/Debug builds: 0 warnings/errors.
- Both configurations: 17 contract behavior checks passed.
- Real game API signatures and actual 0.46D normalizer: passed.
- Real scanner: 18 methods, no refusals/capabilities/hard flags/errors.
- Harness development encountered a game namespace named Directory and a mistaken reflection field/property query. Fixed using System.IO.Directory and GetProperty; final harness passes. These were harness issues, not runtime changes.
- Script syntax/manifest/tracked-file policy passed. Empty XML Capabilities is checked via SelectNodes rather than PowerShell's empty-string XML adapter.
- Release clean Rebuild produced identical DLL SHA-256: 0C966E3CD887F633E2B733DB45033FB404C454773EDEBFF1791E9D94A171BEAA.
- ZIP contains exactly the independent DLL and manifest; both entry hashes match the local installation.
- Installed with game closed to Mods/ascetic_home_storage. Original my_mod git status remains clean.
- Public repository created and main pushed: https://github.com/fuliez/ProbablyStolen-AsceticHomeStorage . Remote API verified PUBLIC and main head matched local commit 8f59d0e00601274666257f7b1860258d7dbfc71d.
- Initial GitHub CI passed both contract configurations and script/repository policy: https://github.com/fuliez/ProbablyStolen-AsceticHomeStorage/actions/runs/43171146383 . Existing toolchain emits .NET 6 support and old action-runtime notices; tests succeed. Future maintenance can upgrade the development toolchain independently of the netstandard2.1 runtime.
- Script checks also passed under Windows PowerShell 5.1. Final documentation-only verification commit skips duplicate CI; no production/build/test logic changed after the successful run.

## Initial status (superseded by the owner report below)

All in-game manual acceptance. No game launch, save edits, release tag, GitHub Release or Workshop upload authorized/performed in this work. Do not claim automated doubles prove actual gameplay.

## Release request and owner-reported acceptance — 2026-11-12

- Owner confirms the mod works and satisfies restoring normal ascetic storage after purchasing the property. They also accepted disabling behavior: entirely outside items disappear from view; partly outside items remain at the boundary and can be retrieved but not replaced outside; re-enabling restores the tested outside items. Do not turn these observations into permanent-loss claims or guarantees for future versions or untested saves while disabled.
- Owner requests Steam Workshop publication and bilingual name: 苦尽甘来-苦行僧购房后仓库恢复 / Hardship Rewarded - Ascetic Storage Restored After Home Purchase. User request authorizes public delivery, with first creation hidden followed by updating the same new item to public. No further publication permission needed.
- Metadata/artwork/tooling version 0.1.1; production patch source unchanged from accepted preview. Mod ID, assembly and repository remain stable.
- Original built-in imagegen cover created for this mod; saved to workshop/cover.jpg and mod/preview.png. Full prompt and provenance in workshop/ARTWORK.md. JPEG 1024-square/330759 bytes, native PNG 1024-square. Package now exactly DLL + manifest + preview; installer permits the exact older two-file layout for migration.
- Adapted generic publisher and offline safeguards from the old project, replacing project paths and source links; never copied its item identity or account configuration. Reuse of the existing SteamCMD executable is development tooling only. Account/password/Guard details are not tracked.
- Fresh real builds, 17 contracts per configuration, actual signatures/version/scanner, PS7/5.1 offline publisher and package/install hash checks passed. Clean Release Rebuild SHA256: 342974F0BDC86BF381FE366E920158A1C1B0A90A4C3121974D1C0236ED366FF2.
- Installed new metadata/cover while game was closed. Updated menu appearance awaits observation; no AI gameplay or save edits.
- Steam first creation returned new ID 3838428231, Hidden. Anonymous metadata initially returned result=9 as expected for hidden content. Updated the SAME ID to Public, then anonymous metadata verified result=1, visibility=0, app=4348910, exact bilingual title, exact Chinese-first/English-second description and a preview URL.
- Workshop URL: https://steamcommunity.com/sharedfiles/filedetails/?id=3838428231 . Identity and intended Public visibility are tracked in workshop/item.json; future uploads update it without NewItem. Never replace it or use the keyboard mod's item.
- SteamCMD reused local cached login and handled its own update; no password/Guard input or credentials were stored by this project. Raw metadata remains ignored locally.
- GitHub Release / final CI verification will be recorded after delivery.
