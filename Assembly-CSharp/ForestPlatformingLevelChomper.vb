Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200087C RID: 2172
Public Class ForestPlatformingLevelChomper
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x0600326F RID: 12911 RVA: 0x001D5D3C File Offset: 0x001D413C
	Protected Overrides Sub OnStart()
		Me.startY = MyBase.transform.position.y
	End Sub

	' Token: 0x06003270 RID: 12912 RVA: 0x001D5D62 File Offset: 0x001D4162
	Public Sub StartAttacking()
		MyBase.StartCoroutine(Me.main_cr())
	End Sub

	' Token: 0x06003271 RID: 12913 RVA: 0x001D5D74 File Offset: 0x001D4174
	Private Iterator Function main_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.initialDelay.RandomFloat())
		Dim timeToApex As Single = Me.speed / Me.gravityUp
		Dim upAnimTime As Single = timeToApex - 0.333333F
		Dim normalizedExtraTime As Single = upAnimTime / 0.333333F Mod 1F
		While True
			If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(100F, 1000F)) Then
				AudioManager.Play("level_chomper_up")
				Me.emitAudioFromObject.Add("level_chomper_up")
			End If
			MyBase.animator.Play("Up", 0, 1F - normalizedExtraTime)
			MyBase.StartCoroutine(Me.move_cr())
			Yield CupheadTime.WaitForSeconds(Me, upAnimTime - 0.1666665F)
			If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(100F, 1000F)) Then
				AudioManager.Play("level_chomper_bite")
				Me.emitAudioFromObject.Add("level_chomper_bite")
			End If
			MyBase.animator.SetTrigger("Bite")
			While Me.moving
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, Me.mainDelay.RandomFloat())
		End While
		Return
	End Function

	' Token: 0x06003272 RID: 12914 RVA: 0x001D5D90 File Offset: 0x001D4190
	Private Iterator Function move_cr() As IEnumerator
		Dim timeToApex As Single = Me.speed / Me.gravityUp
		Dim t As Single = 0F
		Me.moving = True
		While t < timeToApex
			t += CupheadTime.FixedDelta
			MyBase.transform.SetPosition(Nothing, New Single?(Me.startY + Me.speed * t - 0.5F * Me.gravityUp * t * t), Nothing)
			Yield New WaitForFixedUpdate()
		End While
		Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
		Dim apexY As Single = MyBase.transform.position.y
		t = 0F
		While t < timeToApex
			t += CupheadTime.FixedDelta
			MyBase.transform.SetPosition(Nothing, New Single?(apexY - 0.5F * Me.gravityDown * t * t), Nothing)
			Yield New WaitForFixedUpdate()
		End While
		Me.moving = False
		Return
	End Function

	' Token: 0x04003ACC RID: 15052
	<SerializeField()>
	Private speed As Single = 1000F

	' Token: 0x04003ACD RID: 15053
	<SerializeField()>
	Private gravityUp As Single = 1600F

	' Token: 0x04003ACE RID: 15054
	<SerializeField()>
	Private gravityDown As Single = 2400F

	' Token: 0x04003ACF RID: 15055
	<SerializeField()>
	Private initialDelay As MinMax = New MinMax(0F, 0.5F)

	' Token: 0x04003AD0 RID: 15056
	<SerializeField()>
	Private mainDelay As MinMax = New MinMax(1F, 3F)

	' Token: 0x04003AD1 RID: 15057
	Private Const UP_ANIM_LENGTH As Single = 0.333333F

	' Token: 0x04003AD2 RID: 15058
	Private Const BITE_ANIM_TIME_TO_APEX As Single = 0.333333F

	' Token: 0x04003AD3 RID: 15059
	Private Const FREEZE_TIME As Single = 0.041666668F

	' Token: 0x04003AD4 RID: 15060
	Private Const ON_SCREEN_SOUND_PADDING As Single = 100F

	' Token: 0x04003AD5 RID: 15061
	Private startY As Single

	' Token: 0x04003AD6 RID: 15062
	Private moving As Boolean
End Class
