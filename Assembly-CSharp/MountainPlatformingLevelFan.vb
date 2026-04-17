Imports System
Imports System.Collections

' Token: 0x020008E2 RID: 2274
Public Class MountainPlatformingLevelFan
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x0600353B RID: 13627 RVA: 0x001F061E File Offset: 0x001EEA1E
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.check_to_start_cr())
	End Sub

	' Token: 0x0600353C RID: 13628 RVA: 0x001F0634 File Offset: 0x001EEA34
	Public Function GetSpeed() As Single
		If MyBase.transform.localScale.x = 1F Then
			Me.speed = -MyBase.Properties.fanVelocity
		Else
			Me.speed = MyBase.Properties.fanVelocity
		End If
		Return Me.speed
	End Function

	' Token: 0x0600353D RID: 13629 RVA: 0x001F068C File Offset: 0x001EEA8C
	Protected Overrides Sub OnStart()
		MyBase.StartCoroutine(Me.fan_cr())
	End Sub

	' Token: 0x0600353E RID: 13630 RVA: 0x001F069C File Offset: 0x001EEA9C
	Private Iterator Function check_to_start_cr() As IEnumerator
		While MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + Me.offset
			Yield Nothing
		End While
		Me.OnStart()
		Yield Nothing
		Return
	End Function

	' Token: 0x0600353F RID: 13631 RVA: 0x001F06B7 File Offset: 0x001EEAB7
	Private Sub FanOn()
		Me.PlayLionRoarSFX()
		Me.fanOn = True
	End Sub

	' Token: 0x06003540 RID: 13632 RVA: 0x001F06C6 File Offset: 0x001EEAC6
	Private Sub FanOff()
		Me.fanOn = False
	End Sub

	' Token: 0x06003541 RID: 13633 RVA: 0x001F06D0 File Offset: 0x001EEAD0
	Private Iterator Function fan_cr() As IEnumerator
		While True
			While MyBase.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + Me.offset OrElse MyBase.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - Me.offset
				Yield Nothing
			End While
			MyBase.animator.SetBool("IsFan", True)
			Yield Nothing
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
			MyBase.animator.SetBool("WindOn", True)
			Yield Nothing
			Dim t As Single = 0F
			Dim time As Single = MyBase.Properties.fanWaitTime.RandomFloat()
			While t < time
				t += CupheadTime.Delta
				Yield Nothing
			End While
			MyBase.animator.SetBool("IsFan", False)
			MyBase.animator.SetBool("WindOn", False)
			Yield Nothing
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Roar_End", False, True)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003542 RID: 13634 RVA: 0x001F06EB File Offset: 0x001EEAEB
	Private Sub PlayLionRoarSFX()
		AudioManager.Play("castle_rock_lion_roar")
		Me.emitAudioFromObject.Add("castle_rock_lion_roar")
	End Sub

	' Token: 0x06003543 RID: 13635 RVA: 0x001F0708 File Offset: 0x001EEB08
	Protected Overrides Sub Die()
		AudioManager.Play("castle_rock_lion_death")
		Me.emitAudioFromObject.Add("castle_rock_lion_death")
		MyBase.animator.SetBool("WindOn", False)
		Me.speed = 0F
		Me.fanOn = False
		Me.StopAllCoroutines()
		MyBase.animator.SetTrigger("Death")
		MyBase.Dead = True
	End Sub

	' Token: 0x04003D6B RID: 15723
	Private speed As Single

	' Token: 0x04003D6C RID: 15724
	Private offset As Single = 50F

	' Token: 0x04003D6D RID: 15725
	Public fanOn As Boolean
End Class
