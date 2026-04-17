Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000A0D RID: 2573
Public Class HealerCharmSparkEffect
	Inherits Effect

	' Token: 0x06003CD5 RID: 15573 RVA: 0x0021A798 File Offset: 0x00218B98
	Public Sub Create(position As Vector3, scale As Vector3, target As AbstractPlayerController)
		Me.startedFlash = 0
		Dim healerCharmSparkEffect As HealerCharmSparkEffect = TryCast(MyBase.Create(position, scale), HealerCharmSparkEffect)
		healerCharmSparkEffect.target = target
		Me.particleVectors = New List(Of Vector2)() From { New Vector2(-0.26F, -0.93F), New Vector2(0.3F, -0.72F), New Vector2(0.77F, -0.28F), New Vector2(0.98F, 0.23F), New Vector2(0.65F, 0.74F), New Vector2(0.02F, 0.6F), New Vector2(-0.4F, 0.93F), New Vector2(-0.91F, 0.63F), New Vector2(-1F, 0.07F), New Vector2(-0.67F, -0.47F) }
		For i As Integer = 0 To 5 - 1
			Dim num As Integer = Global.UnityEngine.Random.Range(0, Me.particleVectors.Count)
			Global.UnityEngine.[Object].Instantiate(Of HealerCharmParticleEffect)(Me.particle, position, Quaternion.identity).SetVars(New Vector2(Me.particleVectors(num).x * MyBase.transform.localScale.x, Me.particleVectors(num).y), target, healerCharmSparkEffect)
			Me.particleVectors.RemoveAt(num)
		Next
	End Sub

	' Token: 0x06003CD6 RID: 15574 RVA: 0x0021A92C File Offset: 0x00218D2C
	Public Sub StartPlayerFlash()
		If Me.startedFlash >= 0 Then
			Me.startedFlash += 1
			If Me.startedFlash > 4 Then
				MyBase.StartCoroutine(Me.player_flash_cr())
				Me.startedFlash = -1
			End If
		End If
	End Sub

	' Token: 0x06003CD7 RID: 15575 RVA: 0x0021A968 File Offset: 0x00218D68
	Private Iterator Function player_flash_cr() As IEnumerator
		Dim wait As WaitForFrameTimePersistent = New WaitForFrameTimePersistent(0.041666668F, False)
		If Not Me.target.stats.SuperInvincible Then
			Dim levelPlayer As LevelPlayerController = TryCast(Me.target, LevelPlayerController)
			Dim planePlayer As PlanePlayerController = TryCast(Me.target, PlanePlayerController)
			Dim matInstance As Material = Nothing
			Dim playerRend As SpriteRenderer = Nothing
			If levelPlayer IsNot Nothing Then
				levelPlayer.animationController.SetMaterial(Me.flashMaterial)
				matInstance = levelPlayer.animationController.GetMaterial()
				playerRend = levelPlayer.animationController.GetSpriteRenderer()
			ElseIf planePlayer IsNot Nothing Then
				planePlayer.animationController.SetMaterial(Me.flashMaterial)
				matInstance = planePlayer.animationController.GetMaterial()
				playerRend = planePlayer.animationController.GetSpriteRenderer()
			End If
			Dim lightColor As Color = New Color(1F, 0.4509804F, 0.7882353F)
			Dim darkColor As Color = New Color(1F, 0.21960784F, 0.7019608F)
			matInstance.SetFloat("_Amount", 1F)
			playerRend.color = lightColor
			Yield wait
			playerRend.color = darkColor
			Yield wait
			playerRend.color = Color.white
			Yield wait
			matInstance.SetFloat("_Amount", 0F)
			Yield wait
			matInstance.SetFloat("_Amount", 1F)
			playerRend.color = darkColor
			Yield wait
			playerRend.color = Color.white
			Yield wait
			playerRend.color = lightColor
			Yield wait
			playerRend.color = Color.white
			If levelPlayer IsNot Nothing Then
				levelPlayer.animationController.SetOldMaterial()
			ElseIf planePlayer IsNot Nothing Then
				planePlayer.animationController.SetOldMaterial()
			End If
		End If
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04004428 RID: 17448
	<SerializeField()>
	Private flashMaterial As Material

	' Token: 0x04004429 RID: 17449
	<SerializeField()>
	Private particle As HealerCharmParticleEffect

	' Token: 0x0400442A RID: 17450
	Private particleVectors As List(Of Vector2)

	' Token: 0x0400442B RID: 17451
	Private startedFlash As Integer

	' Token: 0x0400442C RID: 17452
	Private target As AbstractPlayerController
End Class
