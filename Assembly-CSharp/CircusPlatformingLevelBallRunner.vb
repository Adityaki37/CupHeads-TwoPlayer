Imports System
Imports UnityEngine

' Token: 0x0200089C RID: 2204
Public Class CircusPlatformingLevelBallRunner
	Inherits PlatformingLevelPathMovementEnemy

	' Token: 0x0600334C RID: 13132 RVA: 0x001DDC60 File Offset: 0x001DC060
	Protected Overrides Sub Die()
		AudioManager.Play("circus_generic_death_fun")
		Me.emitAudioFromObject.Add("circus_generic_death_fun")
		Me.ball.transform.parent = Nothing
		Me.ball.isMoving = True
		Me.ball.direction = New Vector3(CSng(Me._direction), 0F, 0F)
		MyBase.Die()
	End Sub

	' Token: 0x0600334D RID: 13133 RVA: 0x001DDCCC File Offset: 0x001DC0CC
	Private Sub IdleSFX()
		If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(100F, 1000F)) Then
			AudioManager.Play("circus_ball_runner_idle")
			Me.emitAudioFromObject.Add("circus_ball_runner_idle")
		End If
	End Sub

	' Token: 0x04003B98 RID: 15256
	Private Const ON_SCREEN_SOUND_PADDING As Single = 100F

	' Token: 0x04003B99 RID: 15257
	<SerializeField()>
	Private ball As CircusPlatformingLevelBallRunnerBall

	' Token: 0x04003B9A RID: 15258
	<SerializeField()>
	Private ballRoot As Transform
End Class
