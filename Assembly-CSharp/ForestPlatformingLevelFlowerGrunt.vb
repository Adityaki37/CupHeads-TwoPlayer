Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200087F RID: 2175
Public Class ForestPlatformingLevelFlowerGrunt
	Inherits PlatformingLevelGroundMovementEnemy

	' Token: 0x0600327C RID: 12924 RVA: 0x001D6494 File Offset: 0x001D4894
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.StartCoroutine(Me.idle_audio_delayer_cr("level_flowergrunt", 2F, 4F))
		Me.emitAudioFromObject.Add("level_flowergrunt")
		Me.emitAudioFromObject.Add("level_flowergrunt_float")
	End Sub

	' Token: 0x0600327D RID: 12925 RVA: 0x001D64E3 File Offset: 0x001D48E3
	Protected Overrides Sub OnStart()
		MyBase.OnStart()
		If Me.floating Then
			AudioManager.Play("level_flowergrunt_float")
			MyBase.StartCoroutine(Me.handle_float_cr())
		End If
	End Sub

	' Token: 0x0600327E RID: 12926 RVA: 0x001D6510 File Offset: 0x001D4910
	Private Iterator Function handle_float_cr() As IEnumerator
		While Me.floating
			Yield Nothing
		End While
		AudioManager.Play("level_flowergrunt_land")
		Me.emitAudioFromObject.Add("level_flowergrunt_land")
		Return
	End Function

	' Token: 0x0600327F RID: 12927 RVA: 0x001D652B File Offset: 0x001D492B
	Protected Overrides Sub Die()
		AudioManager.Play("level_flowergrunt_death")
		Me.emitAudioFromObject.Add("level_flowergrunt_death")
		MyBase.FrameDelayedCallback(AddressOf Me.Kill, 1)
	End Sub

	' Token: 0x06003280 RID: 12928 RVA: 0x001D655B File Offset: 0x001D495B
	Private Sub Kill()
		MyBase.Die()
	End Sub

	' Token: 0x06003281 RID: 12929 RVA: 0x001D6564 File Offset: 0x001D4964
	Private Function adjustSpeed(speed As Single) As Single
		Return Global.UnityEngine.Random.Range(speed * 0.12F, speed)
	End Function
End Class
