# 发布流程

这是独立项目；不使用旧按键 Mod 的仓库、Workshop ID 或账户配置。发布脚本基于旧项目已验证的通用流程适配，项目路径和身份独立。

1. 运行两种配置的行为检查、`scripts/check.ps1`、`scripts/check-game.ps1 -GameDir ...`，并完成真实引用 Release/Debug 构建。
2. 用 `package.ps1` 产生仅含 Mod DLL、manifest 与 preview.png 的 ZIP，核对条目及 SHA256SUMS.txt；禁止打包源码、游戏 DLL、Harmony、本机配置、日志、存档或反编译材料。
3. 游戏退出后使用 `install-local.ps1`；按 TESTING.md 在备份测试存档上完成验收。不要自动启动会保存进度的游戏测试。
4. 将实际验收结果写入 agent.md / TESTING.md；未观察的项目保持待验收。
5. 版本采用 SemVer，Directory.Build.props / manifest / CHANGELOG 同步。提交前审查暂存文件与 diff --check。
6. 通过用户报告的游戏内验收且用户授权后，创建 annotated tag 和 GitHub Release；本次用户已报告核心验收符合预期，并明确请求本 Mod 发布。
7. 第一次新项目上传使用隐藏可见性，保存 Steam 返回的本项目 ID，然后对同一项目更新为 Public，完成用户要求的公开发布；不借用旧项目的 3837800051。凭据只通过 SteamCMD 本机交互处理，不记录账号信息、密码或验证码。

## 创意工坊工具

`publish-workshop.ps1` 默认只生成准备文件，不上传。首先读取本项目 `workshop/item.json`，再核对忽略的本地 state / VDF；冲突时停止。首次创建必须为 Hidden 且使用 NewItem；不确定的首次上传禁止盲目重试。描述中文优先、英文对应，只在源码部分链接仓库，VDF 保留真实换行。

```powershell
./publish-workshop.ps1 -PreviewFile ./workshop/cover.jpg
./publish-workshop.ps1 -Upload -NewItem -Visibility Hidden -ConfirmUpload 'UPLOAD HIDDEN' -SteamUser YOUR_ACCOUNT -PreviewFile ./workshop/cover.jpg
# 返回 ID 后跟踪到 workshop/item.json；后续只更新同一项目
./publish-workshop.ps1 -Upload -Visibility Public -ConfirmUpload 'UPLOAD PUBLIC' -SteamUser YOUR_ACCOUNT -PreviewFile ./workshop/cover.jpg
```

SteamCMD 路径通过环境变量 STEAMCMD_PATH、参数或忽略的 config.local.json 提供。绝不把账号信息或凭据放入仓库。上传后核对 Steam 返回 ID、匿名 metadata 的 visibility=0、标题、描述和 preview_url；SteamCMD 退出成功本身不代表公开验证通过。

源代码仓库公开，名称 ProbablyStolen-AsceticHomeStorage。首次发行版本 0.1.1，具体发布结果记录于 agent.md。
