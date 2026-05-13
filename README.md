# WorkProject

WPF 桌面應用程式，用於規劃專案進度、檢視甘特圖、匯出 PDF 報表，並具備帳號登入與完整操作軌跡 (Audit Log)。

## 技術棧

- Visual Studio 2017
- .NET Framework 4.8
- C# / WPF (MVVM)
- SQL Server LocalDB
- Entity Framework 6.4 Code First
- Unity 5.11 (依賴注入)
- PDFsharp + MigraDoc (PDF 匯出)
- PBKDF2 (`Rfc2898DeriveBytes`) 密碼雜湊
- NUnit + Moq (單元測試)

## 解決方案結構（三層式 + 介面）

```
WorkProject.sln
├── WorkProject.Domain         POCO 實體與 Enum
├── WorkProject.Contracts      所有介面 (Services / Repositories) 與 DTO
├── WorkProject.DataAccess     EF6 DbContext / Repository / Audit
├── WorkProject.Business       BLL 服務實作
├── WorkProject.Infrastructure 密碼雜湊 / PDF 匯出
├── WorkProject.UI             WPF 應用 (Views / ViewModels / 甘特圖控制項)
└── WorkProject.Tests          單元測試
```

相依規則：UI 僅在 `Composition/UnityBootstrapper.cs` 引用具體實作，其餘 View/ViewModel 只認得 Contracts 介面。

## 主要功能

1. **專案規劃**：建立、編輯、刪除專案，加入成員 (Owner/Manager/Contributor/Viewer)。
2. **任務管理**：建立任務、子任務、里程碑、相依關係、進度更新、留言、附件。
3. **甘特圖**：自製 WPF 控制項 (`Controls/Gantt`)，以 `ItemsControl + Canvas` 搭配附加屬性將日期轉換為像素位置；支援縮放、時間軸標題、捲軸同步。
4. **PDF 匯出**：封面、摘要、成員、任務表，可選擇加入甘特圖快照頁。
5. **帳號登入**：PBKDF2 加鹽 100k 次雜湊；登入失敗也會留下 Audit。
6. **操作軌跡 (Audit)**：覆寫 `DbContext.SaveChanges` 自動寫入 `AuditLogs`，含 Before/After JSON 與 CorrelationId；UI 提供過濾查詢。

## 第一次執行

1. 在 Visual Studio 2017 開啟 `WorkProject.sln`，還原 NuGet 套件 (EntityFramework, PDFsharp-MigraDoc, Unity, NUnit, Moq)。
2. 確認本機已安裝 SQL Server LocalDB：`SqlLocalDB info` 應列出 `MSSQLLocalDB`。
3. 將 `WorkProject.UI` 設為啟動專案，按 F5。
4. 首次執行會自動建立資料庫並建立預設使用者：
   - 帳號：`admin`
   - 密碼：`Admin#12345`

## 預設連線字串

於 `WorkProject.UI/App.config`：

```
Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WorkProjectDb;Integrated Security=True;MultipleActiveResultSets=True
```

## 驗證流程

請參考 `/.claude/plans/visual-studio-2017-net-stateless-lake.md` 中「驗證 (End-to-End)」章節，自登入、建立專案、任務、相依、檢視甘特圖、匯出 PDF 到 Audit 查詢完整測試。
