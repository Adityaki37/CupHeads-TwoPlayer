Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008DE RID: 2270
Public Class MountainPlatformingLevelDragon
	Inherits PlatformingLevelShootingEnemy

	' Token: 0x0600351D RID: 13597 RVA: 0x001EEA04 File Offset: 0x001ECE04
	Protected Overrides Sub Start()
		MyBase.Start()
		AudioManager.Play("castle_dragon_spawn")
		Me.emitAudioFromObject.Add("castle_dragon_spawn")
	End Sub

	' Token: 0x0600351E RID: 13598 RVA: 0x001EEA26 File Offset: 0x001ECE26
	Public Sub Init(startPos As Vector3, endPos As Vector3)
		Me.startPos = startPos
		MyBase.transform.position = startPos
		Me.endPos = endPos
		MyBase.StartCoroutine(Me.move_to_pos_cr())
		MyBase.StartCoroutine(Me.check_cr())
	End Sub

	' Token: 0x0600351F RID: 13599 RVA: 0x001EEA5C File Offset: 0x001ECE5C
	Private Iterator Function move_to_pos_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = MyBase.Properties.dragonTimeIn
		Dim start As Vector2 = MyBase.transform.position
		Me._target = PlayerManager.GetNext()
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, Me.endPos, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.StartShoot()
		Yield Nothing
		Return
	End Function

	' Token: 0x06003520 RID: 13600 RVA: 0x001EEA78 File Offset: 0x001ECE78
	Private Iterator Function check_cr() As IEnumerator
		While MountainPlatformingLevelElevatorHandler.elevatorIsMoving
			Yield Nothing
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x06003521 RID: 13601 RVA: 0x001EEA93 File Offset: 0x001ECE93
	Protected Overrides Sub Shoot()
		MyBase.Shoot()
		AudioManager.Play("castle_dragon_attack")
		Me.emitAudioFromObject.Add("castle_dragon_attack")
		MyBase.StartCoroutine(Me.leave_cr())
	End Sub

	' Token: 0x06003522 RID: 13602 RVA: 0x001EEAC4 File Offset: 0x001ECEC4
	Protected Overrides Sub SpawnShootEffect()
		If MyBase.transform.localScale.x < 0F Then
			Me._effectRoot.localEulerAngles = New Vector3(0F, 0F, Me._effectRoot.localEulerAngles.z - 180F)
		End If
		If Me._shootEffect IsNot Nothing Then
			Dim effect As Effect = Me._shootEffect.Create(Me._effectRoot.position)
			effect.transform.rotation = Me._effectRoot.rotation
		End If
	End Sub

	' Token: 0x06003523 RID: 13603 RVA: 0x001EEB60 File Offset: 0x001ECF60
	Private Iterator Function leave_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = MyBase.Properties.dragonTimeOut
		MyBase.transform.position = Me.endPos
		Dim start As Vector2 = MyBase.transform.position
		Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.dragonLeaveDelay)
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, Me.startPos, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x06003524 RID: 13604 RVA: 0x001EEB7B File Offset: 0x001ECF7B
	Protected Overrides Sub Die()
		AudioManager.Play("castle_dragon_death")
		Me.emitAudioFromObject.Add("castle_dragon_death")
		MyBase.Die()
	End Sub

	' Token: 0x06003525 RID: 13605 RVA: 0x001EEB9D File Offset: 0x001ECF9D
	Private Sub ActivateTail()
		MyBase.animator.Play("Tail", 1)
	End Sub

	' Token: 0x04003D4C RID: 15692
	Private endPos As Vector3

	' Token: 0x04003D4D RID: 15693
	Private startPos As Vector3
End Class
