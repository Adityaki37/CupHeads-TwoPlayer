param(
    [string]$OutputDir = "",
    [string]$HostProcessPathPart = "Cuphead LAN Host",
    [string]$ClientProcessPathPart = "Cuphead LAN Client"
)

$ErrorActionPreference = 'Stop'

if ([string]::IsNullOrWhiteSpace($OutputDir)) {
    $repo = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
    $OutputDir = Join-Path $repo "visual-test"
}

New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null

Add-Type @"
using System;
using System.Runtime.InteropServices;
public static class CupheadVisualWin32 {
    [StructLayout(LayoutKind.Sequential)] public struct RECT { public int Left; public int Top; public int Right; public int Bottom; }
    [StructLayout(LayoutKind.Sequential)] public struct POINT { public int X; public int Y; }
    [DllImport("user32.dll")] public static extern bool SetProcessDPIAware();
    [DllImport("user32.dll")] public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
    [DllImport("user32.dll")] public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll")] public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
    [DllImport("user32.dll")] public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);
}
"@
[CupheadVisualWin32]::SetProcessDPIAware() | Out-Null
Add-Type -AssemblyName System.Drawing

function Get-CupheadProcess([string]$namePart) {
    $info = Get-CimInstance Win32_Process -Filter "name='Cuphead.exe'" |
        Where-Object { $_.ExecutablePath -like "*$namePart*" } |
        Select-Object -First 1

    if (-not $info) {
        throw "Could not find Cuphead process containing path segment: $namePart"
    }

    return Get-Process -Id $info.ProcessId
}

function Capture-ClientArea([System.Diagnostics.Process]$process, [string]$path) {
    $process.Refresh()
    if ($process.MainWindowHandle -eq [IntPtr]::Zero) {
        throw "No window handle for PID $($process.Id)"
    }

    $rect = New-Object CupheadVisualWin32+RECT
    [CupheadVisualWin32]::GetClientRect($process.MainWindowHandle, [ref]$rect) | Out-Null
    $point = New-Object CupheadVisualWin32+POINT
    $point.X = 0
    $point.Y = 0
    [CupheadVisualWin32]::ClientToScreen($process.MainWindowHandle, [ref]$point) | Out-Null

    $width = [Math]::Max(1, $rect.Right - $rect.Left)
    $height = [Math]::Max(1, $rect.Bottom - $rect.Top)
    $bitmap = New-Object System.Drawing.Bitmap($width, $height, [System.Drawing.Imaging.PixelFormat]::Format24bppRgb)
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.CopyFromScreen($point.X, $point.Y, 0, 0, (New-Object System.Drawing.Size($width, $height)))
    $graphics.Dispose()
    $bitmap.Save($path, [System.Drawing.Imaging.ImageFormat]::Png)
    $bitmap.Dispose()

    [pscustomobject]@{
        Path = $path
        Width = $width
        Height = $height
        X = $point.X
        Y = $point.Y
    }
}

