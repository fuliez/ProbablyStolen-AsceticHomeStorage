# Independent project rules

Read README.md, agent.md and docs before editing. This folder is the new project root; the parent is a game installation. Do not edit my_mod or import its runtime patches, Workshop identity or acceptance claims.

- Scope: only ascetic main inventory after IsPropertyPaid. Normal is 24x10, NOT SetBigInv's 28x10. Preserve perk selection/cost, non-ascetic behavior and other inventories.
- Use native IMod and automatic Harmony attributes. Capabilities stay empty; never bundle game or Harmony DLLs or patch Unity/system/loader/trust code.
- Verify APIs and load order against installed binaries. Never overwrite game assemblies or edit saves. Never launch save-mutating gameplay without an explicit user request.
- Keep game files, decompilation, logs, saves, credentials, local configuration and artifacts out of Git. Runtime allowlist: manifest.xml, ProbablyStolen.AsceticHomeStorage.dll and preview.png. Accept the exact older two-file install only for migration.
- Run Release/Debug contracts, script checks, actual-reference builds, actual scanner and packaging as appropriate. Record manual acceptance honestly as pending until observed/reported.
- Warn about real uninstall consequences: move items out of added columns before restoring vanilla dimensions. Do not shrink populated storage during OnDisable.
- Author credit fuliezi; GitHub owner fuliez. Keep bilingual README facts and commands aligned.
- The owner authorized the public ProbablyStolen-AsceticHomeStorage source repository on 2026-11-12, then reported successful gameplay acceptance and requested release to Steam Workshop with the bilingual Hardship Rewarded name. This authorizes GitHub Release and public Workshop delivery. Create hidden first, then publish the same new item publicly after checking content. Never reuse the old mod's Workshop ID.
- This project's item is now 3838428231 and configured Public in workshop/item.json. Preserve this identity/visibility on updates; never silently create a replacement. Runtime 0.1.1 is released; keep its tag and assets immutable.
- Record owner-reported observations precisely: fully outside items disappear from view while disabled, partially outside items can be retrieved but cannot be replaced outside the vanilla grid, and re-enabling restored the tested outside items. Do not claim permanent deletion or guaranteed recovery across future versions or untested save sequences.
- Record decisions, verification and open work in agent.md. No release tags before in-game acceptance.
