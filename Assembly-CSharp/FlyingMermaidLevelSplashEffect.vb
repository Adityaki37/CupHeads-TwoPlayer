Imports System
Imports UnityEngine

' Token: 0x0200069E RID: 1694
Public Class FlyingMermaidLevelSplashEffect
	Inherits Effect

	' Token: 0x060023E7 RID: 9191 RVA: 0x001515F4 File Offset: 0x0014F9F4
	Private Sub LayerUp()
		Dim component As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		Dim num As Integer = component.sortingOrder
		Dim text As String = component.sortingLayerName
		If text = "Foreground" OrElse (text = "Background" AndAlso num < 80) Then
			num = num - num Mod 20 + 21
		Else
			text = "Foreground"
			num = 1
		End If
		component.sortingLayerName = text
		component.sortingOrder = num
	End Sub
End Class
