Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008E4 RID: 2276
Public Class MountainPlatformingLevelFlamer
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x0600354A RID: 13642 RVA: 0x001F0BEB File Offset: 0x001EEFEB
	Protected Overrides Sub OnStart()
		Me.isDead = False
		Me.angle = 3.1415927F
		MyBase.transform.position = Me.startPos
		MyBase.StartCoroutine(Me.check_dist_cr())
	End Sub

	' Token: 0x0600354B RID: 13643 RVA: 0x001F0C20 File Offset: 0x001EF020
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.startPos = MyBase.transform.position
		Me.pivotPoint = New GameObject("pivotPoint")
		Me.pivotPoint.transform.position = New Vector3(MyBase.transform.position.x, MyBase.transform.position.y + 200F)
		Me.angle = 3.1415927F
		MyBase.StartCoroutine(Me.check_dist_cr())
	End Sub

	' Token: 0x0600354C RID: 13644 RVA: 0x001F0CB0 File Offset: 0x001EF0B0
	Private Sub PathMovement()
		Me.angle += Me.speed * CupheadTime.FixedDelta
		Dim vector As Vector3 = New Vector3(-Mathf.Sin(Me.angle) * Me.loopSize, 0F, 0F)
		Dim vector2 As Vector3 = New Vector3(0F, Mathf.Cos(Me.angle) * Me.loopSize, 0F)
		MyBase.transform.position = Me.pivotPoint.transform.position
		MyBase.transform.position += vector + vector2
	End Sub

	' Token: 0x0600354D RID: 13645 RVA: 0x001F0D55 File Offset: 0x001EF155
	Private Sub MovePivot()
		Me.pivotPoint.transform.AddPosition(Me.moveSpeed * CupheadTime.FixedDelta, 0F, 0F)
	End Sub

	' Token: 0x0600354E RID: 13646 RVA: 0x001F0D80 File Offset: 0x001EF180
	Private Iterator Function check_dist_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Me.player = PlayerManager.GetNext()
		Dim movingLeft As Boolean = MyBase.transform.position.x > Me.player.transform.position.x
		Me.moveSpeed = If((Not movingLeft), MyBase.Properties.flamerXSpeed.RandomFloat(), (-MyBase.Properties.flamerXSpeed.RandomFloat()))
		AudioManager.PlayLoop("castle_flamer_loop")
		Me.emitAudioFromObject.Add("castle_flamer_loop")
		MyBase.animator.SetTrigger("OnFlame")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Flame_Appear", False, True)
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.StartCoroutine(Me.move_cr())
		Return
	End Function

	' Token: 0x0600354F RID: 13647 RVA: 0x001F0D9C File Offset: 0x001EF19C
	Private Iterator Function move_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.startDelayRange.RandomFloat())
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		MyBase.StartCoroutine(Me.accelerate_speed_cr())
		While Not Me.isDead
			Me.PathMovement()
			Me.MovePivot()
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06003550 RID: 13648 RVA: 0x001F0DB8 File Offset: 0x001EF1B8
	Private Iterator Function accelerate_speed_cr() As IEnumerator
		Dim incrementBy As Single = 1F
		While Me.speed < MyBase.Properties.flamerCirSpeed AndAlso Not Me.isDead
			Me.speed += incrementBy * CupheadTime.Delta
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06003551 RID: 13649 RVA: 0x001F0DD3 File Offset: 0x001EF1D3
	Private Sub PlayFace()
		MyBase.animator.Play("Flame_Face", 1)
	End Sub

	' Token: 0x06003552 RID: 13650 RVA: 0x001F0DE6 File Offset: 0x001EF1E6
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.DrawWireSphere(MyBase.transform.position, 100F)
	End Sub

	' Token: 0x06003553 RID: 13651 RVA: 0x001F0E03 File Offset: 0x001EF203
	Protected Overrides Sub Die()
		Me.Deactivate()
	End Sub

	' Token: 0x06003554 RID: 13652 RVA: 0x001F0E0C File Offset: 0x001EF20C
	Private Sub Deactivate()
		Me.isDead = True
		AudioManager.[Stop]("castle_flamer_loop")
		Me.speed = 0F
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.Play("Flame_Appear_Loop", 0)
		MyBase.animator.Play("Off", 1)
		MyBase.StartCoroutine(Me.activate_cr())
	End Sub

	' Token: 0x06003555 RID: 13653 RVA: 0x001F0E70 File Offset: 0x001EF270
	Private Iterator Function activate_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.respawnRange.RandomFloat())
		Me.OnStart()
		Return
	End Function

	' Token: 0x04003D70 RID: 15728
	<SerializeField()>
	Private loopSize As Single

	' Token: 0x04003D71 RID: 15729
	<SerializeField()>
	Private startDelayRange As MinMax

	' Token: 0x04003D72 RID: 15730
	<SerializeField()>
	Private respawnRange As MinMax

	' Token: 0x04003D73 RID: 15731
	Private pivotPoint As GameObject

	' Token: 0x04003D74 RID: 15732
	Private angle As Single

	' Token: 0x04003D75 RID: 15733
	Private speed As Single

	' Token: 0x04003D76 RID: 15734
	Private moveSpeed As Single

	' Token: 0x04003D77 RID: 15735
	Private player As AbstractPlayerController

	' Token: 0x04003D78 RID: 15736
	Private startPos As Vector3

	' Token: 0x04003D79 RID: 15737
	Private isDead As Boolean
End Class
