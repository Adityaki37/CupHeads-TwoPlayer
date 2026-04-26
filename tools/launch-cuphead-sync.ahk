#Requires AutoHotkey v2.0
#SingleInstance Off

if A_Args.Length < 14 {
    FileAppend "error=Expected 14 arguments.`n", "*"
    ExitApp 2
}

hostRoot := A_Args[1]
clientRoot := A_Args[2]
outputPath := A_Args[3]
unityArgs := A_Args[4]
hostX := Integer(A_Args[5])
hostY := Integer(A_Args[6])
hostW := Integer(A_Args[7])
hostH := Integer(A_Args[8])
clientX := Integer(A_Args[9])
clientY := Integer(A_Args[10])
clientW := Integer(A_Args[11])
clientH := Integer(A_Args[12])
clientDelayMs := Integer(A_Args[13])
waitSeconds := Integer(A_Args[14])

try FileDelete outputPath

hostExe := hostRoot "\Cuphead.exe"
clientExe := clientRoot "\Cuphead.exe"

hostPid := LaunchCuphead(hostExe, hostRoot, unityArgs)
hostHwnd := WaitForWindow(hostPid, waitSeconds)
if !hostHwnd {
    WriteResult(outputPath, "error=Host window did not appear.`nhostPid=" hostPid "`n")
    ExitApp 3
}
ShowMoveNoActivate(hostHwnd, hostX, hostY, hostW, hostH)

Sleep clientDelayMs

clientPid := LaunchCuphead(clientExe, clientRoot, unityArgs)
clientHwnd := WaitForWindow(clientPid, waitSeconds)
if !clientHwnd {
    WriteResult(outputPath, "error=Client window did not appear.`nhostPid=" hostPid "`nclientPid=" clientPid "`n")
    ExitApp 4
}
ShowMoveNoActivate(clientHwnd, clientX, clientY, clientW, clientH)

WriteResult(
    outputPath,
    "hostPid=" hostPid "`n"
    . "clientPid=" clientPid "`n"
    . "hostHwnd=" hostHwnd "`n"
    . "clientHwnd=" clientHwnd "`n")

ExitApp 0

LaunchCuphead(exePath, workDir, args) {
    command := '"' exePath '" ' args
    Run command, workDir, "Hide", &pid
    return pid
}

WaitForWindow(pid, waitSeconds) {
    title := "ahk_pid " pid
    if !WinWait(title, , waitSeconds)
        return 0
    return WinExist(title)
}

ShowMoveNoActivate(hwnd, x, y, w, h) {
    ; SW_SHOWNOACTIVATE keeps the user's current foreground window alone.
    DllCall("ShowWindow", "ptr", hwnd, "int", 4)

    ; SWP_NOZORDER | SWP_NOACTIVATE | SWP_SHOWWINDOW
    DllCall(
        "SetWindowPos",
        "ptr", hwnd,
        "ptr", 0,
        "int", x,
        "int", y,
        "int", w,
        "int", h,
        "uint", 0x0054)
}

WriteResult(path, text) {
    FileAppend text, path, "UTF-8"
}
