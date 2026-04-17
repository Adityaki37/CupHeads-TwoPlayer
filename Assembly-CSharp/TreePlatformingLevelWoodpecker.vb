Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000898 RID: 2200
Public Class TreePlatformingLevelWoodpecker
	Inherits PlatformingLevelShootingEnemy

	' Token: 0x06003329 RID: 13097 RVA: 0x001DC58C File Offset: 0x001DA98C
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.isDown = False
		Me.startPos = MyBase.transform.position
		Me.endPos = Me.setEndPos.transform.position
		Me.midPos = New Vector3(Me.endPos.x, Me.endPos.y + 200F)
		MyBase.GetComponent(Of DamageReceiver)().enabled = False
	End Sub

	' Token: 0x0600332A RID: 13098 RVA: 0x001DC60F File Offset: 0x001DAA0F
	Protected Overrides Sub Shoot()
		If Not Me.isDown Then
			MyBase.StartCoroutine(Me.move_down_cr())
		End If
	End Sub

	' Token: 0x0600332B RID: 13099 RVA: 0x001DC62C File Offset: 0x001DAA2C
	Private Iterator Function move_down_cr() As IEnumerator
		Me.isDown = True
		MyBase.animator.SetBool("movingDown", True)
		Dim t As Single = 0F
		Dim start As Vector2 = MyBase.transform.position
		While t < MyBase.Properties.WoodpeckermoveDownTime
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / MyBase.Properties.WoodpeckermoveDownTime)
			MyBase.transform.position = Vector2.Lerp(start, Me.midPos, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = Me.midPos
		start = MyBase.transform.position
		Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.WoodpeckerWarningDuration)
		MyBase.animator.SetTrigger("Continue")
		t = 0F
		While t < 0.2F
			Dim val2 As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / 0.5F)
			MyBase.transform.position = Vector2.Lerp(start, Me.endPos, val2)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		t = 0F
		MyBase.transform.position = Me.endPos
		start = MyBase.transform.position
		MyBase.animator.SetBool("isAttacking", True)
		CupheadLevelCamera.Current.Shake(10F, MyBase.Properties.WoodpeckerAttackDuration, False)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.WoodpeckerAttackDuration)
		MyBase.animator.SetBool("isAttacking", False)
		While t < MyBase.Properties.WoodpeckermoveUpTime
			Dim val3 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / MyBase.Properties.WoodpeckermoveUpTime)
			MyBase.transform.position = Vector2.Lerp(start, Me.startPos, val3)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetBool("movingDown", False)
		Me.isDown = False
		Yield Nothing
		Return
	End Function

	' Token: 0x0600332C RID: 13100 RVA: 0x001DC647 File Offset: 0x001DAA47
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = New Color(0F, 1F, 0F, 1F)
		Gizmos.DrawWireSphere(Me.endPos, 100F)
	End Sub

	' Token: 0x0600332D RID: 13101 RVA: 0x001DC682 File Offset: 0x001DAA82
	Private Sub SoundWoodpeckerStart()
		AudioManager.Play("level_platform_woodpecker_attack_start")
		Me.emitAudioFromObject.Add("level_platform_woodpecker_attack_start")
	End Sub

	' Token: 0x04003B6C RID: 15212
	<SerializeField()>
	Private setEndPos As Transform

	' Token: 0x04003B6D RID: 15213
	Private endPos As Vector2

	' Token: 0x04003B6E RID: 15214
	Private midPos As Vector2

	' Token: 0x04003B6F RID: 15215
	Private startPos As Vector2

	' Token: 0x04003B70 RID: 15216
	Private isDown As Boolean
End Class
