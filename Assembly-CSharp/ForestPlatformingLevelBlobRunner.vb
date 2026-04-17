Imports System
Imports System.Collections

' Token: 0x0200087B RID: 2171
Public Class ForestPlatformingLevelBlobRunner
	Inherits PlatformingLevelGroundMovementEnemy

	' Token: 0x0600326A RID: 12906 RVA: 0x001D5A03 File Offset: 0x001D3E03
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.idle_audio_delayer_cr("level_blobrunner", 2F, 4F))
		Me.emitAudioFromObject.Add("level_blobrunner")
	End Sub

	' Token: 0x0600326B RID: 12907 RVA: 0x001D5A37 File Offset: 0x001D3E37
	Protected Overrides Sub FixedUpdate()
		If Not Me.melted Then
			MyBase.FixedUpdate()
		End If
	End Sub

	' Token: 0x0600326C RID: 12908 RVA: 0x001D5A4A File Offset: 0x001D3E4A
	Protected Overrides Sub Die()
		Me.IdleSounds = False
		Me.melted = True
		Me.collider.enabled = False
		MyBase.StartCoroutine(Me.melt_cr())
	End Sub

	' Token: 0x0600326D RID: 12909 RVA: 0x001D5A74 File Offset: 0x001D3E74
	Private Iterator Function melt_cr() As IEnumerator
		AudioManager.[Stop]("level_blobrunner")
		If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING) Then
			AudioManager.Play("level_frogs_tall_firefly_death")
		End If
		MyBase.animator.Play("Melt")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Melt", False, True)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.BlobRunnerMeltDelay.RandomFloat())
		MyBase.animator.SetTrigger("Continue")
		AudioManager.Play("level_blobrunner_reform")
		Me.emitAudioFromObject.Add("level_blobrunner_reform")
		Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.BlobRunnerUnmeltLoopTime)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Unmelt", False, True)
		Me.melted = False
		Me.collider.enabled = True
		MyBase.Health = MyBase.Properties.Health
		Me.turning = False
		Me.timeSinceTurn = 10000F
		Me.IdleSounds = True
		Return
	End Function

	' Token: 0x04003ACB RID: 15051
	Private melted As Boolean
End Class
