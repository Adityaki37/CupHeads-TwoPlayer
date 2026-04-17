Imports System
Imports UnityEngine

' Token: 0x02000B3A RID: 2874
Public Class DEBUG_DLCStatusPrinter
	Inherits MonoBehaviour

	' Token: 0x060045A3 RID: 17827 RVA: 0x0024BDB7 File Offset: 0x0024A1B7
	Private Sub Awake()
		Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
	End Sub

	' Token: 0x060045A4 RID: 17828 RVA: 0x0024BDC4 File Offset: 0x0024A1C4
	Private Sub OnGUI()
		If Time.frameCount < 120 Then
			Return
		End If
		If Me.style Is Nothing Then
			Me.style = New GUIStyle(GUI.skin.GetStyle("Box"))
			Me.style.alignment = TextAnchor.UpperLeft
		End If
		GUI.Box(New Rect(0F, 0F, 200F, 100F), "DLC Enabled: " + DLCManager.DLCEnabled())
	End Sub

	' Token: 0x04004BCC RID: 19404
	Private style As GUIStyle
End Class
