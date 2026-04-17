Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005FE RID: 1534
Public Class DragonLevelRain
	Inherits AbstractPausableComponent

	' Token: 0x06001E84 RID: 7812 RVA: 0x00119666 File Offset: 0x00117A66
	Public Sub StartRain()
		MyBase.gameObject.SetActive(True)
		MyBase.StartCoroutine(Me.fade_cr())
	End Sub

	' Token: 0x06001E85 RID: 7813 RVA: 0x00119684 File Offset: 0x00117A84
	Private Iterator Function fade_cr() As IEnumerator
		Dim alpha As Single = 0F
		While alpha < 1F
			For i As Integer = 0 To Me.rainRenderers.Length - 1
				Dim color As Color = Me.rainRenderers(i).color
				color.a = alpha
				Me.rainRenderers(i).color = color
			Next
			alpha += CupheadTime.Delta / Me.fadeTime
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002760 RID: 10080
	<SerializeField()>
	Private fadeTime As Single

	' Token: 0x04002761 RID: 10081
	<SerializeField()>
	Private rainRenderers As SpriteRenderer()
End Class
