Imports System
Imports TMPro
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x0200045F RID: 1119
Public Class LeaderDotSizer
	Inherits MonoBehaviour

	' Token: 0x060010FB RID: 4347 RVA: 0x000A271F File Offset: 0x000A0B1F
	Private Sub Start()
		Me.SetLeaderDots()
	End Sub

	' Token: 0x060010FC RID: 4348 RVA: 0x000A2728 File Offset: 0x000A0B28
	Private Sub SetLeaderDots()
		Me.leaderDotText.text = ". . . . . . . . . . . . . . . . . . . . . . ."
		Dim num As Single = Me.leaderDotText.rectTransform.sizeDelta.x - Me.descriptionText.preferredWidth
		If num < 0F Then
			Me.leaderDotText.text = String.Empty
			Return
		End If
		Dim num2 As Integer = 100000
		While Me.leaderDotText.text.Length > 2 AndAlso Me.leaderDotText.preferredWidth > num AndAlso num2 > 0
			num2 -= 1
			Me.leaderDotText.text = Me.leaderDotText.text.Substring(0, Me.leaderDotText.text.Length - 2)
		End While
	End Sub

	' Token: 0x04001A62 RID: 6754
	Private Const Dots As String = ". . . . . . . . . . . . . . . . . . . . . . ."

	' Token: 0x04001A63 RID: 6755
	Private Const DotsPadding As Single = 5F

	' Token: 0x04001A64 RID: 6756
	<SerializeField()>
	Private descriptionText As TextMeshProUGUI

	' Token: 0x04001A65 RID: 6757
	<SerializeField()>
	Private leaderDotText As Text
End Class
