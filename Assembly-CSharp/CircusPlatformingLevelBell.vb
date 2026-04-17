Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200089E RID: 2206
Public Class CircusPlatformingLevelBell
	Inherits PlatformingLevelPathMovementEnemy

	' Token: 0x06003356 RID: 13142 RVA: 0x001DDE3F File Offset: 0x001DC23F
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		MyBase.animator.Play("Ring")
		AudioManager.Play("circus_bell_ding")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.StartCoroutine(Me.timer_cr())
	End Sub

	' Token: 0x06003357 RID: 13143 RVA: 0x001DDE74 File Offset: 0x001DC274
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
	End Sub

	' Token: 0x06003358 RID: 13144 RVA: 0x001DDE78 File Offset: 0x001DC278
	Private Iterator Function timer_cr() As IEnumerator
		Dim t As Single = 0F
		While t < Me.coolDown
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Dim collider As Collider2D = MyBase.GetComponent(Of Collider2D)()
		collider.enabled = True
		Yield Nothing
		Return
	End Function

	' Token: 0x06003359 RID: 13145 RVA: 0x001DDE93 File Offset: 0x001DC293
	Protected Overrides Sub CalculateCollider()
	End Sub

	' Token: 0x04003BA0 RID: 15264
	Public coolDown As Single = 0.4F
End Class
