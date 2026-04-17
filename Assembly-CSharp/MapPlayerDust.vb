Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200097C RID: 2428
Public Class MapPlayerDust
	Inherits Effect

	' Token: 0x060038AD RID: 14509 RVA: 0x00204778 File Offset: 0x00202B78
	Public Function Create(position As Vector3, offsetRotation As Single, isLeft As Boolean, sortingOrder As Integer) As Effect
		Dim vector As Vector3 = Vector3.right * Me.offset.x
		If isLeft Then
			vector *= -1F
		End If
		Dim vector2 As Vector3 = Quaternion.Euler(offsetRotation, 0F, 0F) * vector
		vector2.y += Me.offset.y
		Me.spriteRenderer.sortingOrder = sortingOrder
		position.z = position.y - 0.01F
		Return Me.Create(position + vector2)
	End Function

	' Token: 0x060038AE RID: 14510 RVA: 0x0020480C File Offset: 0x00202C0C
	Public Overrides Sub Initialize(position As Vector3, scale As Vector3, randomR As Boolean)
		MyBase.Initialize(position, scale * Global.UnityEngine.Random.Range(Me.scaleRange.min, Me.scaleRange.max), randomR)
		Dim color As Color = Me.spriteRenderer.color
		color.a *= Global.UnityEngine.Random.Range(Me.opacityRange.min, Me.opacityRange.max)
		Me.spriteRenderer.color = color
		MyBase.animator.SetTrigger("startAnim")
		MyBase.StartCoroutine(Me.dust_cr())
	End Sub

	' Token: 0x060038AF RID: 14511 RVA: 0x002048A0 File Offset: 0x00202CA0
	Private Iterator Function dust_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.OnEffectComplete()
		Return
	End Function

	' Token: 0x04004065 RID: 16485
	Private Const StartAnimTrigger As String = "startAnim"

	' Token: 0x04004066 RID: 16486
	<SerializeField()>
	Private scaleRange As MinMax

	' Token: 0x04004067 RID: 16487
	<SerializeField()>
	Private opacityRange As MinMax

	' Token: 0x04004068 RID: 16488
	<SerializeField()>
	Private offset As Vector3

	' Token: 0x04004069 RID: 16489
	<SerializeField()>
	Private spriteRenderer As SpriteRenderer
End Class
