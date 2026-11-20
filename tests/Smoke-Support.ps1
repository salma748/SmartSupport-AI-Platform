param([int]$Port = 5098)
$ErrorActionPreference = 'Stop'

$databaseName = [System.Data.SqlClient.SqlConnectionStringBuilder]::new($env:SUPPORT_TEST_SQL).InitialCatalog
if ($databaseName -ne 'SmartSupportDB_Test') { throw 'SUPPORT_TEST_SQL must target SmartSupportDB_Test.' }

$project = Join-Path $PSScriptRoot '../src/SmartSupport-AI.PL'
$dll = Join-Path $project 'bin/Debug/net10.0/SmartSupport-AI.PL.dll'
$baseUrl = "http://localhost:$Port"
$env:ASPNETCORE_ENVIRONMENT = 'Development'
$env:ConnectionStrings__DefaultConnection = $env:SUPPORT_TEST_SQL
$env:Gemini__AllowCustomerContent = 'false'
$env:Logging__LogLevel__Microsoft = 'Warning'
$env:SUPPORT_TEST_ACCOUNT_SUFFIX = [Guid]::NewGuid().ToString('N')
$env:SUPPORT_TEST_CUSTOMER_PASSWORD = [Guid]::NewGuid().ToString('N')
$env:SUPPORT_TEST_AGENT_PASSWORD = [Guid]::NewGuid().ToString('N')
$env:SUPPORT_TEST_ADMIN_PASSWORD = [Guid]::NewGuid().ToString('N')

function Assert([bool]$Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
}

function Token([string]$Html) {
    $match = [regex]::Match($Html, 'name="__RequestVerificationToken"[^>]*value="([^"]+)"')
    Assert $match.Success 'Expected antiforgery token in Razor output.'
    $match.Groups[1].Value
}

function Login([string]$Role, [string]$Password) {
    $session = New-Object Microsoft.PowerShell.Commands.WebRequestSession
    $first = Invoke-WebRequest -UseBasicParsing "$baseUrl/Support/Login" -WebSession $session
    $loginToken = Token $first.Content
    $email = "$Role-$($env:SUPPORT_TEST_ACCOUNT_SUFFIX)@smartsupport.test"
    $page = Invoke-WebRequest -UseBasicParsing "$baseUrl/Support/Login" -Method Post -WebSession $session -Body @{
        email = $email; password = $Password; __RequestVerificationToken = $loginToken
    }
    @{ Session = $session; Token = (Token $page.Content); Page = $page.Content }
}

function Post($Actor, [string]$Action, [hashtable]$Body) {
    Invoke-RestMethod "$baseUrl/Support/$Action" -Method Post -WebSession $Actor.Session -Headers @{
        RequestVerificationToken = $Actor.Token
    } -Body $Body
}

function Open-Live($Actor) {
    $socket = [System.Net.WebSockets.ClientWebSocket]::new()
    $socket.Options.Cookies = $Actor.Session.Cookies
    $deadline = [System.Threading.CancellationTokenSource]::new(10000)
    try {
        $null = $socket.ConnectAsync([Uri]"ws://localhost:$Port/support-live", $deadline.Token).GetAwaiter().GetResult()
        $bytes = [Text.Encoding]::UTF8.GetBytes('{"protocol":"json","version":1}' + [char]30)
        $null = $socket.SendAsync([ArraySegment[byte]]::new($bytes), [System.Net.WebSockets.WebSocketMessageType]::Text,
            $true, $deadline.Token).GetAwaiter().GetResult()
        $buffer = [byte[]]::new(1024)
        $frame = $socket.ReceiveAsync([ArraySegment[byte]]::new($buffer), $deadline.Token).GetAwaiter().GetResult()
        Assert ([Text.Encoding]::UTF8.GetString($buffer, 0, $frame.Count) -eq ('{}' + [char]30)) 'SignalR handshake failed.'
        return $socket
    }
    finally { $deadline.Dispose() }
}

