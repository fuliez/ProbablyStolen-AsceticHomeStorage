# 发布流程

这是独立项目；不使用旧按键 Mod 的仓库、Workshop ID、配置或发布脚本。

1. 运行两种配置的行为检查、`scripts/check.ps1`、`scripts/check-game.ps1 -GameDir ...`，并完成真实引用 Release/Debug 构建。
2. 用 `package.ps1` 产生仅含 Mod DLL 与 manifest 的 ZIP，核对条目及 SHA256SUMS.txt；禁止打包源码、游戏 DLL、Harmony、本机配置、日志、存档或反编译材料。
3. 游戏退出后使用 `install-local.ps1`；按 TESTING.md 在备份测试存档上完成验收。不要自动启动会保存进度的游戏测试。
4. 将实际验收结果写入 agent.md / TESTING.md；未观察的项目保持待验收。
5. 版本采用 SemVer，Directory.Build.props / manifest / CHANGELOG 同步。提交前审查暂存文件与 diff --check。
6. 通过游戏内验收且用户授权后才创建 annotated tag 和 GitHub Release；本次仅授权创建公开源码仓库，不代指正式发行或 Workshop 上传。
7. 如未来要求创意工坊，另行建立本项目身份与上传流程；第一次新项目上传使用隐藏可见性，凭据保持交互输入，不借用旧项目的 3837800051。

源代码仓库：用户于 2026-11-12 选择公开可见性，名称 ProbablyStolen-AsceticHomeStorage。尚无正式发行或 Workshop 项目。
