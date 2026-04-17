Imports System
Imports UnityEngine

' Token: 0x02000647 RID: 1607
Public Class FlyingCowboyFloatingSausages
	Inherits Effect

	' Token: 0x060020FE RID: 8446 RVA: 0x00130CCA File Offset: 0x0012F0CA
	Public Sub SetAnimation(name As String)
		MyBase.animator.Play(name)
	End Sub

	' Token: 0x060020FF RID: 8447 RVA: 0x00130CD8 File Offset: 0x0012F0D8
	Private Sub FixedUpdate()
		MyBase.transform.position += Vector3.up * 200F * CupheadTime.FixedDelta
		If MyBase.transform.position.y > 460F Then
			Me.OnEffectComplete()
		End If
	End Sub

	' Token: 0x04002998 RID: 10648
	Private Const OFFSET As Single = 100F

	' Token: 0x04002999 RID: 10649
	Private Const SPEED As Single = 200F
End Class