function Compare-Bitmaps([string]$hostPath, [string]$clientPath, [string]$diffPath) {
    $hostBitmap = New-Object System.Drawing.Bitmap($hostPath)
    $clientBitmap = New-Object System.Drawing.Bitmap($clientPath)
    $width = [Math]::Min($hostBitmap.Width, $clientBitmap.Width)
    $height = [Math]::Min($hostBitmap.Height, $clientBitmap.Height)
    $topHeight = [Math]::Max(1, [int]($height * 0.28))
    $diffBitmap = New-Object System.Drawing.Bitmap($width, $height, [System.Drawing.Imaging.PixelFormat]::Format24bppRgb)

    $sum = [double]0
    $sumTop = [double]0
    $over = 0
    $overTop = 0
    $hostRed = 0
    $clientRed = 0
    $hostRedMinX = 999999
    $hostRedMinY = 999999
    $hostRedMaxX = -1
    $hostRedMaxY = -1
    $clientRedMinX = 999999
    $clientRedMinY = 999999
    $clientRedMaxX = -1
    $clientRedMaxY = -1

    for ($y = 0; $y -lt $height; $y++) {
        for ($x = 0; $x -lt $width; $x++) {
            $hostPixel = $hostBitmap.GetPixel($x, $y)
            $clientPixel = $clientBitmap.GetPixel($x, $y)
            $delta = [Math]::Abs([int]$hostPixel.R - [int]$clientPixel.R) +
                [Math]::Abs([int]$hostPixel.G - [int]$clientPixel.G) +
                [Math]::Abs([int]$hostPixel.B - [int]$clientPixel.B)

            $sum += $delta
            if ($delta -gt 90) {
                $over++
            }

            if ($y -lt $topHeight) {
                $sumTop += $delta
                if ($delta -gt 90) {
                    $overTop++
                }

                if ($hostPixel.R -gt 135 -and $hostPixel.G -lt 90 -and $hostPixel.B -lt 90) {
                    $hostRed++
                    if ($x -lt $hostRedMinX) { $hostRedMinX = $x }
                    if ($x -gt $hostRedMaxX) { $hostRedMaxX = $x }
                    if ($y -lt $hostRedMinY) { $hostRedMinY = $y }
                    if ($y -gt $hostRedMaxY) { $hostRedMaxY = $y }
                }

                if ($clientPixel.R -gt 135 -and $clientPixel.G -lt 90 -and $clientPixel.B -lt 90) {
                    $clientRed++
                    if ($x -lt $clientRedMinX) { $clientRedMinX = $x }
                    if ($x -gt $clientRedMaxX) { $clientRedMaxX = $x }
                    if ($y -lt $clientRedMinY) { $clientRedMinY = $y }
                    if ($y -gt $clientRedMaxY) { $clientRedMaxY = $y }
                }
            }

            $value = [Math]::Min(255, [int]$delta)
            if ($delta -gt 90) {
                $diffBitmap.SetPixel($x, $y, [System.Drawing.Color]::FromArgb(255, $value, 0, 255))
            } else {
                $diffBitmap.SetPixel($x, $y, [System.Drawing.Color]::FromArgb($value, $value, $value))
            }
        }
    }

    $diffBitmap.Save($diffPath, [System.Drawing.Imaging.ImageFormat]::Png)
    $hostBitmap.Dispose()
    $clientBitmap.Dispose()
    $diffBitmap.Dispose()

    $pixelCount = [double]($width * $height)
    $topPixelCount = [double]($width * $topHeight)
    [pscustomobject]@{
        Width = $width
        Height = $height
        MeanAbsDiffPerChannel = [Math]::Round($sum / ($pixelCount * 3), 3)
        PixelsOverThresholdPct = [Math]::Round(100.0 * $over / $pixelCount, 3)
        TopHudMeanAbsDiffPerChannel = [Math]::Round($sumTop / ($topPixelCount * 3), 3)
        TopHudPixelsOverThresholdPct = [Math]::Round(100.0 * $overTop / $topPixelCount, 3)
        HostRedPixelsTop = $hostRed
        ClientRedPixelsTop = $clientRed
        HostRedBox = if ($hostRed -gt 0) { "$hostRedMinX,$hostRedMinY-$hostRedMaxX,$hostRedMaxY" } else { "none" }
        ClientRedBox = if ($clientRed -gt 0) { "$clientRedMinX,$clientRedMinY-$clientRedMaxX,$clientRedMaxY" } else { "none" }
        DiffPath = $diffPath
    }
}

$hostProcess = Get-CupheadProcess $HostProcessPathPart
$clientProcess = Get-CupheadProcess $ClientProcessPathPart

$hostProcess.Refresh()
$clientProcess.Refresh()
[CupheadVisualWin32]::ShowWindow($hostProcess.MainWindowHandle, 9) | Out-Null
[CupheadVisualWin32]::ShowWindow($clientProcess.MainWindowHandle, 9) | Out-Null
[CupheadVisualWin32]::MoveWindow($hostProcess.MainWindowHandle, 30, 60, 672, 425, $true) | Out-Null
[CupheadVisualWin32]::MoveWindow($clientProcess.MainWindowHandle, 730, 60, 672, 425, $true) | Out-Null
Start-Sleep -Milliseconds 800

$hostShot = Join-Path $OutputDir "host-small-window.png"
$clientShot = Join-Path $OutputDir "client-small-window.png"
$diffShot = Join-Path $OutputDir "host-client-diff.png"
$hostCapture = Capture-ClientArea $hostProcess $hostShot
$clientCapture = Capture-ClientArea $clientProcess $clientShot
$comparison = Compare-Bitmaps $hostShot $clientShot $diffShot

$result = [pscustomobject]@{
    HostCapture = $hostCapture
    ClientCapture = $clientCapture
    Comparison = $comparison
}

$reportPath = Join-Path $OutputDir "comparison.json"
$result | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath $reportPath -Encoding UTF8
$result | ConvertTo-Json -Depth 6
