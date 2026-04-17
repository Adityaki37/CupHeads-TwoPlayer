Imports System
Imports System.Text
Imports Rewired
Imports UnityEngine

' Token: 0x02000B3E RID: 2878
Public Class DEBUG_RewiredPrinter
	Inherits MonoBehaviour

	' Token: 0x060045B1 RID: 17841 RVA: 0x0024C27A File Offset: 0x0024A67A
	Private Sub Awake()
		Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
	End Sub

	' Token: 0x060045B2 RID: 17842 RVA: 0x0024C288 File Offset: 0x0024A688
	Private Sub OnGUI()
		If Not ReInput.isReady Then
			Return
		End If
		Dim stringBuilder As StringBuilder = New StringBuilder()
		stringBuilder.AppendLine("===PLAYERS===")
		For Each player As Player In ReInput.players.AllPlayers
			stringBuilder.AppendLine(player.name)
			For Each joystick As Joystick In player.controllers.Joysticks
				DEBUG_RewiredPrinter.appendControllerInfo(joystick, stringBuilder)
			Next
		Next
		stringBuilder.AppendLine("===UNASSIGNED===")
		For Each joystick2 As Joystick In ReInput.controllers.Joysticks
			If Not ReInput.controllers.IsJoystickAssigned(joystick2) Then
				DEBUG_RewiredPrinter.appendControllerInfo(joystick2, stringBuilder)
			End If
		Next
		stringBuilder.AppendLine("===BUTTONS===")
		GUI.Box(New Rect(0F, 0F, 700F, 400F), stringBuilder.ToString())
	End Sub

	' Token: 0x060045B3 RID: 17843 RVA: 0x0024C400 File Offset: 0x0024A800
	Private Shared Sub appendControllerInfo(j As Joystick, builder As StringBuilder)
		builder.AppendFormat("{0} :: {1}", j.name, j.id.ToString())
		builder.Append(vbLf)
	End Sub
End Class
