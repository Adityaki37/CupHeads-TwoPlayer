Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008CB RID: 2251
Public Class HarbourPlatformingLevelKrill
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x0600349A RID: 13466 RVA: 0x001E88B0 File Offset: 0x001E6CB0
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.gravity = MyBase.Properties.krillGravity
		Me.velocity.x = Global.UnityEngine.Random.Range(-MyBase.Properties.krillVelocityX.min, -MyBase.Properties.krillVelocityX.max)
		Me.velocity.y = Global.UnityEngine.Random.Range(MyBase.Properties.krillVelocityY.min, MyBase.Properties.krillVelocityY.max)
		Me._canParry = Me.isParryable
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x0600349B RID: 13467 RVA: 0x001E894F File Offset: 0x001E6D4F
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x0600349C RID: 13468 RVA: 0x001E8951 File Offset: 0x001E6D51
	Public Sub SetType(type As String)
		MyBase.GetComponent(Of PlatformingLevelEnemyAnimationHandler)().SelectAnimation(type)
	End Sub

	' Token: 0x0600349D RID: 13469 RVA: 0x001E8960 File Offset: 0x001E6D60
	Private Iterator Function move_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MyBase.Properties.krillLaunchDelay)
		Me.JumpSFX()
		While True
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.Delta, Me.velocity.y * CupheadTime.Delta, 0F)
			Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600349E RID: 13470 RVA: 0x001E897B File Offset: 0x001E6D7B
	Private Sub JumpSFX()
		AudioManager.Play("harbour_shrimp_jump")
		Me.emitAudioFromObject.Add("harbour_shrimp_jump")
	End Sub

	' Token: 0x0600349F RID: 13471 RVA: 0x001E8997 File Offset: 0x001E6D97
	Protected Overrides Sub Die()
		AudioManager.Play("harbour_krill_death")
		Me.emitAudioFromObject.Add("harbour_krill_death")
		MyBase.Die()
	End Sub

	' Token: 0x04003CC5 RID: 15557
	Private velocity As Vector2

	' Token: 0x04003CC6 RID: 15558
	Private gravity As Single

	' Token: 0x04003CC7 RID: 15559
	Public isParryable As Boolean
End Class
