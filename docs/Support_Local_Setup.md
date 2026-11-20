# Run SmartSupport locally

Run the commands from the solution root in PowerShell. The local path below uses the isolated SQL Server Express database. The migration and fixture commands explicitly require `SmartSupportDB_Test`.

## Database and accounts

```powershell
$web = "src/SmartSupport-AI.PL/SmartSupport-AI.PL.csproj"
$env:ConnectionStrings__DefaultConnection = 'Server=.\SQLEXPRESS;Database=SmartSupportDB_Test;Integrated Security=True;TrustServerCertificate=True'
dotnet build SmartSupport-AI.slnx
dotnet run --project $web --no-build -- --migrate-support-test

$env:SUPPORT_TEST_CUSTOMER_PASSWORD = Read-Host "New test customer password (12+ characters)" -MaskInput
$env:SUPPORT_TEST_AGENT_PASSWORD = Read-Host "New test agent password (12+ characters)" -MaskInput
$env:SUPPORT_TEST_ADMIN_PASSWORD = Read-Host "New test administrator password (12+ characters)" -MaskInput
dotnet run --project $web --no-build -- --provision-support
Remove-Item Env:SUPPORT_TEST_CUSTOMER_PASSWORD, Env:SUPPORT_TEST_AGENT_PASSWORD, Env:SUPPORT_TEST_ADMIN_PASSWORD
```

Use PowerShell 7 for `Read-Host -MaskInput`. Provisioning creates missing test accounts and keeps existing credentials. The three default emails are `customer@smartsupport.test`, `agent@smartsupport.test`, and `admin@smartsupport.test`. The customer owns sample order `TEST-1001`. If you need a second fixture set, set `SUPPORT_TEST_ACCOUNT_SUFFIX` to an alphanumeric value before provisioning. The value is appended to each email's local part and the order reference, preceded by a hyphen.

## Gemini

The ASP.NET Core web project, `SmartSupport-AI.PL`, reads `GEMINI_API_KEY` from its configuration. Set the variable for the process starting the web app, or use that project's ASP.NET Core User Secrets store. On Windows, the User Secrets file is under `%APPDATA%\Microsoft\UserSecrets\<UserSecretsId>\secrets.json`. Keep the key out of tracked appsettings and browser assets.

To set the key for a PowerShell 7 session, enter it at the masked prompt. If the application process already receives the key securely, keep that configuration.

```powershell
$env:GEMINI_API_KEY = Read-Host "Gemini API key" -MaskInput
```

The `Gemini:AllowCustomerContent` setting defaults to false. Enable it only after approving transmission of customer questions and approved article excerpts to Google under the terms for your account and tier. For a test with synthetic customer questions, set the following in the launch shell.

```powershell
$env:Gemini__AllowCustomerContent = 'true'
dotnet run --project $web --no-build --launch-profile http
```

Restart the application after changing environment variables or User Secrets. VS Code can stay open when you run the commands in the same terminal. Open <http://localhost:5005/Support/Login>. In Development, sign in as the test administrator and click **Verify Gemini connection**. The probe sends a synthetic message and reports whether Gemini returned a response. The configured default is `gemini-3.7-flash`. Your Google AI Studio project determines actual access, quota and billing. Account-specific access was unverified during implementation. [Google Gemini pricing](https://ai.google.dev/gemini-api/docs/pricing/)

## Exercise the workflows

1. Sign in as the customer, start a conversation, ask “Where is order TEST-1001?”, and follow with “that one”. Ask “What is the capital of France?” to check the scope refusal.
2. As the administrator, publish an approved article with a recognizable title and category. In customer chat ask a support question containing those keywords. The customer sees a verified excerpt or the published article.
3. With the agent signed out, ask “Please connect me to a human.” Confirm that the customer sees a queued ticket with a reference. Sign in as the agent in a separate browser profile, wait for **Live updates connected**, then choose **Join** on the ticket. Reply and check the customer's conversation. Subsequent customer messages should go to the human thread.
4. Use a new conversation to test refunds. An administrator must configure `RefundEligibility` from approved business terms. The JSON fields are `maxDaysFromOrder` (positive integer) and `eligibleStatuses` (array of order statuses). For synthetic fixtures only, `{"maxDaysFromOrder":7,"eligibleStatuses":["Delivered"]}` is usable with a newly provisioned sample order. Ask to refund the order, enter the reason, then explicitly confirm. Expect a pending review reference and a support ticket.
5. Save a rating and verify the administrator's aggregate rating changes.

## Verification commands

```powershell
$env:SUPPORT_TEST_SQL = $env:ConnectionStrings__DefaultConnection
dotnet test --no-build
powershell -NoProfile -File tests/Smoke-Support.ps1
```

The SQL suite and smoke test create synthetic records in the isolated database. The smoke test starts its own process on port 5098 and stops that process when it ends. Tests use deterministic or mocked AI responses. The test suite may create a synthetic refund policy if that isolated database has none.

## Live Gemini agent test

Run the following from the solution root after building. It uses the same key and model as the web app and sends only a synthetic warranty question and article. It checks the model tool call and the grounded final reply. The test does not require SQL Server.

```powershell
dotnet run --project src/SmartSupport-AI.PL --no-build --launch-profile http -- --verify-gemini
```

The same test is available at `tests/Test-GeminiLive.ps1`. If your PowerShell execution policy blocks scripts, use the command above. In Development, the app also reads `src/SmartSupport-AI.PL/gemini.secrets.json` with a top-level `GEMINI_API_KEY` entry. An environment variable or User Secrets setting takes precedence. The local file is ignored by Git.
