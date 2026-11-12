# Independent project rules

Read README.md, agent.md and docs before editing. This folder is the new project root; the parent is a game installation. Do not edit my_mod or import its runtime patches, Workshop identity or acceptance claims.

- Scope: only ascetic main inventory after IsPropertyPaid. Normal is 24x10, NOT SetBigInv's 28x10. Preserve perk selection/cost, non-ascetic behavior and other inventories.
- Use native IMod and automatic Harmony attributes. Capabilities stay empty; never bundle game or Harmony DLLs or patch Unity/system/loader/trust code.
- Verify APIs and load order against installed binaries. Never overwrite game assemblies or edit saves. Never launch save-mutating gameplay without an explicit user request.
- Keep game files, decompilation, logs, saves, credentials, local configuration and artifacts out of Git. Runtime allowlist: manifest.xml and ProbablyStolen.AsceticHomeStorage.dll.
- Run Release/Debug contracts, script checks, actual-reference builds, actual scanner and packaging as appropriate. Record manual acceptance honestly as pending until observed/reported.
- Warn about real uninstall consequences: move items out of added columns before restoring vanilla dimensions. Do not shrink populated storage during OnDisable.
- Author credit fuliezi; GitHub owner fuliez. Keep bilingual README facts and commands aligned.
- The owner authorized the public ProbablyStolen-AsceticHomeStorage source repository on 2026-11-12. Release publication and Workshop uploads are separate tasks. Never reuse the old Workshop ID.
- Record decisions, verification and open work in agent.md. No release tags before in-game acceptance.
