Imports System
Imports UnityEngine

' Token: 0x02000B82 RID: 2946
Public Class DialoguerExampleStart
	Inherits MonoBehaviour

	' Token: 0x060046D7 RID: 18135 RVA: 0x0024FE9D File Offset: 0x0024E29D
	Private Sub Awake()
		Dialoguer.Initialize()
	End Sub

	' Token: 0x060046D8 RID: 18136 RVA: 0x0024FEA4 File Offset: 0x0024E2A4
	Private Sub OnGUI()
		If GUI.Button(New Rect(10F, 10F, 100F, 30F), "Start!") Then
			Dialoguer.StartDialogue(3)
		End If
		Dim text As String = "Open this file (DialoguerExampleStart.cs) to see how to start using Dialoguer"
		GUI.Label(New Rect(10F, 50F, 500F, 500F), text)
	End Sub
End Class
