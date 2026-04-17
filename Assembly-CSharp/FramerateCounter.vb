Imports System
Imports UnityEngine

' Token: 0x0200035E RID: 862
Public Class FramerateCounter
	Inherits MonoBehaviour

	' Token: 0x170001F1 RID: 497
	' (get) Token: 0x06000990 RID: 2448 RVA: 0x0007BEA9 File Offset: 0x0007A2A9
	' (set) Token: 0x06000991 RID: 2449 RVA: 0x0007BEB0 File Offset: 0x0007A2B0
	Public Shared Property Current As FramerateCounter

	' Token: 0x06000992 RID: 2450 RVA: 0x0007BEB8 File Offset: 0x0007A2B8
	Public Shared Sub Init()
		If FramerateCounter.Current Is Nothing Then
			Dim gameObject As GameObject = New GameObject("Framerate Counter")
			FramerateCounter.Current = gameObject.AddComponent(Of FramerateCounter)()
			Global.UnityEngine.[Object].DontDestroyOnLoad(gameObject)
		End If
	End Sub

	' Token: 0x06000993 RID: 2451 RVA: 0x0007BEF1 File Offset: 0x0007A2F1
	Protected Overridable Sub Start()
		Me.timeleft = Me.updateInterval
	End Sub

	' Token: 0x06000994 RID: 2452 RVA: 0x0007BF00 File Offset: 0x0007A300
	Private Sub Update()
		Me.timeleft -= Time.deltaTime
		Me.accum += Time.timeScale / Time.deltaTime
		Me.frames += 1
		If CDbl(Me.timeleft) <= 0.0 Then
			Dim num As Single = Me.accum / CSng(Me.frames)
			Dim text As String = String.Format("{0:F2} FPS" & vbLf & "{1} HP", num, Me.hpCounter)
			Me.text = text
			If num < 10F Then
				Me.color = "red"
			ElseIf num < 30F Then
				Me.color = "orange"
			Else
				Me.color = "lime"
			End If
			Me.timeleft = Me.updateInterval
			Me.accum = 0F
			Me.frames = 0
		End If
	End Sub

	' Token: 0x06000995 RID: 2453 RVA: 0x0007BFEC File Offset: 0x0007A3EC
	Protected Overridable Sub OnGUI()
		If Not FramerateCounter.SHOW Then
			Return
		End If
		If Me.style Is Nothing Then
			Me.style = New GUIStyle(GUI.skin.label)
			Me.style.alignment = TextAnchor.UpperRight
			Me.style.richText = True
			Me.style.padding = New RectOffset(20, 20, 20, 20)
		End If
		GUI.Label(New Rect(0F, 0F, CSng((Screen.width + 1)), CSng((Screen.height + 1))), "<color=black>" + Me.text + "</color>", Me.style)
		GUI.Label(New Rect(0F, 0F, CSng(Screen.width), CSng(Screen.height)), String.Concat(New String() { "<color=", Me.color, ">", Me.text, "</color>" }), Me.style)
	End Sub

	' Token: 0x04001431 RID: 5169
	Public Shared SHOW As Boolean

	' Token: 0x04001432 RID: 5170
	Public updateInterval As Single = 0.25F

	' Token: 0x04001433 RID: 5171
	Public hpCounter As Integer

	' Token: 0x04001434 RID: 5172
	Private accum As Single

	' Token: 0x04001435 RID: 5173
	Private frames As Integer

	' Token: 0x04001436 RID: 5174
	Private timeleft As Single

	' Token: 0x04001437 RID: 5175
	Private style As GUIStyle

	' Token: 0x04001438 RID: 5176
	Private text As String

	' Token: 0x04001439 RID: 5177
	Private color As String = "white"
End Class
