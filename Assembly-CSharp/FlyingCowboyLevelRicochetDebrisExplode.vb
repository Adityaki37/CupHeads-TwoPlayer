Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200065B RID: 1627
Public Class FlyingCowboyLevelRicochetDebrisExplode
	Inherits Effect

	' Token: 0x060021E8 RID: 8680 RVA: 0x0013BFEC File Offset: 0x0013A3EC
	Private Sub Start()
		Dim position As Vector3 = MyBase.transform.position
		position.z = Global.UnityEngine.Random.Range(0F, 1F)
		MyBase.transform.position = position
	End Sub

	' Token: 0x060021E9 RID: 8681 RVA: 0x0013C028 File Offset: 0x0013A428
	Private Iterator Function movement_cr() As IEnumerator
		Dim renderer As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		Dim elapsedTime As Single = 0F
		While elapsedTime < 0.5F
			Yield Nothing
			elapsedTime += CupheadTime.Delta
			Dim position As Vector3 = MyBase.transform.position
			position.x -= 900F * CupheadTime.Delta
			MyBase.transform.position = position
			Dim color As Color = renderer.color
			color.a = Mathf.Lerp(1F, 0F, elapsedTime / 0.5F)
			renderer.color = color
		End While
		Me.OnEffectComplete()
		Return
	End Function

	' Token: 0x060021EA RID: 8682 RVA: 0x0013C043 File Offset: 0x0013A443
	Private Sub animationEvent_StartMovement()
		MyBase.StartCoroutine(Me.movement_cr())
	End Sub
End Class
