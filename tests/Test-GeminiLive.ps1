param()
$project = Join-Path $PSScriptRoot '../src/SmartSupport-AI.PL/SmartSupport-AI.PL.csproj'
dotnet run --project $project --no-build --launch-profile http -- --verify-gemini
exit $LASTEXITCODE
