Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000B13 RID: 2835
Public Class OneTimeScrollingSprite
	Inherits AbstractPausableComponent

	' Token: 0x060044C2 RID: 17602 RVA: 0x0024672F File Offset: 0x00244B2F
	Private Sub Start()
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x060044C3 RID: 17603 RVA: 0x00246740 File Offset: 0x00244B40
	Private Iterator Function loop_cr() As IEnumerator
		Dim spriteRenderer As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		While MyBase.transform.position.x + spriteRenderer.bounds.size.x / 2F > -1280F
			If Me.OutCondition IsNot Nothing AndAlso Me.OutCondition() Then
				Return
			End If
			Dim position As Vector2 = MyBase.transform.localPosition
			position.x -= Me.speed * CupheadTime.Delta
			MyBase.transform.localPosition = position
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04004A7D RID: 19069
	Private Const X_OUT As Single = -1280F

	' Token: 0x04004A7E RID: 19070
	Public speed As Single

	' Token: 0x04004A7F RID: 19071
	Public OutCondition As Func(Of Boolean)
End Class
