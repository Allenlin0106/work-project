# 專案進度規劃系統

A project planning web application built for **Visual Studio 2017 / .NET Framework 4.8 / C#**.

## 功能

1. 規劃專案進度（建立、編輯、刪除專案與任務、相依關係）
2. 甘特圖檢視（自行繪製 SVG，支援今日線與相依箭頭）
3. 匯出進度資訊為 PDF（Rotativa / HTML→PDF）
4. 專案成員可加入專案內容（任務、留言）
5. 軌跡查詢（登入/登出、CRUD 變更含前後值、檢視/匯出）

## 架構

三層式架構搭配介面與 Autofac 依賴注入：

| 專案 | 角色 |
|---|---|
| `ProjectPlanning.Entities` | POCO 實體、列舉、DTO |
| `ProjectPlanning.Common` | 共用工具（Audit JSON 設定） |
| `ProjectPlanning.DAL` | EF6 DbContext、Repository、Migrations、SaveChanges 軌跡攔截 |
| `ProjectPlanning.BLL` | 服務介面與實作 |
| `ProjectPlanning.Web` | ASP.NET MVC 5 + Autofac + Windows 認證 + Rotativa |
| `ProjectPlanning.Tests` | MSTest 單元測試 |

## 開發環境

- Visual Studio 2017 (15.x)
- .NET Framework 4.8 Targeting Pack
- SQL Server LocalDB
- IIS Express（啟用 Windows 驗證、停用匿名）

## 首次執行步驟

1. 以 VS2017 開啟 `ProjectPlanning.sln`，等待自動還原 NuGet 套件
2. 將 `ProjectPlanning.Web` 設為啟動專案
3. 在 IIS Express／專案屬性 Web 分頁：Windows Authentication = Enabled、Anonymous Authentication = Disabled
4. Package Manager Console（預設專案選 `ProjectPlanning.DAL`，啟動專案選 `ProjectPlanning.Web`）：
   ```powershell
   Add-Migration Initial
   Update-Database
   ```
5. F5 啟動，瀏覽器自動以 Windows 身份進入專案列表
6. 第一次匯出 PDF 前確認 `ProjectPlanning.Web\Rotativa\wkhtmltopdf.exe` 已隨 NuGet 安裝且 **Copy to Output Directory = Copy if newer**

## Smoke Test

| 功能 | 步驟 | 預期結果 |
|---|---|---|
| 專案 CRUD | 建立 → 編輯 → 刪除 | AuditLog 出現 3 筆，Update 包含 Before/After JSON |
| 任務 / 相依 | 建立任務並設定 FS 相依 | 甘特圖出現相依箭頭 |
| 甘特圖 | `/Gantt/Index?projectId=1` | SVG 正確繪製 Bar、今日線、軸標 |
| PDF 匯出 | `/Export/ProjectPdf/1` | A3 橫向 PDF 下載；多一筆 Export AuditLog |
| 成員 | 加入另一帳號為 Contributor | 該帳號登入後看得到此專案 |
| 軌跡查詢 | `/AuditLogs` 篩選 | 結果正確並可展開 JSON 差異 |

## 規劃文件

詳見實作計畫：`/root/.claude/plans/visual-studio-2017-net-cuddly-lobster.md`
