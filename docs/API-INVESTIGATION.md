# 调研记录 — 2026-11-12

先阅读旧项目 `my_mod/docs/` 四份记录及其项目规则、交接记录。仅参考流程，不修改旧项目，不继承其 Workshop ID、特性范围、版本号或验收结果。

## 本机依据

- Steam App 4348910，本机 appmanifest build 26525552。
- `Assembly-CSharp.dll` SHA-256：`725CB999D019DFF1FF31BED353C7F9BB8D81A7605A15DE1F80F5A3A6CF7B1D05`，与旧项目调查时相同，因此其只读反编译资料仍对应当前二进制。
- 本机程序集标识 100-Rev13；使用旧项目核实的菜单运行标识 0.46D，并用实际 `ModUIManager.NormalizeVersion` 再次验证匹配。这里只声明本机目标，不把旧日期的 Steam 分支记录当成今天最新版本结论。
- 对照[开发者指南](https://github.com/dragonpan2/probably-stolen-modding/blob/main/ModdingGuide.md)及本地副本：使用原生 IMod、自动 Harmony 特性补丁、空 Capabilities，不随包分发游戏或 Harmony DLL。

## 核实的调用关系（事实描述，不分发反编译源码）

| 位置 | 行为与结论 |
| --- | --- |
| `EmporiumEntry` 初始化 | 主仓库 `invElement` 是 24×10；前后台等容器有独立实例 |
| `EmporiumEntry.SetSmallInv()` | 苦行僧 `ascetic` 为 16×10，否则为 20×10；后者对应极简主义 |
| `EmporiumEntry.SetBigInv()` | 28×10，非默认大小，不能作为本功能的恢复方法 |
| `StartingPerkList.Ascetic()` | ID `ascetic`、cost -4、与 `minimalist` 互斥；本 Mod 均不修改 |
| `StartingPerk.IsAscetic()` | 读取当前 `PlayerStore.startingPerks`，因此不缓存结果 |
| `PlayerStore.StartNewGame()` | 选择苦行僧/极简主义后调用 SetSmallInv |
| `PlayerStore.LoadGame()` | 先加载 PlayerStore（包括 IsPropertyPaid 和 startingPerks），再调用 SetSmallInv，然后才 DecodeSaveItem(mainInvJSON, invElement) |
| `PlayerStore.BuyOutStore()` | 原版处理扣款、统计、IsPropertyPaid=true，随后调用 OnStoreOwned |
| `PlayerStore.TakeMortgage()` | 设置还贷状态并扣首付，不设置 IsPropertyPaid，不应解锁 |
| `PlayerStore.OnMortgagePaidOff()` | IsPropertyPaid=true，再调用 OnStoreOwned，属于付清房产 |
| `PlayerStore.OnStoreOwned()` | 两种付清路径的共同完成入口；没有对应的原生购房通知事件 |
| `GameGridInventory.SetShape(int,int)` | 修改格子形状并校验；不移动物品坐标。扩大范围保留原有坐标 |
| `PlayerStore.DecodeSaveItem()` | 把解码容器内物品放入当前目标仓库，无法放入的对象会走原版清理路径；故读档前必须保持大尺寸，停用前必须清空新增列 |

## 实现选择

1. 在 `SetSmallInv` 前缀中，仅 `Enabled && IsPropertyPaid && IsAscetic()` 时阻止缩仓。若当前为 16×10，则扩为 24×10；初始化时已经是 24×10，无须写入。
2. 在 `OnStoreOwned` 后缀中，以同一条件检查，仅扩大确切的 16×10 主仓库。全款与房贷结清共享此入口，保留所有原版购房操作。
3. 不改 `IsAscetic` 的返回值，避免影响特性其他逻辑；不删除/重写特性，不改价格与点数，不调用 28×10 的 SetBigInv，不补丁系统、Unity、加载器或存档方法。
4. 无逐帧轮询、无额外存档标记、无场景间的解锁缓存。异常只警告一次；前缀读状态失败时允许原版执行。
5. 生命周期停用时不强制缩小已装有物品的仓库。安装、停用和卸载均按重启工作流。

## 验证边界

Release/Debug 实际引用构建与行为替身测试、实际扫描器均通过；具体见 TESTING.md。游戏内视觉、实际购房操作与实际存档往返尚待用户验收。没有启动游戏或操作存档。
