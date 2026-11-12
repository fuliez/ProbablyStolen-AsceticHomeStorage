# Handoff — 2026-11-12

## User intent and project separation

Restore only the ascetic main-storage restriction after the property is fully paid. No perk/points/other-container changes. Reviewed previous my_mod/docs records as workflow references; old runtime/project left unchanged. User chose a public GitHub repository named ProbablyStolen-AsceticHomeStorage. Author fuliezi, owner fuliez.

## Decisions

- Verified installed game SHA matches old investigation; target build 26525552 / 100-Rev13, menu identifier 0.46D.
- Initial investigation briefly mistook SetBigInv(28x10) for normal; corrected after checking initialization and mind-reprogrammer baseline: normal is 24x10. The user was informed before implementation.
- Two narrow patches: SetSmallInv prefix for paid ascetic loads (before item decode); OnStoreOwned postfix for cash purchase and mortgage payoff. Down payment does not qualify.
- Do not change IsAscetic, StartingPerkList, points, money, mortgage, saves or other containers. Never cache an unlock across saves.
- Only expand exact 16x10; leave existing normal/larger dimensions intact. OnDisable does not shrink populated grids; restart restores vanilla rules.
- Explicit uninstall instructions: empty columns 17–24 before disabling, because vanilla decoding can discard items outside its 16x10 grid.
- Independent two-file runtime package; no reused cover, keyboard code, Workshop ID, old configs or old test results.

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
- Remote creation/push status will be verified after committing the reviewed source.

## Still pending

All in-game manual acceptance. No game launch, save edits, release tag, GitHub Release or Workshop upload authorized/performed in this work. Do not claim automated doubles prove actual gameplay.
