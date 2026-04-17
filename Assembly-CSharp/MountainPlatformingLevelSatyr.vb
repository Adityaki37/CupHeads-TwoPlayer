Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008ED RID: 2285
Public Class MountainPlatformingLevelSatyr
	Inherits PlatformingLevelGroundMovementEnemy

	' Token: 0x06003599 RID: 13721 RVA: 0x001F389B File Offset: 0x001F1C9B
	Protected Overrides Sub Start()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.Start()
		MyBase.StartCoroutine(Me.satyr_land_cr())
	End Sub

	' Token: 0x0600359A RID: 13722 RVA: 0x001F38BC File Offset: 0x001F1CBC
	Public Sub Init(direction As PlatformingLevelGroundMovementEnemy.Direction, isForeground As Boolean)
		Me._direction = direction
		MyBase.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Background.ToString()
	End Sub

	' Token: 0x0600359B RID: 13723 RVA: 0x001F38EC File Offset: 0x001F1CEC
	Private Iterator Function satyr_land_cr() As IEnumerator
		AudioManager.Play("castle_imp_spawn")
		Me.emitAudioFromObject.Add("castle_imp_spawn")
		Me.floating = False
		MyBase.Jump()
		MyBase.StartCoroutine(Me.change_layer_cr())
		While Not MyBase.Grounded
			Yield Nothing
		End While
		Me.landing = True
		AudioManager.Play("castle_imp_land")
		Me.emitAudioFromObject.Add("castle_imp_land")
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro_Continue", False, True)
		Me.landing = False
		Yield Nothing
		Return
	End Function

	' Token: 0x0600359C RID: 13724 RVA: 0x001F3908 File Offset: 0x001F1D08
	Private Iterator Function change_layer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		MyBase.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Enemies.ToString()
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = 20
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Yield Nothing
		Return
	End Function

	' Token: 0x0600359D RID: 13725 RVA: 0x001F3923 File Offset: 0x001F1D23
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If phase = CollisionPhase.Enter AndAlso hit.GetComponent(Of MountainPlatformingLevelWall)() Then
			Me.Turn()
		End If
	End Sub

	' Token: 0x0600359E RID: 13726 RVA: 0x001F394A File Offset: 0x001F1D4A
	Protected Overrides Sub Die()
		AudioManager.Play("castle_generic_death_honk")
		Me.emitAudioFromObject.Add("castle_generic_death_honk")
		MyBase.Die()
	End Sub
End Class
