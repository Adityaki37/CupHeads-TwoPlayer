param(
    [Parameter(Mandatory = $true)]
    [string] $ReportA,

    [Parameter(Mandatory = $true)]
    [string] $ReportB,

    [string] $OutputPath = ""
)

$ErrorActionPreference = "Stop"

function Resolve-ReportDirectory([string]$Path) {
    $resolved = (Resolve-Path -LiteralPath $Path).Path
    if (-not (Test-Path -LiteralPath $resolved -PathType Container)) {
        throw "Report path must be an exported CupHeads report folder: $Path"
    }

    foreach ($required in @("diagnostics.txt", "LogOutput.log")) {
        if (-not (Test-Path -LiteralPath (Join-Path $resolved $required))) {
            throw "Missing $required in report folder: $resolved"
        }
    }

    return $resolved
}

function Read-ReportText([string]$Dir, [string]$Name) {
    $path = Join-Path $Dir $Name
    if (Test-Path -LiteralPath $path) {
        return [System.IO.File]::ReadAllText($path)
    }
    return ""
}

function Get-Field([string]$Text, [string]$Name) {
    $escaped = [regex]::Escape($Name)
    $match = [regex]::Match($Text, "(?m)^$escaped\s*:\s*(.*)$")
    if ($match.Success) {
        return $match.Groups[1].Value.Trim()
    }
    return ""
}

function New-ReportInfo([string]$Dir) {
    $pairing = Read-ReportText $Dir "pairing.txt"
    $diagnostics = Read-ReportText $Dir "diagnostics.txt"
    $combined = $pairing + "`n" + $diagnostics

    $role = Get-Field $combined "Role"
    if ([string]::IsNullOrWhiteSpace($role)) {
        $isHost = Get-Field $diagnostics "Is Host"
        if ($isHost -ieq "True") { $role = "Host" }
        elseif ($isHost -ieq "False") { $role = "Guest" }
        else { $role = "Unknown" }
    }

    $key = Get-Field $combined "Pairing Key"
    if ([string]::IsNullOrWhiteSpace($key)) {
        $lobby = Get-Field $diagnostics "Lobby ID"
        if ([string]::IsNullOrWhiteSpace($lobby) -or $lobby -eq "(none)") {
            $key = "(missing)"
        } else {
            $key = "steam-lobby-$lobby"
        }
    }

    [pscustomobject]@{
        Dir = $Dir
        Role = $role
        PairingKey = $key
        GeneratedLocal = Get-Field $combined "Generated Local"
        GeneratedUtc = Get-Field $combined "Generated UTC"
        Version = Get-Field $diagnostics "Version"
        State = Get-Field $diagnostics "State"
        LobbyId = Get-Field $diagnostics "Lobby ID"
        Peer = Get-Field $diagnostics "Peer"
        LastStatus = Get-Field $diagnostics "Last Status"
        LastFailure = Get-Field $diagnostics "Last Failure"
    }
}

function Get-PairLogEvents([string]$Dir, [string]$Label) {
    $log = Read-ReportText $Dir "LogOutput.log"
    $events = New-Object System.Collections.Generic.List[object]
    $lineNumber = 0

    foreach ($line in ($log -split "`r?`n")) {
        $lineNumber++
        if ($line -notmatch "\[PairLog\]") {
            continue
        }

        $utc = ""
        $role = ""
        $key = ""
        $state = ""
        $event = ""
        $detail = $line

        if ($line -match "utc=(\S+)") { $utc = $Matches[1] }
        if ($line -match "role=(\S+)") { $role = $Matches[1] }
        if ($line -match "key=(\S+)") { $key = $Matches[1] }
        if ($line -match "state=(\S+)") { $state = $Matches[1] }
        if ($line -match "event=(\S+)") { $event = $Matches[1] }
        if ($line -match "detail=(.*)$") { $detail = $Matches[1].Trim() }

        $parsedUtc = [datetime]::MinValue
        [datetime]::TryParse($utc, [ref]$parsedUtc) | Out-Null

        $events.Add([pscustomobject]@{
            Utc = $parsedUtc
            UtcText = $utc
            Side = $Label
            Role = $role
            PairingKey = $key
            State = $state
            Event = $event
            Detail = $detail
            LineNumber = $lineNumber
        })
    }

    return $events
}

