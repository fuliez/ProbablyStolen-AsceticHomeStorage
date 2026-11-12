# 苦尽甘来-苦行僧购房后仓库恢复

**Hardship Rewarded - Ascetic Storage Restored After Home Purchase**

[简体中文](README.md) | [English](README.en.md)

《深空当铺 / Probably Stolen》的独立 Mod，作者 **fuliezi**。仅当角色拥有“苦行僧”特性且房产已付清时，把主仓库从 **16×10（160 格）恢复到正常的 24×10（240 格）**。

- 全款购房、还清房贷时立即生效；已有已购房存档读档时也生效。
- 未购房、仅付房贷首付或仍在还贷时，保留苦行僧限制。
- 保留苦行僧特性本身、其 -4 点价格、特性选择和其他效果。极简主义、无苦行僧角色、其他容器、现金、租金和房贷逻辑均由原版处理。
- 不删除特性、不修改原始游戏 DLL、不直接读写存档、不增加 Mod 存档字段，无依赖 Mod，无额外能力权限。
- 与 Drag-Aware Hotkeys 使用独立目录、Mod ID、程序集、Git 历史和仓库，不包含按键补丁。

当前版本 **0.1.1**：所有者已实测并确认苦行僧购房后恢复正常仓库的功能符合预期，也测试了停用及重新启用时的物品表现，详见下文。这是用户报告的验收结果；未逐项报告的分支测试仍见清单。本机目标为 100-Rev13 / Steam build 26525552，菜单标识 0.46D；不宣称其他版本已测试。正常大小为 24×10，`SetBigInv()` 的 28×10 属于另一个扩仓方法。

## 构建与安装

推荐通过 [Steam 创意工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=3838428231)订阅、更新和反馈。订阅后在游戏 Mods 菜单启用并重启。下面的命令用于自行构建和本地安装。

需要 .NET SDK（当前使用 6.0.410）、PowerShell 和合法安装的游戏。SDK 6 已停止支持；它是开发工具，不随 Mod 分发。

```powershell
$env:PROBABLY_STOLEN_GAME_DIR = (Resolve-Path '..').Path # 项目放在游戏目录下时
./build.ps1
./install-local.ps1
./package.ps1
```

也可对脚本传入 `-GameDir`，或复制 `config.example.json` 为被忽略的 `config.local.json`。构建与打包不会自动安装；安装脚本要求游戏已退出，只写入 `Mods/ascetic_home_storage/`。

安装包位于 `artifacts/ProbablyStolen-AsceticHomeStorage-0.1.1.zip`，仅包含 `ascetic_home_storage/` 下的 `manifest.xml`、`ProbablyStolen.AsceticHomeStorage.dll` 和 `preview.png`。解压到游戏 `Mods/`，在游戏 Mods 菜单启用、检查权限并重启。包含独立的游戏内封面。请勿把整个源码仓库复制到 Mods。

## 停用与存档注意事项

本 Mod 不直接编辑存档，但新增区域里的物品会通过游戏正常保存。**停用或卸载前，请把主仓库第 17～24 列的物品移入原本 16×10 范围或其他容器，再保存并退出。** 建议更新游戏或变更 Mod 前先备份存档。

2026-11-12 所有者实测：停用后，完全放在额外区域中的物品消失于当前视图；仅一部分跨出原有范围的物品会卡在仓库边界，但仍可正常取出，取出后不能放回额外空间；重新启用 Mod 后，测试中的物品仍在额外空间中。该结果不能解释为永久删除，也不保证未来游戏版本或所有停用后保存操作都能恢复。请避免在仓库异常时覆盖唯一存档。

停用并重启，或退出游戏后移除 `Mods/ascetic_home_storage/`，即可恢复原版尺寸规则。运行中停用不会强行缩小已装有物品的仓库；以重启后的状态为准。

## 验证与实现

```powershell
dotnet run --project tests/Contracts/Contracts.csproj -c Release
dotnet run --project tests/Contracts/Contracts.csproj -c Debug
./scripts/check.ps1
./scripts/check-game.ps1
./tests/Workshop.ps1
```

仅两个游戏方法补丁：`EmporiumEntry.SetSmallInv` 的条件前缀，避免已购房苦行僧在物品解码前缩仓；`PlayerStore.OnStoreOwned` 的后缀，在购房完成时扩大原本的 16×10 主仓库。仅将确切的 16×10 扩为 24×10，不缩小已有更大尺寸。无需逐帧检测，也不缓存跨存档的解锁状态。

详见[调研记录](docs/API-INVESTIGATION.md)、[验收清单](docs/TESTING.md)、[发布流程](docs/RELEASING.md)和[交接记录](agent.md)。公开 CI 只验证手写替身的行为检查与仓库规则；实际游戏构建和扫描须使用本机游戏引用，自动检查不能代替游戏内验收。

## 源码与许可

[GitHub 源码](https://github.com/fuliez/ProbablyStolen-AsceticHomeStorage)，MIT 许可。游戏程序集、反编译源码、存档、日志和本机配置不进入仓库或安装包。发布文件见 [GitHub Releases](https://github.com/fuliez/ProbablyStolen-AsceticHomeStorage/releases)，订阅与反馈见 [Steam 创意工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=3838428231)。
