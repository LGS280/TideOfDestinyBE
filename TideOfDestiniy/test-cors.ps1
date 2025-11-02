# PowerShell Script to Test CORS Configuration
# Usage: .\test-cors.ps1

param(
    [string]$ApiUrl = "https://tideofdestinybe.onrender.com",
    [string]$Endpoint = "/api/News?category=1",
    [string]$Origin = "https://tide-of-destiny-client.vercel.app"
)

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "   CORS Configuration Test" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

$fullUrl = "$ApiUrl$Endpoint"

Write-Host "Testing Configuration:" -ForegroundColor Yellow
Write-Host "  API URL: $ApiUrl" -ForegroundColor White
Write-Host "  Endpoint: $Endpoint" -ForegroundColor White
Write-Host "  Origin: $Origin" -ForegroundColor White
Write-Host "  Full URL: $fullUrl" -ForegroundColor White
Write-Host ""

# Test 1: OPTIONS Preflight Request
Write-Host "Test 1: OPTIONS Preflight Request" -ForegroundColor Yellow
Write-Host "-----------------------------------" -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri $fullUrl -Method OPTIONS `
        -Headers @{
            "Origin" = $Origin
            "Access-Control-Request-Method" = "GET"
            "Access-Control-Request-Headers" = "Content-Type"
        } -ErrorAction Stop

    Write-Host "✅ Status Code: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "`nCORS Headers:" -ForegroundColor Cyan
    
    $corsHeaders = @(
        "Access-Control-Allow-Origin",
        "Access-Control-Allow-Methods",
        "Access-Control-Allow-Headers",
        "Access-Control-Allow-Credentials",
        "Access-Control-Max-Age"
    )
    
    $hasCors = $false
    foreach ($header in $corsHeaders) {
        if ($response.Headers.ContainsKey($header)) {
            $value = $response.Headers[$header]
            Write-Host "  $header`: $value" -ForegroundColor Green
            if ($header -eq "Access-Control-Allow-Origin") {
                $hasCors = $true
                if ($value -eq $Origin -or $value -eq "*") {
                    Write-Host "    ✅ Origin is allowed!" -ForegroundColor Green
                } else {
                    Write-Host "    ⚠️  Origin mismatch! Expected: $Origin, Got: $value" -ForegroundColor Yellow
                }
            }
        } else {
            Write-Host "  $header`: NOT SET" -ForegroundColor Red
        }
    }
    
    if (-not $hasCors) {
        Write-Host "`n❌ CORS is NOT configured! No Access-Control-Allow-Origin header found." -ForegroundColor Red
    } else {
        Write-Host "`n✅ CORS preflight is working!" -ForegroundColor Green
    }
} catch {
    Write-Host "❌ Preflight Request Failed!" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Message -like "*CORS*" -or $_.Exception.Message -like "*blocked*") {
        Write-Host "   This indicates CORS is blocking the request." -ForegroundColor Yellow
    }
}

Write-Host "`n"

# Test 2: GET Request
Write-Host "Test 2: GET Request" -ForegroundColor Yellow
Write-Host "-----------------------------------" -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri $fullUrl -Method GET `
        -Headers @{
            "Origin" = $Origin
        } -ErrorAction Stop

    Write-Host "✅ Status Code: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "✅ GET request successful!" -ForegroundColor Green
    
    if ($response.Headers.ContainsKey("Access-Control-Allow-Origin")) {
        Write-Host "   CORS Origin: $($response.Headers['Access-Control-Allow-Origin'])" -ForegroundColor Green
    }
} catch {
    Write-Host "❌ GET Request Failed!" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "   Test Complete" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "💡 Tips:" -ForegroundColor Yellow
Write-Host "   1. If CORS headers are missing, check your Program.cs configuration" -ForegroundColor White
Write-Host "   2. Ensure your code is deployed to Render.com" -ForegroundColor White
Write-Host "   3. Check that Render.com service has restarted after deployment" -ForegroundColor White
Write-Host "   4. Verify the origin matches exactly (case-sensitive in some cases)" -ForegroundColor White

