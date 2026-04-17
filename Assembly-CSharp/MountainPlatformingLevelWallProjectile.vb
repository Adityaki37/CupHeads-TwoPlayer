Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008F3 RID: 2291
Public Class MountainPlatformingLevelWallProjectile
	Inherits AbstractProjectile

	' Token: 0x060035BE RID: 13758 RVA: 0x001F58CC File Offset: 0x001F3CCC
	Public Function Create(pos As Vector2, rotation As Single, velocity As Vector2, gravity As Single, yGround As Single) As MountainPlatformingLevelWallProjectile
		Dim mountainPlatformingLevelWallProjectile As MountainPlatformingLevelWallProjectile = TryCast(MyBase.Create(), MountainPlatformingLevelWallProjectile)
		mountainPlatformingLevelWallProjectile.transform.position = pos
		mountainPlatformingLevelWallProjectile.velocity = velocity
		mountainPlatformingLevelWallProjectile.startVelocity = velocity
		mountainPlatformingLevelWallProjectile.gravity = gravity
		mountainPlatformingLevelWallProjectile.yGround = yGround
		mountainPlatformingLevelWallProjectile.transform.SetEulerAngles(Nothing, Nothing, New Single?(rotation))
		Return mountainPlatformingLevelWallProjectile
	End Function

	' Token: 0x060035BF RID: 13759 RVA: 0x001F5938 File Offset: 0x001F3D38
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.timeToApex = Mathf.Sqrt(2F * Me.velocity.y / Me.gravity)
		Me.startVelocity.y = Me.timeToApex * Me.gravity
		MyBase.StartCoroutine(Me.check_to_kill_cr())
	End Sub

	' Token: 0x060035C0 RID: 13760 RVA: 0x001F5994 File Offset: 0x001F3D94
	Private Iterator Function handle_hit_ground_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Hit", False, True)
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Me.onGround = False
		Yield Nothing
		Return
	End Function

	' Token: 0x060035C1 RID: 13761 RVA: 0x001F59AF File Offset: 0x001F3DAF
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060035C2 RID: 13762 RVA: 0x001F59D0 File Offset: 0x001F3DD0
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		If Me.onGround Then
			Return
		End If
		If MyBase.transform.position.y <= Me.yGround Then
			Me.onGround = True
			Me.HandleHitGround()
		End If
	End Sub

	' Token: 0x060035C3 RID: 13763 RVA: 0x001F5A30 File Offset: 0x001F3E30
	Private Sub HandleHitGround()
		Me.velocity.y = Me.startVelocity.y
		MyBase.animator.SetTrigger("OnHitGround")
		MyBase.StartCoroutine(Me.handle_hit_ground_cr())
	End Sub

	' Token: 0x060035C4 RID: 13764 RVA: 0x001F5A68 File Offset: 0x001F3E68
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") Then
			Return
		End If
		MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
		Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.FixedDelta
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(Mathf.Atan2(-Me.velocity.y, -Me.velocity.x) * 57.29578F))
	End Sub

	' Token: 0x060035C5 RID: 13765 RVA: 0x001F5B30 File Offset: 0x001F3F30
	Private Sub ChangeRootEnd()
		MyBase.transform.position = Me.root.transform.position
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(Mathf.Atan2(-Me.velocity.y, -Me.velocity.x) * 57.29578F))
	End Sub

	' Token: 0x060035C6 RID: 13766 RVA: 0x001F5BA0 File Offset: 0x001F3FA0
	Private Sub ChangeRootBeginning()
		AudioManager.Play("castle_mountain_wall_oil_bounce")
		Me.emitAudioFromObject.Add("castle_mountain_wall_oil_bounce")
		MyBase.transform.position = Me.root1.transform.position
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
	End Sub

	' Token: 0x060035C7 RID: 13767 RVA: 0x001F5C0C File Offset: 0x001F400C
	Private Iterator Function check_to_kill_cr() As IEnumerator
		While CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(0F, 1000F))
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04003DD2 RID: 15826
	<SerializeField()>
	Private root As Transform

	' Token: 0x04003DD3 RID: 15827
	<SerializeField()>
	Private root1 As Transform

	' Token: 0x04003DD4 RID: 15828
	Private velocity As Vector2

	' Token: 0x04003DD5 RID: 15829
	Private startVelocity As Vector2

	' Token: 0x04003DD6 RID: 15830
	Private gravity As Single

	' Token: 0x04003DD7 RID: 15831
	Private timeToApex As Single

	' Token: 0x04003DD8 RID: 15832
	Private yGround As Single

	' Token: 0x04003DD9 RID: 15833
	Private onGround As Boolean
End Class