Push-Location $project
try {
    dotnet $dll --provision-support
    Assert ($LASTEXITCODE -eq 0) 'Could not provision isolated smoke-test identities.'
    $server = Start-Process dotnet -ArgumentList @('"' + $dll + '"', '--urls', $baseUrl) -PassThru -WindowStyle Hidden
    try {
        $ready = $false
        for ($attempt = 0; $attempt -lt 20; $attempt++) {
            try {
                $health = Invoke-WebRequest -UseBasicParsing "$baseUrl/Support/Login" -TimeoutSec 2
                if ($health.StatusCode -eq 200) { $ready = $true; break }
            }
            catch { Start-Sleep -Milliseconds 300 }
        }
        Assert $ready 'Support host did not start.'
        $customer = Login 'customer' $env:SUPPORT_TEST_CUSTOMER_PASSWORD
        Assert ($customer.Page -match 'New conversation') 'Customer Razor screen was unavailable.'
        $agent = Login 'agent' $env:SUPPORT_TEST_AGENT_PASSWORD
        Assert ($agent.Page -match 'Agent workspace') 'Agent Razor screen was unavailable.'
        $admin = Login 'admin' $env:SUPPORT_TEST_ADMIN_PASSWORD
        Assert ($admin.Page -match 'Support administration') 'Admin Razor screen was unavailable.'

        $articleTitle = 'Warranty coverage ' + $env:SUPPORT_TEST_ACCOUNT_SUFFIX
        $articleBody = 'Synthetic test policy. This warranty fixture is for the isolated test database.'
        $null = Invoke-WebRequest -UseBasicParsing "$baseUrl/SupportAdmin/Article" -Method Post -WebSession $admin.Session -Body @{
            __RequestVerificationToken = $admin.Token; title = $articleTitle; category = 'Smoke fixture';
            content = $articleBody; publish = 'true'
        }
        $id = (Post $customer Start @{}).id
        $null = Post $customer Send @{ id = $id; message = "What is the warranty policy $($env:SUPPORT_TEST_ACCOUNT_SUFFIX)?" }
        $thread = Invoke-RestMethod "$baseUrl/Support/Thread?id=$id" -WebSession $customer.Session
        Assert ($thread.messages[-1].text.Contains($articleBody)) 'Published knowledge retrieval failed.'
        $orderNumber = "TEST-1001-$($env:SUPPORT_TEST_ACCOUNT_SUFFIX)"
        $null = Post $customer Send @{ id = $id; message = "Where is my order ${orderNumber}?" }
        $null = Post $customer Send @{ id = $id; message = 'that one' }
        $thread = Invoke-RestMethod "$baseUrl/Support/Thread?id=$id" -WebSession $customer.Session
        Assert ($thread.messages[-1].text -match 'Delivered') 'Order follow-up failed.'
        $null = Post $customer Send @{ id = $id; message = 'I want to talk to a human.' }
        $thread = Invoke-RestMethod "$baseUrl/Support/Thread?id=$id" -WebSession $customer.Session
        Assert ($thread.tickets.Length -eq 1 -and $thread.tickets[0].status -eq 'Queued') 'Offline agent did not produce a queued ticket.'

        $socket = Open-Live $agent
        $customerSocket = Open-Live $customer
        try {
            $null = Post $agent Ticket @{ ticketId = $thread.tickets[0].id; action = 'Join' }
            $null = Post $customer Send @{ id = $id; message = 'My order arrived damaged.' }
            $null = Post $agent Send @{ id = $id; message = 'I can help with the damaged order.' }
            $thread = Invoke-RestMethod "$baseUrl/Support/Thread?id=$id" -WebSession $customer.Session
            Assert ($thread.status -eq 'Human' -and $thread.messages[-1].sender -eq 'Agent' -and $thread.messages[-2].sender -eq 'Customer') 'Handoff or agent reply failed.'
            $deadline = [Threading.CancellationTokenSource]::new(10000)
            try {
                $buffer = [byte[]]::new(1024)
                $frame = $customerSocket.ReceiveAsync([ArraySegment[byte]]::new($buffer), $deadline.Token).GetAwaiter().GetResult()
                Assert ([Text.Encoding]::UTF8.GetString($buffer, 0, $frame.Count) -match '"target":"Changed"') 'Customer did not receive a SignalR invalidation.'
            }
            finally { $deadline.Dispose() }
            $null = Post $customer Feedback @{ id = $id; rating = 5 }
        }
        finally { $socket.Dispose(); $customerSocket.Dispose() }
        $requeued = $false
        for ($attempt = 0; $attempt -lt 20; $attempt++) {
            $thread = Invoke-RestMethod "$baseUrl/Support/Thread?id=$id" -WebSession $customer.Session
            if ($thread.status -eq 'Waiting' -and $thread.tickets[0].status -eq 'Queued') { $requeued = $true; break }
            Start-Sleep -Milliseconds 250
        }
        Assert $requeued 'A disconnected agent left the conversation assigned.'

        Write-Output 'Passed MVC/SQL/SignalR smoke test.'
    }
    finally { if ($server -and -not $server.HasExited) { Stop-Process -Id $server.Id } }
}
finally {
    Pop-Location
    $env:SUPPORT_TEST_CUSTOMER_PASSWORD = $null
    $env:SUPPORT_TEST_AGENT_PASSWORD = $null
    $env:SUPPORT_TEST_ADMIN_PASSWORD = $null
    $env:SUPPORT_TEST_ACCOUNT_SUFFIX = $null
}
