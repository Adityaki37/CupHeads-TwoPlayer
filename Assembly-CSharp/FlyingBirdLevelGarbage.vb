Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200061E RID: 1566
Public Class FlyingBirdLevelGarbage
	Inherits BasicProjectile

	' Token: 0x06001FDD RID: 8157 RVA: 0x00124710 File Offset: 0x00122B10
	Protected Overrides Sub Start()
		MyBase.Start()
		If Not Me.isBoot Then
			MyBase.StartCoroutine(Me.not_boot_cr())
		Else
			MyBase.animator.SetBool("OnClockwise", Rand.Bool())
		End If
		MyBase.StartCoroutine(Me.change_layer_cr())
	End Sub

	' Token: 0x06001FDE RID: 8158 RVA: 0x00124764 File Offset: 0x00122B64
	Private Iterator Function not_boot_cr() As IEnumerator
		Dim frameTime As Single = 0F
		Me.bootSpeed = If((Not Rand.Bool()), 600F, 300F)
		Me.bootSpeed = If((Not Rand.Bool()), Me.bootSpeed, (-Me.bootSpeed))
		While True
			frameTime += CupheadTime.Delta
			If frameTime > 0.041666668F Then
				frameTime -= 0.041666668F
				MyBase.transform.Rotate(0F, 0F, Me.bootSpeed * CupheadTime.Delta)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001FDF RID: 8159 RVA: 0x00124780 File Offset: 0x00122B80
	Private Iterator Function change_layer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Projectiles.ToString()
		Return
	End Function

	' Token: 0x06001FE0 RID: 8160 RVA: 0x0012479B File Offset: 0x00122B9B
	Protected Overrides Sub Die()
		MyBase.Die()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
	End Sub

	' Token: 0x04002857 RID: 10327
	Private Const ROTATE_FRAME_TIME As Single = 0.041666668F

	' Token: 0x04002858 RID: 10328
	<SerializeField()>
	Private isBoot As Boolean

	' Token: 0x04002859 RID: 10329
	Private bootSpeed As Single
End Class
