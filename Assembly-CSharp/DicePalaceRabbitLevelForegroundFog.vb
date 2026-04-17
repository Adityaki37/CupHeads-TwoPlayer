Imports System
Imports UnityEngine

' Token: 0x020005DD RID: 1501
Public Class DicePalaceRabbitLevelForegroundFog
	Inherits AbstractPausableComponent

	' Token: 0x06001DA7 RID: 7591 RVA: 0x00110B5C File Offset: 0x0010EF5C
	Private Sub Update()
		Me.angle += Me.speed * CupheadTime.Delta
		Dim vector As Vector3 = New Vector3(-Mathf.Sin(Me.angle) * Me.loopSize, 0F, 0F)
		Dim vector2 As Vector3 = New Vector3(0F, Mathf.Cos(Me.angle) * Me.loopSize, 0F)
		MyBase.transform.position = Me.pivotPoint.position
		MyBase.transform.position += vector + vector2
		If Me.fadingOut Then
			If Me.time < Me.fadeTime Then
				If MyBase.GetComponent(Of SpriteRenderer)().color.a > 0.5F Then
					MyBase.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F - Me.time / Me.fadeTime)
				End If
				Me.time += CupheadTime.Delta
			Else
				Me.fadingOut = Not Me.fadingOut
				Me.time = 0F
			End If
		ElseIf Me.time < Me.fadeTime Then
			MyBase.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 0.5F + Me.time / Me.fadeTime)
			Me.time += CupheadTime.Delta
		Else
			Me.fadingOut = Not Me.fadingOut
			Me.time = 0F
		End If
	End Sub

	' Token: 0x04002685 RID: 9861
	<SerializeField()>
	Private pivotPoint As Transform

	' Token: 0x04002686 RID: 9862
	Private loopSize As Single = 5F

	' Token: 0x04002687 RID: 9863
	Private speed As Single = 2F

	' Token: 0x04002688 RID: 9864
	Private angle As Single

	' Token: 0x04002689 RID: 9865
	Private time As Single

	' Token: 0x0400268A RID: 9866
	Private fadeTime As Single = 5F

	' Token: 0x0400268B RID: 9867
	Private fadingOut As Boolean = True
End Class