function Get-InterestingLogLines([string]$Dir) {
    $log = Read-ReportText $Dir "LogOutput.log"
    return ($log -split "`r?`n") | Where-Object {
        $_ -match "\[PairLog\]|\[SteamNet\]|\[Session\]|\[LevelStartSync\]|\[StatusSync\]|\[Loadout\]|\[MapDialogue\]|\[EnemySync\]|\[Weapon\]|FAIL|Error|Exception"
    }
}

$dirA = Resolve-ReportDirectory $ReportA
$dirB = Resolve-ReportDirectory $ReportB
$infoA = New-ReportInfo $dirA
$infoB = New-ReportInfo $dirB

if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path (Get-Location) ("paired-cupheads-report-" + (Get-Date -Format "yyyyMMdd-HHmmss"))
}
New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null

$events = @()
$events += Get-PairLogEvents $dirA "A-$($infoA.Role)"
$events += Get-PairLogEvents $dirB "B-$($infoB.Role)"
$events = $events | Sort-Object @{ Expression = { $_.Utc } }, Side, LineNumber

$summary = New-Object System.Collections.Generic.List[string]
$summary.Add("# CupHeads Paired Report")
$summary.Add("")
$summary.Add("Generated: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')")
$summary.Add("")
$summary.Add("Pairing keys match: " + ($infoA.PairingKey -eq $infoB.PairingKey))
$summary.Add("")
$summary.Add("| Side | Role | Pairing Key | Version | State | Lobby | Peer |")
$summary.Add("| --- | --- | --- | --- | --- | --- | --- |")
$summary.Add("| A | $($infoA.Role) | $($infoA.PairingKey) | $($infoA.Version) | $($infoA.State) | $($infoA.LobbyId) | $($infoA.Peer) |")
$summary.Add("| B | $($infoB.Role) | $($infoB.PairingKey) | $($infoB.Version) | $($infoB.State) | $($infoB.LobbyId) | $($infoB.Peer) |")
$summary.Add("")
$summary.Add("## Last Status")
$summary.Add("")
$summary.Add("A: $($infoA.LastStatus)")
$summary.Add("")
$summary.Add("B: $($infoB.LastStatus)")
$summary.Add("")
$summary.Add("## Last Failure")
$summary.Add("")
$summary.Add("A: $($infoA.LastFailure)")
$summary.Add("")
$summary.Add("B: $($infoB.LastFailure)")
$summary.Add("")
$summary.Add("## PairLog Timeline")
$summary.Add("")

if ($events.Count -eq 0) {
    $summary.Add("No `[PairLog]` entries were found. Use CupHeads v1.2.34 or newer on both sides for timestamped pairing.")
} else {
    $summary.Add("| UTC | Side | Role | State | Event | Detail |")
    $summary.Add("| --- | --- | --- | --- | --- | --- |")
    foreach ($event in $events) {
        $summary.Add("| $($event.UtcText) | $($event.Side) | $($event.Role) | $($event.State) | $($event.Event) | $($event.Detail.Replace('|', '/')) |")
    }
}

[System.IO.File]::WriteAllLines((Join-Path $OutputPath "paired-summary.md"), $summary.ToArray())
[System.IO.File]::WriteAllLines((Join-Path $OutputPath "A-interesting.log"), (Get-InterestingLogLines $dirA))
[System.IO.File]::WriteAllLines((Join-Path $OutputPath "B-interesting.log"), (Get-InterestingLogLines $dirB))

Write-Output $OutputPath
